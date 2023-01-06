using UnityEngine;
using System;

public class EnemyCueHandler : MonoBehaviour
{
    [SerializeField] private View_EnemyCueState m_currentStateView;

    [SerializeField] private View_EnemyCueSightedBar m_view_sightedBar;

    [SerializeField] private View_EnemyCueState m_view_curious;
    [SerializeField] private View_EnemyCueState m_view_combat;

    [SerializeField] private View_FillingBar m_view_hp;

    public View_EnemyCueSightedBar View_sightedBar { get => m_view_sightedBar; }

    public View_EnemyCueState View_curious { get => m_view_curious; }
    public View_EnemyCueState View_combat { get => m_view_combat; }

    public View_FillingBar View_HP { get => m_view_hp; }

    /// <summary>
    /// Squential switch view: <br/>
    /// OnHide THEN OnShow
    /// <br/><br/>
    /// OnHide() current view<br/>
    /// OnShow() newly selected view
    /// </summary>
    public void OnSwitchViewSequential(View_EnemyCueState _newView, Action hideCallback = null, Action showCallback = null)
    {
        // Hide currently selected view
        if (m_currentStateView && m_currentStateView != _newView && m_currentStateView.gameObject.activeSelf)
        {
            m_currentStateView.StopAllCoroutines();
            m_currentStateView.OnHide(() =>
            {
                if (hideCallback != null) { hideCallback.Invoke(); }
                // m_currentStateView is set at end of onShow
                m_currentStateView = _newView;
                _newView.OnShow(showCallback);
            });
        }
        else
        {
            m_currentStateView = _newView;
            _newView.OnShow(showCallback);
        }
    }

    public void OnHideCurrentStateView(Action hideCallback = null)
    {
        if (m_currentStateView && m_currentStateView.gameObject.activeSelf)
        {
            m_currentStateView.StopAllCoroutines();
            m_currentStateView.OnHide();
        }
    }

    public void OnHideAll()
    {
        if (m_view_sightedBar.gameObject.activeSelf)
        {
            m_view_sightedBar.OnHide();
        }

        if (m_view_curious.gameObject.activeSelf)
        {
            m_view_curious.OnHide();
        }

        if (m_view_sightedBar.gameObject.activeSelf)
        {
            m_view_sightedBar.OnHide();
        }

        if (m_view_hp.gameObject.activeSelf)
        {
            m_view_hp.OnHide();
        }
    }
}
