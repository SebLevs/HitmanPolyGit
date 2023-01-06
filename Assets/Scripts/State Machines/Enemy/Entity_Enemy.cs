using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;

public class Entity_Enemy : Entity_Living //not abstract
{
    //TO DO : put RB on head kinematic when ragdoll is activate
    //need to set patrol back to is first node if doesnt see enemy
    [Header("Enemy Type")]
    public EnemyType Type;
    [SerializeField] private float RunSpeed;
    [Header("Player detection field")]
    [SerializeField] protected float FieldOfView;
    [SerializeField] protected float DetectionRadius;
    [SerializeField] protected float TimeBeforeLoosePLayerFocus;
                     private float m_timeWatchLoosePLayer;
                     private Vector3 PlayerLastSeenPos;
                    private int m_rayHitChance;
    [Header("Chassing Parameter field")]
    [SerializeField] private float AttackRange;
    [SerializeField] private float AttackDamage;
    [SerializeField] private float EnemyRage;
    [SerializeField][Range(0.5f,2.5f)] private float RageMultiplier;
    [SerializeField] private float MaximunRage;
    [SerializeField][Range(0.1f,3f)] private float RagePerRayHit;
    [SerializeField] private float WeaponFireRate;
                     private float m_TimeWatchFireRate;
                     [SerializeField]private bool m_isPLayerDetect;
    [Header("Dead Target Parameter ")]
    [SerializeField] private float RangeToSeeDeadTarget;
    [SerializeField] private float LookAtDeadTargetDuration;
                     private float m_TimeWatchDeadTarget;
                     private bool SeeDeadTarget;
                     private Vector3 m_deadTargetPos;
    [Header("Socket Transform parameter")]
    public Transform Root;
    public Transform WeaponSocket;
    public GameObject FlashLight;
    public ParticleSystem HitParticule;
    private bool isHit;

    //idle timer
    private float m_idleTime;
    private float m_idlestopwatch;
    //distact value

    private bool IsDistract;
    //private bool IsPLayerDetect;
    //component
    private NavMeshAgent m_agent;
    //player value
    public Entity_Player PlayerRef { get; private set; }
    private RaycastHit[] m_playerHitspoints;

    //path
    public ListOfEnemyPath m_listOfPath;
    [SerializeField] private EnemyCueHandler m_enemyCueHandler;
    public Vector3 m_nextPath { get; private set; }

    //manager
    protected EnemyManager m_enemyManager {  get; private set; }
    //TYPE

    //STATEMACHINE + STATECONTAINER
    public MasterOfState<Entity_Enemy> Statemachine { get; protected set; }
    public Entity_Enemy_StateContainer StateContainer { get; private set; }
    public EnemyCueHandler EnemyCueHandler { get => m_enemyCueHandler;}
    private EnemyStateType m_currentState;

    const float m_hpPopupDistance = 25f;
    private bool isCloseToPlayer = false;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_agent = GetComponent<NavMeshAgent>(); 
        m_listOfPath = GetComponent<ListOfEnemyPath>();
    }
    protected override void Init()
    {
        base.Init();
        IsDistract = false;
        m_playerHitspoints = new RaycastHit[5];
        m_rayHitChance = 0;
        isHit = false;
        if (!m_enemyManager)
        {
            m_enemyManager = EnemyManager.Instance;
            m_enemyManager.Subscribe(this);
        }
        if (!PlayerRef) 
        {
            PlayerRef = Entity_Player.Instance;
        }
        StateContainer = new Entity_Enemy_StateContainer(this);
        Statemachine = new MasterOfState<Entity_Enemy>(StateContainer.State_Idle);
        EnableRagdoll(false);

        m_enemyCueHandler.View_HP.DefaultSelectedElement.SetTitle(m_currentHP.Value.ToString("D3"));

        float _currentHP = m_currentHP.Value;
        float _maxHP = m_maxHP.Value;
        float _normalised = _currentHP / _maxHP;
        m_enemyCueHandler.View_HP.FillingBarElement.SetFilling(_normalised);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        Statemachine.OnUpdate();

        HandleHpBarUponDistance();
    }

    // TODO: Delete whole behaviour?
    // Hp dissapear when player is fleeing which makes it difficult to see what enemy was already engaged in battle
    // 
    private void HandleHpBarUponDistance() 
    {
        if (IsDead) { return; }
        float _distance = Vector3.Distance(transform.position, Entity_Player.Instance.transform.position);

        if (Statemachine.CurrentState == StateContainer.State_Attack || Statemachine.CurrentState == StateContainer.State_Chasse)
        {
            if (!isCloseToPlayer && _distance <= m_hpPopupDistance)
            {
                isCloseToPlayer = true;
                EnemyCueHandler.View_HP.StopAllCoroutines();
                EnemyCueHandler.View_HP.OnShow(() => { });
            }
        }
        else
        {
            if (isCloseToPlayer && _distance > m_hpPopupDistance)
            {
                isCloseToPlayer = false;
                EnemyCueHandler.View_HP.StopAllCoroutines();
                EnemyCueHandler.View_HP.OnHide();
            }
        }
    }

    private void SetAnimationState(EnemyStateType _currentState = 0)
    {
        if(m_currentState != _currentState)
        {
            m_currentState = _currentState;
            Animator.SetInteger("currentState", (int)_currentState);
        }
    }

    #region Set Path to the Agent
    public void MoveAgent(Vector3 _nextPath)
    {
        if (m_agent.isStopped)
        {
            m_agent.isStopped = false;
        }
        m_agent.destination = _nextPath;
    }

    public void SetAgentSpeed(float _speed = 4f) // 4f is walk speed;
    {
        if(m_agent.speed != _speed)
        {
            m_agent.speed = _speed;

        }
    }
    public void StopAgent()
    {
        if (!m_agent.isStopped)
        {
            m_agent.isStopped = true;
            //MoveAgent(transform.position);
        }
    }
    public void UnStopAgent()
    {
        m_agent.isStopped = false;
    }
    public void SetNewPatrol()
    {
        //if (IsPathFinish() || m_nextPath == Vector3.zero)//maybe to be removed
        //{
            m_nextPath = m_listOfPath.GetNextWayPoint();
        //}
    }
    public void GetGuardPoint()
    {
        m_nextPath = m_listOfPath.SetGuardPosition();
    }

    public bool IsPathFinish()
    {
        return Vector3.Distance(transform.position, m_agent.destination) < 2f;
    }

    protected bool IsValidPath(Vector3 path)
    {
        NavMeshPath navPath = new NavMeshPath();
        m_agent.CalculatePath(path, navPath);
        if (navPath.status == NavMeshPathStatus.PathInvalid)
            return false;

        return true;
    }
   
    #endregion

    #region ACTIONS USE IN MY STATES
   
    public void OnIdleEnter()
    {
        RandomIdleWaitTime();
        SetAnimationState(EnemyStateType.IDLE);
        StopAgent();
    }
    public void OnIdleUpdate()
    {
        IncommingHit();
        if (IsDead) // can add type for civilan not die ?
        {
            Statemachine.OnTransitionState(StateContainer.State_Dead);
        }
        if(Type != EnemyType.GUARD)
        {
            if (IsIdleTimeDone())
            {
                Statemachine.OnTransitionState(StateContainer.State_Patrol);
            }
        }
        if (Type != EnemyType.TARGET && InvestigateTargetState())
        {
            Statemachine.OnTransitionState(StateContainer.State_DeadTarget);
        }

        if (Type != EnemyType.CIVILIAN && CanSeePlayer())
        {
            Statemachine.OnTransitionState(StateContainer.State_PlayerFound);
        }
        else if (IsDistract) 
        {
            Statemachine.OnTransitionState(StateContainer.State_Distract);
        }
    }


    public void OnPatrolEnter()
    {
        SetAgentSpeed();
        SetAnimationState(EnemyStateType.WALK);
        if(Type == EnemyType.GUARD)
        {
            GetGuardPoint();
        }
        else
        {
            SetNewPatrol();
        }
        MoveAgent(m_nextPath);
    } 
    public void OnPatrolUpdate()
    {
        IncommingHit();
        if (IsDead) 
        {
            Statemachine.OnTransitionState(StateContainer.State_Dead);
        }
        else if (Type != EnemyType.CIVILIAN && CanSeePlayer())
        {
            Statemachine.OnTransitionState(StateContainer.State_PlayerFound);
        }
        else if (IsPathFinish())
        {
            Statemachine.OnTransitionState(StateContainer.State_Idle);
        }
        else if (IsDistract)
        {
            Statemachine.OnTransitionState(StateContainer.State_Distract);
        }
        else if (Type != EnemyType.TARGET   && InvestigateTargetState())
        {
            Statemachine.OnTransitionState(StateContainer.State_DeadTarget);
        }
    }
    public void OnDistractEnter()
    {
        SetAgentSpeed();
        SetAnimationState(EnemyStateType.WALK);
        MoveAgent(m_nextPath);
        IsDistract = false;
    }
    public void OnDistractUpdate()
    {
        IncommingHit();
        if (IsDead)
        {
            Statemachine.OnTransitionState(StateContainer.State_Dead);
        }
        else if (Type != EnemyType.CIVILIAN && CanSeePlayer())
        {
            Statemachine.OnTransitionState(StateContainer.State_PlayerFound);
        }
        else if (IsDistract && Type != EnemyType.CIVILIAN) // to beverify
        {
                if (m_agent.isStopped)
                {
                    UnStopAgent();
                }
                MoveAgent(m_nextPath);
                IsDistract = false;
                //SetAnimationState(EnemyStateType.WALK);
        }
        else if (IsPathFinish() && Type != EnemyType.CIVILIAN) //need to add _currentHP wait time to play look around anim
        {
            OnLookUpdate(EnemyStateType.DISTRACTLOOK, "DistractLook");
        }
        else if (Type != EnemyType.TARGET && InvestigateTargetState())
        {
            Statemachine.OnTransitionState(StateContainer.State_DeadTarget);
        }
    }

    public void OnInvestigateEnter()
    {
        SetAgentSpeed();
        SetAnimationState(EnemyStateType.WALK);
        MoveAgent(PlayerLastSeenPos);
    } 
    public void OnInvestigateUpdate()
    {
        IncommingHit();
        if (IsDead)
        {
            Statemachine.OnTransitionState(StateContainer.State_Dead);
        }
        if (Type != EnemyType.TARGET && InvestigateTargetState())
        {
            Statemachine.OnTransitionState(StateContainer.State_DeadTarget);
        }
        else if (Type != EnemyType.CIVILIAN && CanSeePlayer())
        {
            Statemachine.OnTransitionState(StateContainer.State_PlayerFound);
        }
        else if (IsPathFinish() && Type != EnemyType.CIVILIAN) // need to add _currentHP wait time for play look around anim
        {
            OnLookUpdate(EnemyStateType.INVESTIGATELOOK, "InvestigateLook");
        }
        else if (IsDistract)
        {
            Statemachine.OnTransitionState(StateContainer.State_Distract);
        }
    }
    public void OnPlayerFoundStart()
    {
        StopAgent();
        SetAnimationState(EnemyStateType.PLAYERFOUND);
    }
    public void OnPlayerFoundUpdate()
    {
        LookAtTarget(PlayerRef.transform.position);
        IncommingHit();
        if (CanSeePlayer())
        {
            IncreaseRage();

            if (CanChasse())
            {
                EnemyCueHandler.View_sightedBar.OnHide();
                Statemachine.OnTransitionState(StateContainer.State_Chasse);
            }
        }
        if (IsDead)
        {
            Statemachine.OnTransitionState(StateContainer.State_Dead);
        }

        else if (!CanSeePlayer()&& CanInvestigate())
        {
            Statemachine.OnTransitionState(StateContainer.State_Investigate);
        }
       else if(!CanSeePlayer() && Type != EnemyType.GUARD)
       {
            Statemachine.OnTransitionState(StateContainer.State_Patrol);
       }
        //else if(!CanSeePlayer() && Type == EnemyType.GUARD)
        //{
        //    Statemachine.OnTransitionState(StateContainer.State_Idle);
        //}
        else if (Type != EnemyType.TARGET && InvestigateTargetState())
        {
            Statemachine.OnTransitionState(StateContainer.State_DeadTarget);
        }
    }
    public void OnChasseEnter()
    {
        SetAgentSpeed(RunSpeed); 
        SetAnimationState(EnemyStateType.CHASSE);
        m_enemyManager.SetAlert(); // TODO: Find better place that doesn't get called every N time when player flees
    }
    public void OnChasseUpdate()
    {
        MoveAgent(PlayerRef.transform.position);
        LookAtTarget(PlayerRef.transform.position);
        //CanSeePlayer(); //needed?
        if (IsDead)
        {
           Statemachine.OnTransitionState(StateContainer.State_Dead);
        }
        if (InAttackRange() && CanSeePlayer())
        {
            Statemachine.OnTransitionState(StateContainer.State_Attack);
        }
        else if (!CanSeePlayer() && CanInvestigate())
        {
            Statemachine.OnTransitionState(StateContainer.State_Investigate);
        }
        else if (Type != EnemyType.TARGET && InvestigateTargetState())
        {
            Statemachine.OnTransitionState(StateContainer.State_DeadTarget);
        }
    }

    public void OnShotEnter()
    {
        SetAnimationState(EnemyStateType.ATTACK);
        //Attack();
        //IsWeaponEmpty();
        StopAgent();
    }
    /// <summary>
    /// player is not found and enemy got hit
    /// </summary>
    public void IncommingHit()
    {
        if (isHit && Type != EnemyType.CIVILIAN)
        {
            if (InAttackRange())
            {
                isHit = false;
                Statemachine.OnTransitionState(StateContainer.State_Attack);
                
            }
            else
            {
                isHit = false;
                Statemachine.OnTransitionState(StateContainer.State_Chasse);
            }
            
        }
    }
    public void OnShotUpdate()
    {
        LookAtTarget(PlayerRef.transform.position);
        if (IsDead)
        {
            Statemachine.OnTransitionState(StateContainer.State_Dead);
        }
        if (Type != EnemyType.TARGET && InvestigateTargetState() && !m_enemyManager.IsAlertActivated)
        {
            Statemachine.OnTransitionState(StateContainer.State_DeadTarget);
        }
        if (InAttackRange() && CanShoot())
        {
            Attack();
        }
        else if (!InAttackRange())
        {
            Statemachine.OnTransitionState(StateContainer.State_Chasse);
        }
        else if(!CanSeePlayer() && CanInvestigate())
        {
            Statemachine.OnTransitionState(StateContainer.State_Investigate);
        }
       
    }

    public void Attack()
    {
        LookAtTarget(PlayerRef.transform.position);
        var _weapon = WeaponSocket.GetComponentInChildren<Entity_Weapon_Ranged>();
        if (_weapon)
        {
            if (_weapon.IsEmpty)
            {
                _weapon.Reload();
            }
            else
            {
                Vector3 _targetDirection = (PlayerRef.transform.position - this.transform.position).normalized; //maybe add offset
                _targetDirection.y -= 0.025f;
                _weapon.SetEnemyForward(_targetDirection, this);
                _weapon.OnAttack(); //animator enabled need to be change enemy cant use it otherwise
                print("shoot");//slow down firereate too quick
                m_TimeWatchFireRate = 0.00f;
                StopAgent();
            }

        }
    }

    public void OnSeeDeadTargetEnter()
    {
         SetAnimationState(EnemyStateType.WALK);

    }
    public void OnSeeDeadTargetUpdate()
    {
        LookAtTarget(m_deadTargetPos);
        MoveAgent(m_deadTargetPos);
        if (IsDead)
        {
            Statemachine.OnTransitionState(StateContainer.State_Dead);
        }
        if (IsPathFinish())
        {
            SetAnimationState(EnemyStateType.PLAYERFOUND);
            StopAgent();
            m_TimeWatchDeadTarget += Time.deltaTime;
            
            if (m_TimeWatchDeadTarget > LookAtDeadTargetDuration && 
                Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && 
                Animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerFound"))
            {
                m_TimeWatchDeadTarget = 0.00f;
                m_enemyManager.SetAlert();
                UIManager.Instance.View_Alert.OnShow();
                if (CanSeePlayer() && m_enemyManager.IsAlertActivated)
                {
                    Statemachine.OnTransitionState(StateContainer.State_PlayerFound);
                    return;
                }
                Statemachine.OnTransitionState(StateContainer.State_Patrol); //need to change patrol behviour??
            }
        }
    }

    public void OnLookUpdate(EnemyStateType _animState, string _animName) 
    {
        if (!m_agent.isStopped)
        {
            StopAgent();
            this.SetAnimationState(_animState);
        }
        if ( Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && Animator.GetCurrentAnimatorStateInfo(0).IsName(_animName))
        {
            Statemachine.OnTransitionState(StateContainer.State_Patrol); //was idle before
        }
    }
    #endregion
    private void IncreaseRage()
    {
        //to be verify 
        if (EnemyRage < MaximunRage)
        {

            float _multiplier = 0;
            if (Vector3.Distance(transform.position, PlayerRef.transform.position) <= DetectionRadius / 2)
            {
                _multiplier = RageMultiplier / 2;
            }
            //EnemyRage += (RageMultiplier + _multiplier + RageMultiplier);// to to test _currentHP good value for ragmultiplier set on how may raycasthit the target
            if(EnemyRage < MaximunRage)
            {
                EnemyRage += (RageMultiplier + _multiplier); // need to add  * Time.Deltatime ?
                if (!EnemyCueHandler.View_sightedBar.gameObject.activeSelf)//triangle
                {
                    EnemyCueHandler.View_sightedBar.gameObject.SetActive(true);
                }
                EnemyCueHandler.View_sightedBar.CueSightedElement.SetFilling(EnemyRage / MaximunRage);
            }
        }
    }

    public void SetDistract(bool _state, Vector3 _objPosition)
    {
        if (IsValidPath(_objPosition) && _state)
        {
            if (!IsDistract)
            {
                IsDistract = true;
            }
            m_nextPath = _objPosition;
        }
    }

    public void LookAtTarget(Vector3 _target)
    {
        var lookAtTarget = new Vector3(_target.x, transform.position.y, _target.z);
        this.transform.LookAt(lookAtTarget);
    }

    #region Boolean function to check if can enter any state
    public bool CanChasse()
    {
        return EnemyRage >= 100f;
    }
    // 50% of maxrage value and check if player not too high 
    public bool CanInvestigate()
    {
        return EnemyRage >= (MaximunRage / 2) && PlayerRef.transform.position.y  <= 2.5f; 
    }
    public  bool InAttackRange()
    {
        return Vector3.Distance(transform.position, Entity_Player.Instance.transform.position) <= AttackRange;
    }

    public bool CanShoot()
    {
        m_TimeWatchFireRate += Time.deltaTime;
        if(m_TimeWatchFireRate > WeaponFireRate)
        {
            m_TimeWatchFireRate = 0.0f;
            return true;
        }
        return false;
    }

    public void RandomIdleWaitTime()
    {
        m_idleTime = Random.Range(3, 7);
    }
    public bool IsIdleTimeDone()
    {
        m_idlestopwatch += Time.deltaTime;
        if (m_idlestopwatch > m_idleTime)
        {
            ResetIdleTimer();
            return true;
        }
        return false;
    }
    public void ResetIdleTimer()
    {
        m_idlestopwatch = 0f;
    }

    public bool InvestigateTargetState()
    {
        if (m_enemyManager.IsAlertActivated)
        {
            return false;
        }
        Vector3 _targetPosition = Vector3.zero;
        if (m_enemyManager.TargetToKill)
        {
            _targetPosition = m_enemyManager.TargetToKill.transform.position;
        }
        if (Vector3.Distance(transform.position, _targetPosition) < DetectionRadius)
        {


            var _position = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);//offset otherwise hit ground
            Vector3 _targetDirection = (_targetPosition - this.transform.position).normalized;
            float _tagetDistance = Vector3.Distance(transform.position, _targetDirection);
           // UnityEngine.Debug.DrawLine(_position, _position + _targetDirection * _tagetDistance, Color.green, 3f);
            if (Vector3.Angle(transform.forward, _targetDirection) < FieldOfView / 2 &&
                Physics.Raycast(_position, _targetDirection, out RaycastHit _hit, _tagetDistance))
            {
                var _enemyCast = _hit.collider.GetComponentInParent<Entity_Enemy>();
                if (_enemyCast)
                {
                    if (_enemyCast.Type == EnemyType.TARGET && _enemyCast.IsDead)
                    {
                        print("Target Dead found");
                        m_deadTargetPos = _targetPosition;
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public bool CanSeePlayer()
    {
        
        
        if (Physics.CheckSphere(transform.position, DetectionRadius, 1 << 10)) //player layer = 10
        {
            Vector3 _targetDirection = (PlayerRef.transform.position - this.transform.position).normalized;
            float _tagetDistance = Vector3.Distance(transform.position, PlayerRef.transform.position);
            if (Vector3.Angle(transform.forward, _targetDirection) < FieldOfView /2   && CheckForPLayer(_targetDirection, _tagetDistance))
            {
                m_isPLayerDetect = true;
                    m_timeWatchLoosePLayer = 0.00f;
                    PlayerLastSeenPos = PlayerRef.transform.position;

                return true;
            }
            else
            {
                
                m_timeWatchLoosePLayer += Time.deltaTime;
                if ( m_timeWatchLoosePLayer > TimeBeforeLoosePLayerFocus)
                {
                    m_isPLayerDetect = false; // maybe put inside enemyrage
                    if (EnemyRage > 0)
                    {
                        EnemyRage -= 1f;
                        EnemyCueHandler.View_sightedBar.CueSightedElement.SetFilling(EnemyRage / MaximunRage);
                    }
                    return false;
                }
            }
        }
        return false;
    }

    private bool CheckForPLayer(Vector3 _TargetDir, float _tagetDistance)
     {
        float scale1 = 0.5f, scale2 = 1.1f, scale3 = 1.3f;
        float _directionScale = 0.015f;
        Vector3 _offsetDir1 = new Vector3(_TargetDir.x + _directionScale, _TargetDir.y, _TargetDir.z);
        Vector3 _offsetDir2 = new Vector3(_TargetDir.x - _directionScale, _TargetDir.y, _TargetDir.z);

        bool playerFound = false;
        var _position1 = new Vector3(transform.position.x, transform.position.y + scale1, transform.position.z);
        var _position2 = new Vector3(transform.position.x, transform.position.y + scale2, transform.position.z);
        var _position3 = new Vector3(transform.position.x, transform.position.y + scale3, transform.position.z);

        UnityEngine.Debug.DrawLine(_position1, _position1 + _TargetDir * _tagetDistance, Color.red);
        UnityEngine.Debug.DrawLine(_position2, _position2 + _TargetDir * _tagetDistance, Color.red);
        UnityEngine.Debug.DrawLine(_position3, _position2 + _offsetDir1 * _tagetDistance, Color.red);
        UnityEngine.Debug.DrawLine(_position3, _position2 + _offsetDir2 * _tagetDistance, Color.red);
        UnityEngine.Debug.DrawLine(_position3, _position3 + _TargetDir * _tagetDistance, Color.red);

        float _hitcount = 0;
         Physics.Raycast(_position1, _TargetDir, out m_playerHitspoints[0], _tagetDistance);
         Physics.Raycast(_position2, _TargetDir, out m_playerHitspoints[1], _tagetDistance);
         Physics.Raycast(_position2, _offsetDir1, out m_playerHitspoints[2], _tagetDistance);
         Physics.Raycast(_position2, _offsetDir2, out m_playerHitspoints[3], _tagetDistance);
         Physics.Raycast(_position3, _TargetDir, out m_playerHitspoints[4], _tagetDistance);
        {
            for (int i = 0; i < m_playerHitspoints.Length; i++)
            {
                if (m_playerHitspoints[i].collider != null)
                {
                    if (m_playerHitspoints[i].collider.gameObject.GetComponent<Entity_Player>())
                    {
                        if(m_rayHitChance != 0)
                        {
                            m_rayHitChance = 0;
                        }
                        _hitcount += RagePerRayHit;
                        if (!playerFound)
                        {
                            playerFound = true;
                        }
                    }
                }
            }
        }
        RageMultiplier = _hitcount;
        
        if (!playerFound && m_rayHitChance < 6)
        {
            m_rayHitChance++;
            playerFound = true;
        }
        //print(playerFound);
        //print(RageMultiplier);
        //print(_hitcount);
        return playerFound;
    }
    #endregion



    #region RAGDOLL
    private void EnableRagdoll(bool _isActive)
    {
        Collider[] m_colls = GetComponentsInChildren<Collider>();
        foreach (var coll in m_colls)
        {
            if (coll != Collider)//main collider
            {
                coll.isTrigger = !_isActive;
                if (coll.attachedRigidbody)
                {
                    coll.GetComponent<Rigidbody>().isKinematic = !_isActive;
                }
            }
        }
        Rigidbody.isKinematic = _isActive;
        Collider.enabled = !_isActive;
    }
    #endregion

    #region OnHit
    public override void OnHit(int damage)
    {
        base.OnHit(damage);
        if (damage > 0)
        {
            if (!IsDead && !m_enemyCueHandler.View_HP.gameObject.activeSelf)
            {
                m_enemyCueHandler.View_HP.OnShow();
            }
            if (!m_isPLayerDetect)
            {
                isHit = true; // todo :
            }
            //HitParticule.transform.position = transform.position;
            HitParticule.Play();
            m_enemyCueHandler.View_HP.DefaultSelectedElement.SetTitle(m_currentHP.Value.ToString("D3"));
		  

            float _currentHP = m_currentHP.Value;
            float _maxHP = m_maxHP.Value;
            float _normalised = _currentHP / _maxHP;
            m_enemyCueHandler.View_HP.FillingBarElement.SetFilling(_normalised);
        }
    }
    #endregion

    #region DEAD BEHAVIOUR
    public override void OnDeath()
    {
        //WeaponSocket.GetComponentInChildren<Entity_Object_Weapon>().Animator.enabled = false;
        m_enemyCueHandler.OnHideAll();
        StopAgent();
        Animator.enabled = false;
        EnableRagdoll(true);
        /*m_agent.enabled = false;
        var obstacle = gameObject.AddComponent<NavMeshObstacle>();
        obstacle.enabled = true;
        obstacle.carving = true;*/
        /*GetComponent<NavMeshObstacle>().enabled = true;
        GetComponent<NavMeshObstacle>().carving = true;*/

        //m_enemyManager.killCount++;
        if (m_enemyManager.IsTargetDead())
        {
            m_enemyManager.canKillOther = true;
            if (EnemyManager.Instance.IsTargetDead() && this.Type == EnemyType.TARGET)
            {
                QuestManager.Instance.currentKillQuest.OnCompleteQuest();
            }
            // TEMP END OF GAME
            if (!GameManager.Instance.IsGameOver)
            {
                GameManager.Instance.IsGameOver = true;
               // SceneLoadManager.Instance.OnLoadMainMenu(); // TODO: Delete when implementing proper end of game
            }
            return;
        }
        else if (!m_enemyManager.canKillOther)
        {
            print("you loose");

            // TEMP END OF GAME
            if (!GameManager.Instance.IsGameOver)
            {
                GameManager.Instance.IsGameOver = true;
                //SceneLoadManager.Instance.OnLoadMainMenu(); // TODO: Delete when implementing proper end of game
            }
            //gameover and restart
        }
    }
    #endregion
}
