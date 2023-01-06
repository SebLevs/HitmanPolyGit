using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Weapon_Ranged : Entity_Object_Weapon
{
    [Header("Ranged Weapon Class")]
    [SerializeField] private int m_ammoCount = 1;
    [SerializeField] private int m_maxAmmo;
    [SerializeField] private GameObject reloadEntity;
    public bool IsEmpty => m_ammoCount <= 0;

    [SerializeField] protected AudioElement_Single m_emptyChamberSound;
    [SerializeField] private View_Counter m_viewAmmoCount;

    [SerializeField] private ParticleSystem muzzleFlash;

    // TODO: Use attackFrom only in this.class, implement melee logic and replace ranged logic of main class OnAttack() here
    //[SerializeField] private Transform m_attackFrom;

    protected override void Init()
    {
        base.Init();
        m_viewAmmoCount.CounterElement.SetTitle(m_ammoCount.ToString("D3"));
    }

    public override void OnAttack(Transform _direction = null)
    {
        // Empty clip sound
        if (IsEmpty)
        {
            m_audioController.PlayOneShot(m_emptyChamberSound.GetClip());
            return;
        }

        base.OnAttack(_direction);
        muzzleFlash.Play();
        //if(InputReceiver.Instance.isADS)
        //{
        //    Entity_Player.Instance.gameObject.GetComponentInChildren<Camera>().GetComponent<Animator>().Play("CameraShakeADS");
        //}
        //else
        //{
        //    Entity_Player.Instance.gameObject.GetComponentInChildren<Camera>().GetComponent<Animator>().Play("CameraShake");
        //}
        Entity_Player.Instance.gameObject.GetComponentInChildren<Camera>().GetComponent<Animator>().Play("CameraShake");
        m_ammoCount--;

        // Always set visuals in case player pickup enemy weapon
        if (!IsEmpty)
        {
            m_viewAmmoCount.CounterElement.SetTitle(m_ammoCount.ToString("D3"));
        }
    }

    public void Reload()
    {
        var newpos = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);
        reloadEntity.transform.position = newpos;
        reloadEntity.SetActive(true);
        reloadEntity.transform.parent = null;
        m_audioController.PlayOneShot(m_emptyChamberSound.GetClip());
        if (IsEmpty)
        {
            m_ammoCount = m_maxAmmo;
        }
    }

    public override void OnPickup()
    {
        base.OnPickup();
        m_viewAmmoCount.OnShow();
    }

    public override void OnDrop()
    {
        base.OnDrop();
        m_viewAmmoCount.OnHide();
    }

    public bool AE_OnBecomeThrowable(float distractRadius = 10f) // Animation event
    {
        if (IsEmpty)
        {
            m_viewAmmoCount.CounterElement.OnWiggleNegative(true);
            m_viewAmmoCount.OnHide();
            gameObject.AddComponent<Entity_Object_Throwable>();
            Entity_Object_Throwable m_throwable = GetComponent<Entity_Object_Throwable>();
            m_throwable.DistractRadiusSet = distractRadius;

            if (Entity_Player.Instance.Weapon == this)
            {
                if (Entity_Player.Instance.Throwable == null)
                {
                    m_throwable.OnPickup();
                }
                else
                {
                    OnDrop();
                }
            }
            else
            {
                OnDrop();
            }
            Destroy(Animator);
            Destroy(this);
            return true;
        }
        return false;
    }
}
