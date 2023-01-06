using System;
using UnityEngine;

public class UIManager : Manager<UIManager>
{
    [SerializeField] private FloatVariable _characterPrintSpeed; // TODO: May need moving into a SystemManager or something
    [SerializeField] private FloatVariable _linePrintPauseBetween; // TODO: May need moving into a SystemManager or something

    // the current "menu" | For debug purpose only (delete and use property bellow when finished)
    [SerializeField] private View_Base m_currentView; 

    [Header("Views")]
    [SerializeField] private View_MainMenu m_view_mainMenu;
    [SerializeField] private View_LoadingLevel m_view_loadingLevel;
    [SerializeField] private View_Crosshair m_view_crosshair;
    [SerializeField] private View_Target m_view_target;
    [SerializeField] private View_Quest m_view_quest;
    [SerializeField] private View_Alarm m_view_alert;
    [SerializeField] private View_Extraction m_view_extraction;
    [SerializeField] private View_Infos m_view_infos;
    [SerializeField] private View_QuestCompletion m_view_questCompletion;
    [SerializeField] private View_Highscore m_view_highscore;
    [SerializeField] private View_FillingBar m_view_loadingBarLevel;
    [SerializeField] private View_DeathScreen m_view_deathScreen;
    [SerializeField] private View_TitleScreen m_view_titleScreen;
    [SerializeField] private View_TextPrinter m_view_tutorial;

    [SerializeField] private View_Base m_view_debug;

    public FloatVariable CharacterPrintSpeed { get => _characterPrintSpeed; }
    public FloatVariable LinePrintPauseBetween { get => _linePrintPauseBetween; }

    public View_Base CurrentView { get => m_currentView; set => m_currentView = value; }
    public View_MainMenu View_MainMenu { get => m_view_mainMenu; }
    public View_LoadingLevel View_LoadingLevel { get => m_view_loadingLevel; }
    public View_Crosshair View_Crosshair { get => m_view_crosshair; }
    public View_Target View_Target { get => m_view_target; }
    public View_Quest View_Quest { get => m_view_quest; }
    public View_Alarm View_Alert { get => m_view_alert; }
    public View_Extraction View_Extraction { get => m_view_extraction; }
    public View_Infos View_Infos { get => m_view_infos; }
    public View_QuestCompletion View_QuestCompletion { get => m_view_questCompletion; }
    public View_Highscore View_highscore { get => m_view_highscore; }
    public View_FillingBar View_loadingBarLevel { get => m_view_loadingBarLevel; }
    public View_DeathScreen View_deathScreen { get => m_view_deathScreen; }
    public View_TitleScreen View_titleScreen { get => m_view_titleScreen; }
    public View_TextPrinter View_tutorial { get => m_view_tutorial; }
    public View_Base View_debug { get => m_view_debug; }

    /// <summary>
    /// Syncronous switch view: <br/>
    /// OnHide AND OnShow at the same time
    /// <br/><br/>
    /// OnHide() current view<br/>
    /// OnShow() newly selected view
    /// </summary>
    public void OnSwitchViewSynchronous(View_Base _newView, Action hideCallback = null, Action showCallback = null)
    {
        // Hide currently selected view
        if (m_currentView)
        {
            m_currentView.StopAllCoroutines();
            m_currentView.OnHide(hideCallback);
        }

        m_currentView = _newView;

        // m_currentStateView is set at end of onShow
        _newView.StopAllCoroutines();
        _newView.OnShow(showCallback);
    }

    /// <summary>
    /// Squential switch view: <br/>
    /// OnHide THEN OnShow
    /// <br/><br/>
    /// OnHide() current view<br/>
    /// OnShow() newly selected view
    /// </summary>
    public void OnSwitchViewSequential(View_Base _newView, Action hideCallback = null, Action showCallback = null)
    {
        // Hide currently selected view
        if (m_currentView)
        {
            m_currentView.StopAllCoroutines();
            m_currentView.OnHide( () => 
            {
                if (hideCallback != null) { hideCallback.Invoke(); }
                // m_currentStateView is set at end of onShow
                m_currentView = _newView;
                _newView.StopAllCoroutines();
                _newView.OnShow(showCallback);
            });
        }
    }

    /// <summary>
    /// Show every HUD <br/>
    /// No transition is applied
    /// </summary>
    public void ShowHUD()
    {
        m_view_infos.OnShow();
    }

    /// <summary>
    /// Hide every HUD View
    /// No transition is applied
    /// </summary>
    public void HideHUD()
    {
        if (m_view_crosshair.gameObject.activeSelf && View_Crosshair != CurrentView)
        {
            View_Crosshair.OnQuickHide();
        }

        if (m_view_target.gameObject.activeSelf && View_Target != CurrentView)
        {
            View_Target.OnQuickHide();
        }

        if (m_view_alert.gameObject.activeSelf)
        {
            View_Alert.OnQuickHide();
        }

        if (m_view_extraction.gameObject.activeSelf)
        {
            View_Extraction.OnQuickHide();
        }

        if (m_view_infos.gameObject.activeSelf)
        {
            m_view_infos.OnQuickHide();
        }

/*        if (m_view_deathScreen.gameObject.activeSelf)
        {
            m_view_deathScreen.OnHide();
        }*/
    }
    
    public void OnTransitionFromTitleScreen()
    {
        if (CurrentView != m_view_titleScreen) { return;  }

        OnSwitchViewSequential(m_view_mainMenu);
    }

    public void OnTransitionToTitleScreen()
    {
        if (CurrentView != m_view_mainMenu) { return; }

        OnSwitchViewSequential(m_view_titleScreen);
    }
}
