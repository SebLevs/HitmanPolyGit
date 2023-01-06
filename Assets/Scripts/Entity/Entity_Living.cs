using UnityEngine;

public abstract class Entity_Living : Entity_Collider
{
    [Header("Health")]
    [SerializeField] protected IntReference m_maxHP;
    [SerializeField] protected IntReference m_currentHP;

    [Header("Audio")]
    [SerializeField] protected AudioController_Multiple m_audioControllerHit;

    //public AudioSource AudioSource { get; protected set; }

    public Animator Animator { get; protected set; }

    public bool IsDead => m_currentHP.Value <= 0;

    public IntReference CurrentHP { get => m_currentHP; }

    protected override void OnAwake()
    {
        base.OnAwake();
    }

    protected override void OnStart()
    {
    }

    protected override void Init()
    {
        base.Init();
        FullHeal();
        Animator = GetComponent<Animator>();
    }

    public virtual void OnHit(int damage)
    {
        if (!IsDead)
        {
            m_currentHP.Value -= damage;
            m_audioControllerHit.PlayOneShot();

            if (m_currentHP.ClampMin(0))
            {
                OnDeath();
            }
        }
    }

    public virtual void OnInstantDeath()
    {
        //AudioSource.PlayOneShot(AudioSource.clip);
        m_currentHP.Value -= m_currentHP.Value;
        m_currentHP.ClampMin(0);
        OnDeath();
    }

    protected void OnHeal(int value)
    {
        m_currentHP.ClampMax(value);
    }

    public virtual void OnDeath()
    {
        // TODO: Create logic on death of living entity
    }

    public void SetCurrentHP(int value)
    {
        m_currentHP.Value = value;
        m_currentHP.ClampValue(0, m_maxHP.Value);
    }

    public void SetMaxHP(int value)
    {
        m_maxHP.Value = value;
        m_maxHP.ClampMin(0);
    }

    public void FullHeal()
    {
        m_currentHP.Value = m_maxHP.Value;
    }
}
