using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerUpdater : MonoBehaviour
{
    private void FixedUpdate()
    {
        UIManager _uiManager = UIManager.Instance;
        _uiManager.View_highscore.SetTimerDirty();
        _uiManager.View_Infos.CurrentTimer.SetTitle(HighScoreManager.Instance.CurrentTimer);
    }
}
