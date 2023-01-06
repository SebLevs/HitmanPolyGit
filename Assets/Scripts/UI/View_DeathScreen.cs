using System;
using System.Collections;
using UnityEngine;

public class View_DeathScreen : View_Base
{
    protected override IEnumerator OnShowIE(Action callback = null)
    {
        float timer = 0.0f;
        while (timer < m_fadeInOutTime || m_canvasGroup.alpha != 1) // TODO: Set as  a method that takes an action  (see TODO bellow) (overridable in children if there<s a desire for it)
        {
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
            m_canvasGroup.alpha = timer / m_fadeInOutTime; // TODO: Set as an action for this specific implementation
        }

        m_canvasGroup.interactable = true;
        m_canvasGroup.alpha = 1;

        SceneLoadManager.Instance.UnloadCurrentSceneAsync();
        
        yield return new WaitForSeconds(1.5f);

        if (callback != null)
        {
            callback.Invoke();
        }

        // If no selected element, select default element (if any)
        SetAndSelectLastElement();

        UIManager.Instance.OnSwitchViewSequential(UIManager.Instance.View_MainMenu);
    }
}
