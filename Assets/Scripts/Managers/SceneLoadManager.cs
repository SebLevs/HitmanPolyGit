using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : Manager<SceneLoadManager>
{
    public SpawningPointsContainer SceneStartingPoint { get; set; }

    // Keep for unload
    public int CurrentScene { get; set; }
    public int NextScene { get; set; }

    private const float minimalWaitTime = 1.5f;
    private AsyncOperation async;

    public void OnLoadScene(string scene)
    {
        async = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        //async.allowSceneActivation = false;
        StartCoroutine(LoadAsync());
        CurrentScene = SceneManager.GetSceneByName(scene).buildIndex;
    }

    public void OnLoadScene(int scene)
    {
        Entity_Player.Instance.ResetPlayer();

        // TODO: Delete when found why player don<t reset position at 000
        if (UIManager.Instance.View_Alert.gameObject.activeSelf) { UIManager.Instance.View_Alert.OnQuickHide(); } 

        UIManager.Instance.View_loadingBarLevel.OnShow();
        (UIManager.Instance.View_loadingBarLevel.FillingBarElement as UIElement_Extraction).ActivateTitleCue();
        UIManager.Instance.View_loadingBarLevel.FillingBarElement.SetFilling(0);

        async = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        //async.allowSceneActivation = false;
        StartCoroutine(LoadAsync());
        CurrentScene = scene;
    }

    private IEnumerator LoadAsync()
    {
        UIManager _uiManager = UIManager.Instance;
        while (async.progress < 0.95f)
        {
            yield return new WaitForFixedUpdate();
            _uiManager.View_loadingBarLevel.FillingBarElement.SetFilling(async.progress);
        }
        yield return new WaitUntil(() =>  async.progress > 0.95f );
        yield return new WaitForSeconds(minimalWaitTime);

        _uiManager.View_loadingBarLevel.FillingBarElement.SetFilling(1.0f);
        _uiManager.View_loadingBarLevel.OnHide();

        //async.allowSceneActivation = true;
        async = null;
        InitScene();
    }

    private void InitScene()
    {
        // Manager References
        Entity_Player _player = Entity_Player.Instance;
        UIManager _uiManager = UIManager.Instance;
        QuestManager _questManager = QuestManager.Instance;

        // Spawn _player
        Transform startAt = SceneStartingPoint.GetRandomPoint();
        _player.transform.position = new Vector3(startAt.position.x, _player.transform.position.y, startAt.transform.position.z);

        // Enemy
        EnemyManager.Instance.InitEnemySkin();
        _uiManager.View_Target.SetTargetMeshOnUI();
        _uiManager.View_Quest.OnResetDestroyAllUIElement();
        ExtractManager.Instance.ChooseExtractPoints();
        EnemyManager.Instance.DeactivateAlert();

        _uiManager.HideHUD();

        // Side quests
        HighScoreManager.Instance.ResetGameScore();
        _questManager.CreateAllQuests();
        _uiManager.View_Quest.MainQuest.InitQuestTexts(_questManager.mainQuest.questTitle, _questManager.mainQuest.questDescription);
        _questManager.currentExtractQuest.elem = _uiManager.View_Quest.AddSideQuest(_questManager.currentExtractQuest.questTitle, _questManager.currentExtractQuest.questDescription);
        _questManager.currentKillQuest.elem = _uiManager.View_Quest.AddSideQuest(_questManager.currentKillQuest.questTitle, _questManager.currentKillQuest.questDescription);
        _questManager.currentPickupQuest.elem = _uiManager.View_Quest.AddSideQuest(_questManager.currentPickupQuest.questTitle, _questManager.currentPickupQuest.questDescription);

        UIManager.Instance.View_highscore.SetHighscoreViewCurrentPlay();
        // Hide black screen > Enable HUD & input
        _uiManager.OnSwitchViewSynchronous(UIManager.Instance.View_Target, showCallback: () =>
        {
            _uiManager.ShowHUD();
            _player.Input.enabled = true;

            if (!GameManager.Instance.HasSeenTutorial.Value)
            {
                _uiManager.View_tutorial.OnShow(callback: () =>
                {
                    _uiManager.View_tutorial.TextPrinterElement.OnPrintCharByChar();
                });
            }
            else
            {
                HighScoreManager.Instance.StartTimer();
            }

            _uiManager.View_highscore.TitleElement.SetTitle("CURRENT SCORE");
            _uiManager.View_highscore.SetElementScaleToOne();
            _player.DesiredActions.PurgeAllAction();

            _player.canUseGravity = true;
            _player.Gravity = _player.MaxGravity;
        });
    }

    public void UnloadCurrentSceneAsync()
    {
        Entity_Player.Instance.canUseGravity = false;
        //SceneManager.UnloadSceneAsync(CurrentScene);
        SceneManager.UnloadSceneAsync(1);
    }

    public void OnLoadMainMenu(bool unloadScene = false)
    {
        Entity_Player _player = Entity_Player.Instance;
        _player.canCrouch = true;
        OnClearBeforeLeavingLevel();

        UIManager.Instance.OnSwitchViewSynchronous(UIManager.Instance.View_LoadingLevel, showCallback: () =>
        {
            if (unloadScene)
            {
                UnloadCurrentSceneAsync();
            }

            _player.transform.position = Vector3.zero;
            
            UIManager.Instance.OnSwitchViewSynchronous(UIManager.Instance.View_MainMenu);
        });
    }

    public void OnClearBeforeLeavingLevel()
    {
        Entity_Player.Instance.DropSockets();

        UIManager.Instance.HideHUD();

        ExtractManager.Instance.ClearExtracts();
        PickupManager.Instance.pickableList.Clear();
    }
}
