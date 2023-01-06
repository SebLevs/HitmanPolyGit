using System;
using System.Collections;
using UnityEngine;

public class View_Target : View_Base
{
    [Header("Target visuals")]
    [SerializeField] private GameObject visuals;

    [Header("Additional views")]
    [SerializeField] private View_Base _viewInputs;
    [SerializeField] private View_Base _viewQuest;

    public MeshFilter m_meshFilter { get; private set; }
    public MeshRenderer m_meshRenderer { get; private set; }

    protected override void OnAwake()
    {
        base.OnAwake();
        SetRefs();

    }
    protected override void Init()
    {
        base.Init();
        SetRefs();
    }

    private void SetRefs()
    {
        if (m_meshFilter == null || m_meshRenderer == null)
        {
            m_meshFilter = visuals.GetComponentInChildren<MeshFilter>();
            m_meshRenderer = visuals.GetComponentInChildren<MeshRenderer>();
        }
    }

    protected override IEnumerator OnShowIE(Action callback = null)
    {
        _viewInputs.OnShow();
        _viewQuest.OnShow();

        float timer = 0.0f;

        Entity_Player player = Entity_Player.Instance;

        player.SocketWeapon.gameObject.SetActive(false); // TODO: Can be placed inside of while to pop up with time
        player.SocketThrowable.gameObject.SetActive(false); // TODO: Can be placed inside of while to pop up with time
        while (timer < m_fadeInOutTime)
        {
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
            // Move in view over time here or some other crazy cool behaviour
            //m_canvasGroup.alpha = timer / m_fadeInOutTime; 
        }

        if (callback != null)
        {
            callback.Invoke();
        }

        UIManager.Instance.CurrentView = this;
    }

    protected override IEnumerator OnHideIE(Action callback = null)
    {
        Entity_Player.Instance.DesiredActions.PurgeAllAction();
        _viewInputs.OnHide();
        _viewQuest.OnHide();
        if (UIManager.Instance.View_tutorial.gameObject.activeSelf)
        {
            UIManager.Instance.View_tutorial.OnHide();
        }

        float timer = m_fadeInOutTime;

        Entity_Player player = Entity_Player.Instance;

        while (timer > 0)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            timer -= Time.deltaTime;
            // Move out of view over time here or some other lit behaviour, or whatever the kids say nowadays
            //m_canvasGroup.alpha = timer / m_fadeInOutTime;
        }
        player.SocketWeapon.gameObject.SetActive(true); // TODO: Can be placed inside of while to pop up with time
        player.SocketThrowable.gameObject.SetActive(true); // TODO: Can be placed inside of while to pop up with time

        if (callback != null)
        {
            callback.Invoke();
        }

        gameObject.SetActive(false); // To prevent alpha shenanigans
    }

    /// <summary>
    /// Set references to mesh
    /// </summary>
    public void SetTargetMeshOnUI()
    {
        Init();

        SkinnedMeshRenderer _targetSkinnedMeshRenderer = EnemyManager.Instance.TargetToKill.GetComponentInChildren<SkinnedMeshRenderer>();
        m_meshFilter.mesh = _targetSkinnedMeshRenderer.sharedMesh;
        m_meshRenderer.materials[0] = _targetSkinnedMeshRenderer.materials[0];
    }
}
