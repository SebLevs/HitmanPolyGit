using UnityEngine;
using System;

public class View_TitleScreen : View_Base
{
    [SerializeField] private GameObject m_quitButton;

    public GameObject QuitButton { get => m_quitButton; }

    public override void OnShow(Action callback = null)
    {
        Entity_Player _player = Entity_Player.Instance;
        UIManager _uiManager = UIManager.Instance;

        _player.Gravity = 0;
        _player.Input.enabled = false;
        base.OnShow(() => 
        {
            if (callback != null) { callback.Invoke(); }
            GameManager.Instance.SetCursor(true, CursorLockMode.Confined);
        });
    }
}
