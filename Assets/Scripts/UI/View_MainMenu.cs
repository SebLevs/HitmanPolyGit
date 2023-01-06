using UnityEngine;
using System;

public class View_MainMenu : View_Base
{
/*    [SerializeField] private GameObject m_quitButton;

    public GameObject QuitButton { get => m_quitButton; }*/

    public override void OnShow(Action callback = null)
    {
        Entity_Player _player = Entity_Player.Instance;
        UIManager _uiManager = UIManager.Instance;

        _player.OnSetCrouch(false);
        _player.ResetPlayer();

        _uiManager.HideHUD(); // just in case
        _uiManager.View_highscore.TitleElement.SetTitle("HIGH SCORE");
        _uiManager.View_Quest.OnResetDestroyAllUIElement();
        _uiManager.View_Quest.ResetQuestsTexts();
        
        _uiManager.View_highscore.OnShow();
        base.OnShow(() => {
            if (callback != null) { callback.Invoke(); }
            m_canvasGroup.interactable = true;
            m_defaultSelectedElement.OnSelect();
            GameManager.Instance.SetCursor(true, CursorLockMode.Confined);
            EnemyManager.Instance.EnemyList.Clear();
            _uiManager.View_highscore.SetHighscoreView();
        });
    }

    public void OnLaunchGame()
    {
        GameManager.Instance.IsGameOver = false;
        GameManager.Instance.SetCursor(false, CursorLockMode.Locked);
        View_LoadingLevel view_LoadingLevel = UIManager.Instance.View_LoadingLevel;
        // TODO: To be set to proper scene when multiple scenes are implemented
        SceneLoadManager.Instance.NextScene = 1; // set for delayed load (View_LoadingLevel)

        UIManager.Instance.View_highscore.OnHide();
        UIManager.Instance.OnSwitchViewSynchronous(view_LoadingLevel, showCallback: () => 
        {
            SceneLoadManager.Instance.OnLoadScene(SceneLoadManager.Instance.NextScene);
        });
    }
}
