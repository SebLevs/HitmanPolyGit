using UnityEngine;

public class GameManager : Manager<GameManager>
{
    [SerializeField] private bool _isPaused = false;
    [SerializeField] private BoolVariable _hasSeenTutorial;

    public bool IsPaused { get => _isPaused; set => _isPaused = value; }
    public BoolVariable HasSeenTutorial { get => _hasSeenTutorial; }
    public bool IsGameOver { get; set; }

    protected override void OnAwake()
    {
        SetCursor(true, CursorLockMode.Confined);
        SetGameUnPaused();

        #if UNITY_WEBPLAYER
            DestroyImmediate(UIManager.Instance.View_titleScreen.QuitButton);
        #endif
    }

    protected override void OnStart()
    {
        base.OnStart();
        UIManager.Instance.OnSwitchViewSynchronous(UIManager.Instance.View_titleScreen);
    }

    protected override void Init()
    {
        base.Init();
        ResetGame();
    }

    public void ResetGame()
    {
        IsGameOver = false;
        EnemyManager enemyManager = EnemyManager.Instance;
        //enemyManager.ResetManager();
    }

    public void SetGamePaused()
    {
        IsPaused = true;
    }

    public void SetGameUnPaused()
    {
        IsPaused = false;
    }

    public void SetCursor(bool visible, CursorLockMode lockMode)
    {
        Cursor.visible = visible;
        Cursor.lockState = lockMode;
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                 Application.Quit();
        #endif
    }
}
