using System;
using System.Collections;
using UnityEngine;

public class UIElement_QuestCompletion : UIElement_QuestBase
{
    private const float _timerTransitionWait = 1.5f;
    public override IEnumerator PopoutQuestIE(float _popoutTime = 0.75F, Action callback = null)
    {
        yield return new WaitForSeconds(_timerTransitionWait);
        yield return base.PopoutQuestIE(_popoutTime, callback);
    }
}
