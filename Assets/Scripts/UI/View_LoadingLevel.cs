using System;
using UnityEngine.InputSystem;

public class View_LoadingLevel : View_Base
{
    public override void OnShow(Action callback = null)
    {
        Entity_Player.Instance.Input.enabled = false;
        base.OnShow(callback);
    }
}
