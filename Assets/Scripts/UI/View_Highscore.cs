using UnityEngine;

public class View_Highscore : View_Base
{
    [Header("Highscore Section")]
    [SerializeField] private UIElement_Base m_titleElement;
    [SerializeField] private UIElement_Time m_timerElement;
    // Every section contains: Completion image | Title for type of quest
    
    [SerializeField] private UIElement_HighScoreQuestSection m_sectionKillElement;
    [SerializeField] private UIElement_HighScoreQuestSection m_sectionPickupElement;
    [SerializeField] private UIElement_HighScoreQuestSection m_sectionExtractElement;
    [SerializeField] private UIElement_HighScoreQuestSection m_sectionKillWithSpecificElement;

    public UIElement_Base TitleElement { get => m_titleElement; }
    public UIElement_Time TimerElement { get => m_timerElement; }
    public UIElement_HighScoreQuestSection SectionKillElement { get => m_sectionKillElement; set => m_sectionKillElement = value; }
    public UIElement_HighScoreQuestSection SectionKillWithSpecificElement { get => m_sectionKillWithSpecificElement; }
    public UIElement_HighScoreQuestSection SectionPickupElement { get => m_sectionPickupElement; }
    public UIElement_HighScoreQuestSection SectionExtractElement { get => m_sectionExtractElement; }

    public void SetElementScaleToOne()
    {
        m_timerElement.MyTitle.transform.localScale = Vector3.one;
        m_sectionKillElement.transform.localScale = Vector3.one;
        m_sectionPickupElement.transform.localScale = Vector3.one;
        m_sectionExtractElement.transform.localScale = Vector3.one;
        m_sectionKillWithSpecificElement.transform.localScale = Vector3.one;
    }

    public void SetHighscoreView()
    {
        //UIManager.Instance.View_highscore.TitleElement.SetTitle("HIGH SCORE");
        // 
        TimerElement.ExecuteVerticalBlink(_elementToBlink: m_timerElement.MyTitle.transform, callback: () =>
        {
            m_timerElement.SetTitle(HighScoreManager.Instance.highScoreScriptable.bestTime);
        });

        // Section Kill
        SectionKillElement.ExecuteVerticalBlink(callback: () =>
        {
            m_sectionKillElement.SetQuestSuccessState(HighScoreManager.Instance.highScoreScriptable.questKillDone);
        });

        // Section Pickup
        SectionPickupElement.ExecuteVerticalBlink(callback: () =>
        {
            m_sectionPickupElement.SetQuestSuccessState(HighScoreManager.Instance.highScoreScriptable.questPickupDone);
        });

        // Section Extract
        SectionExtractElement.ExecuteVerticalBlink(callback: () =>
        {
            m_sectionExtractElement.SetQuestSuccessState(HighScoreManager.Instance.highScoreScriptable.questExtractDone);
        });
    }
    
    public void SetHighscoreViewCurrentPlay()
    {
        m_timerElement.SetTitle(HighScoreManager.Instance.CurrentTimer);
        m_sectionKillElement.SetQuestSuccessState(HighScoreManager.Instance.highScoreScriptable.questKillDone);
        m_sectionPickupElement.SetQuestSuccessState(HighScoreManager.Instance.highScoreScriptable.questPickupDone);
        m_sectionExtractElement.SetQuestSuccessState(HighScoreManager.Instance.highScoreScriptable.questExtractDone);
    }

    public void SetTimerDirty()
    {
        m_timerElement.SetTitle(HighScoreManager.Instance.CurrentTimer);
    }
}
