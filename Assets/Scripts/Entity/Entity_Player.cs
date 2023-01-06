using UnityEngine;
using UnityEngine.InputSystem;

public class Entity_Player : Entity_Living
{
    public static Entity_Player Instance { get; set; }

    [Header("Input")]
    [SerializeField] private PlayerAction m_action;

    public PlayerActionsContainer DesiredActions;

    [SerializeField] private Transform socketThrowable;
    [SerializeField] private Transform socketWeapon;

    public float WeaponRange;
    //public bool inADS = false;

    [SerializeField] private PlayerInput m_input;
    private bool isCrouch;
    [SerializeField] private float moveSpeed;
    private float crouchSpeed = 5f;
    private float uncrouchSpeed = 10.0f;

    private float jumpHeight;
    private float uncrouchJumpHeight = 7f;
    private float crouchJumpHeight = 6f;
    public bool canCrouch = true;

    private CharacterController controller;
    public Vector3 gravityVelo;
    [SerializeField] private float gravity;
    public readonly float MaxGravity = 25f;
    private Vector3 finalVelo;
    private Vector3 moveVelo;
    private Vector3 jumpVelo;

    public bool canUseGravity = true;

    public MasterOfState<Entity_Player> MasterOfState { get; private set; }
    public StateContainer_Player StateContainer { get; private set; }
    public PlayerAction Action { get => m_action; set => m_action = value; }
    public Entity_Object_Pickable LastSeenPickable { get; set; }
    public Camera Camera { get; private set;  }
    public Transform SocketThrowable { get => socketThrowable; }
    public Entity_Object_Throwable Throwable { get; set; }
    public Transform SocketWeapon { get => socketWeapon; }
    public Entity_Object_Weapon Weapon { get; set; }
    public bool IsCrouch { get => isCrouch; set => isCrouch = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float JumpHeight { get => jumpHeight; set => jumpHeight = value; }
    public CharacterController Controller { get => controller; set => controller = value; }
    public float Gravity { get => gravity; set => gravity = value; }
    public Vector3 MoveVelo { get => moveVelo; set => moveVelo = value; }
    public Vector3 JumpVelo { get => jumpVelo; set => jumpVelo = value; }
    public Vector3 FinalVelo { get => finalVelo; set => finalVelo = value; }
    public float UncrouchJumpHeight { get => uncrouchJumpHeight; set => uncrouchJumpHeight = value; }
    public float CrouchJumpHeight { get => crouchJumpHeight; set => crouchJumpHeight = value; }
    public float CrouchSpeed { get => crouchSpeed; set => crouchSpeed = value; }
    public float UncrouchSpeed { get => uncrouchSpeed; set => uncrouchSpeed = value; }
    public PlayerInput Input { get => m_input; }
    public InputReceiver InputReceiver { get; private set; }


    //public Vector3 GravityVelo { get => gravityVelo; set => gravityVelo = value; }

    protected override void OnAwake()
    {
        base.OnAwake();
        if (Instance == null)
        {
            Instance = this;
        }
    }
    protected override void Init()
    {
        canUseGravity = false;
        base.Init();
        Camera = GetComponentInChildren<Camera>();

        StateContainer = new StateContainer_Player(this);
        MasterOfState = new MasterOfState<Entity_Player>(StateContainer.IddleState);
        jumpHeight = UncrouchJumpHeight;
        moveSpeed = UncrouchSpeed;

        DesiredActions = new PlayerActionsContainer();
        controller = GetComponent<CharacterController>();
        InputReceiver = GetComponent<InputReceiver>();
    }

    public bool isHit = false;
    protected override void OnUpdate() 
    {
        base.OnUpdate();

        if (canUseGravity)
        {
            AddGravity();
        }
        
        // TODO: move to fixedupdate?
        MasterOfState.OnUpdate();
        DesiredActions.OnUpdateActions();

        DoFinalMove();
        HandleGrounded();

        if (isHit)
        {
            isHit = false;
            OnHit(1);
        }
    }

    public override void OnHit(int damage)
    {
        base.OnHit(damage);
        if (damage > 0 && !IsDead)
        {
            if (UIManager.Instance.View_Infos.gameObject.activeSelf)
            {
                UIManager.Instance.View_Infos.DefaultSelectedElement.SetTitle(m_currentHP.Value.ToString("D3"));
            }
            
            UIElement_Base UIElement = UIManager.Instance.View_Infos.DefaultSelectedElement;
            if (!UIElement.gameObject.activeSelf) { return; }
            (UIElement as UIElement_Counter).OnWiggleNegative();
        }
    }

    public override void OnDeath()
    {
        base.OnDeath();
        // DO DEATH LOGIC HERE
        //UIManager.Instance.View_Infos.OnHide();

        (UIManager.Instance.View_Infos.DefaultSelectedElement as UIElement_Counter).OnWiggleNegative(true, () =>
        {
            UIManager.Instance.View_Infos.OnHide();
        });
    }

    private void AddGravity()
    {
        gravityVelo.y -= Time.deltaTime * gravity;
    }

    private void HandleGrounded()
    {
        if(controller.isGrounded)
        {
            gravityVelo.y = 0;
            jumpVelo.y = 0;
        }
    }

    public bool IsGrounded()
    {
        RaycastHit hit;
        bool isGrounded = Physics.SphereCast(this.transform.position + Vector3.up * 0.35f, 0.5f, Vector3.down, out hit, 0.1f);
        return (isGrounded || controller.isGrounded);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.35f, 0.5f);
    }

    private void DoFinalMove()
    {
        if(controller.enabled)
        {
            controller.Move(GetFinalVelo() * Time.deltaTime);
            finalVelo = Vector3.zero;
        }
    }

    private Vector3 GetFinalVelo()
    {
        finalVelo = moveVelo + gravityVelo + jumpVelo;
        return FinalVelo;
    }

    /// <summary>
    /// Drop current weapon and throwable
    /// </summary>
    public void DropSockets()
    {
        if (Throwable)
        {
            Throwable.OnDrop();
        }
        if (Weapon)
        {
            Weapon.OnDrop();
        }
    }

    public void OnSetCrouch(bool isCrouched)
    {
        // isCrouch = current crouched state
        if (isCrouched && !isCrouch)
        {
            Animator.Play("Crouch");
            isCrouch = true;
            moveSpeed = crouchSpeed;
            jumpHeight = crouchJumpHeight;
        }
        else if (!isCrouched && isCrouch)
        {
            Animator.Play("UnCrouch");
            isCrouch = false;
            moveSpeed = uncrouchSpeed;
            jumpHeight = uncrouchJumpHeight;
        }
    }

    /// <summary>
    /// Reset every aspects of the player <br/>
    /// transform, camera, health and healthbar, iddle state
    /// </summary>
    public void ResetPlayer()
    {
        moveVelo = Vector3.zero;
        jumpVelo = Vector3.zero;
        finalVelo = Vector3.zero;
        gravity = 0;
        Input.enabled = false;
        transform.position = new Vector3(0f, -0.5f, 0f);
        InputReceiver.Instance.CamRXYReset();

        FullHeal();
        UIManager.Instance.View_Infos.DefaultSelectedElement.SetTitle(m_currentHP.Value.ToString("D3"));

        MasterOfState.OnTransitionState(StateContainer.IddleState);
    }
}
