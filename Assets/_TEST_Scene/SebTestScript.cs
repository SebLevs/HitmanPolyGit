using UnityEngine;

public class SebTestScript : MonoBehaviour
{
    public bool isTimerAddition = false;
    private int timerCounter = 0;
    public bool isQuestComplete = false;
    public bool isHighScoreUpdated = false;
    public bool isAlert = false;
    public bool fakeKillPlayer = false;

    private void Update()
    {
        //UIManager.Instance.View_debug.DefaultSelectedElement.SetTitle(Entity_Player.Instance.transform.position.ToString());
        if (isTimerAddition)
        {
            isTimerAddition = false;
            TimerManager.Instance.AddSequentialTimer(2.5f, () => { Debug.Log($"TIMER AT INDEX {timerCounter++} ENDED"); });
        }

        //UIManager.Instance.View_debug.DefaultSelectedElement.SetTitle(Entity_Player.Instance.transform.position.ToString());
        if (isQuestComplete)
        {
            isQuestComplete = false;
            UIManager.Instance.View_QuestCompletion.OnPopupQuestCompletion(true, "Pickup the football at the gas station");
        }

        if (isHighScoreUpdated)
        {
            isHighScoreUpdated = false;
            // Setup some arbitrary highscore values here

            // Time
            View_Highscore view_highscore = UIManager.Instance.View_highscore;

            view_highscore.TimerElement.ExecuteVerticalBlink(_elementToBlink: view_highscore.TimerElement.MyTitle.transform, callback: () =>
            {
                view_highscore.TimerElement.SetTitle(3600f);
            });

            // Section Kill
            view_highscore.SectionKillElement.ExecuteVerticalBlink(callback: () =>
            {
                view_highscore.SectionKillElement.SetQuestSuccessState(true);
            });

            // Section Pickup
            view_highscore.SectionPickupElement.ExecuteVerticalBlink(callback: () =>
            {
                view_highscore.SectionPickupElement.SetQuestSuccessState(true);
            });

            // Section Extract
            view_highscore.SectionExtractElement.ExecuteVerticalBlink(callback: () =>
            {
                view_highscore.SectionExtractElement.SetQuestSuccessState(false, "This is a custom optional text override");
            });
        }

        if (isAlert)
        {
            isAlert = false;
            UIManager.Instance.View_Alert.OnShow();
        }

        if (fakeKillPlayer)
        {
            fakeKillPlayer = false;

            SceneLoadManager.Instance.OnClearBeforeLeavingLevel();
            UIManager _uIManager = UIManager.Instance;
            _uIManager.OnSwitchViewSynchronous(_uIManager.View_deathScreen);
        }
    }
}
