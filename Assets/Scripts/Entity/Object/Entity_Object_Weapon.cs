using UnityEngine;

public class Entity_Object_Weapon : Entity_Object_Pickable
{
    public float WeaponRange;
    public int WeaponDamage;
    [SerializeField] private Animator animator;
    [SerializeField] protected Transform m_attackFrom; // TODO: May need refactoring into ranged weapon only after implementation of melee OnAttack()
    private Vector3 m_enemyDirectionAIm;
    public string weaponName;

    [Header("Sounds")]
    [SerializeField] protected AudioController_Multiple m_audioController;

    public Animator Animator { get => animator; set => animator = value; }

    protected override void Init()
    {
        base.Init();
        if (GetComponent<Animator>())
        {
            animator = GetComponent<Animator>();
            animator.enabled = false;
        }
		
	   if(!WeaponManager.Instance.weaponList.Contains(this))
        {
            WeaponManager.Instance.weaponList.Add(this);
        }

        if (m_attackFrom == null)
		{
            m_attackFrom = transform;
		}
    }

    public override void OnAttack(Transform _direction= null)
    {
        if (animator)
        {
            animator.Play("Attack");
        }
        Vector3 _tempForward = Vector3.zero;
        int layer = 0;
        if (GetComponentInParent<Entity_Player>())
        {
            _tempForward = m_attackFrom.forward;
            layer = 1 << 6;
        }
        else if(GetComponentInParent<Entity_Enemy>())
        {
            _tempForward = m_enemyDirectionAIm;
            layer = 1 << 10;
        }

        print(_tempForward);

        m_audioController.PlayOneShot();

        Debug.DrawLine(transform.position, transform.position + _tempForward * WeaponRange, Color.red);

        if (Physics.Raycast(transform.position, _tempForward, out RaycastHit hit, WeaponRange, layer))
        {
            Entity_Living livingEntity = hit.transform.gameObject.GetComponent<Entity_Living>();
            Entity_Living _castToParent = hit.transform.gameObject.GetComponentInParent<Entity_Living>();

            if (livingEntity)
            {
                livingEntity.OnHit(WeaponDamage);
            }
            else if (_castToParent )
            {
                _castToParent.OnHit(WeaponDamage);
            }
        }
    }
    public  void SetEnemyForward(Vector3 _direction, Entity_Enemy _entity = null)
    {
        if (_entity)
        {
            m_enemyDirectionAIm = _direction;
        }
    }

    public override void OnPickup()
    {
        base.OnPickup();

        if (Entity_Player.Instance.Weapon)
        {
            Entity_Player.Instance.Weapon.animator.enabled = false;
            Entity_Player.Instance.Weapon.OnDrop();
        }

        Entity_Player.Instance.Weapon = this;
        this.animator.enabled = true; // prevents weapons from runing away in fear of being forced to kill civilians again
        PickupHelper(Entity_Player.Instance.SocketWeapon);
    }
}
