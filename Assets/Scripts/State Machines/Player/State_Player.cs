using System;
using UnityEngine;

[Serializable]
public class StateContainer_Player
{
    public State_PlayerIddle IddleState { get; private set; }
    public State_PlayerMoving MovingState { get; private set; }
    public State_PlayerJumping JumpingState { get; private set; }
    public State_PlayerAttack AttackState { get; private set; }
    public State_PlayerCrouch CrouchState { get; private set; }
    public State_PlayerPickup PickupState { get; private set; }
    public State_PlayerThrow ThrowState { get; private set; }
    public State_PlayerLookTarget LookTargetState { get; private set; }

    public State_PlayerDead DeadState { get; private set; }

    //public State_PlayerADS ADSState { get; private set; }

    public StateContainer_Player(Entity_Player context)
    {
        IddleState = new State_PlayerIddle(context);
        MovingState = new State_PlayerMoving(context);
        JumpingState= new State_PlayerJumping(context);
        AttackState= new State_PlayerAttack(context);
        CrouchState = new State_PlayerCrouch(context);
        PickupState = new State_PlayerPickup(context);
        ThrowState = new State_PlayerThrow(context);
        LookTargetState = new State_PlayerLookTarget(context);
        DeadState = new State_PlayerDead(context);
        //ADSState= new State_PlayerADS(context);
    }
}

[Serializable]
public class State_PlayerIddle : State_Origin<Entity_Player>
{
    public State_PlayerIddle(Entity_Player context): base(context)
    {
    }

    public override void HandleStateTransition()
    {
    }

    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        if(m_context.IsDead)
        {
            m_context.MasterOfState.OnTransitionState(m_context.StateContainer.DeadState);
        }
        if (InputReceiver.Instance.moveDirection != Vector2.zero)
        {
            m_context.MasterOfState.OnTransitionState(m_context.StateContainer.MovingState);
        }

        if (m_context.DesiredActions.Contains(PlayerActionsType.WEAPONATTACK))
        {
            m_context.MasterOfState.OnTransitionState(m_context.StateContainer.AttackState);
        }

        if (m_context.DesiredActions.Contains(PlayerActionsType.JUMP))
        {
            m_context.MasterOfState.OnTransitionState(m_context.StateContainer.JumpingState);
        }

        if (m_context.DesiredActions.Contains(PlayerActionsType.CROUCH))
        {
            m_context.MasterOfState.OnTransitionState(m_context.StateContainer.CrouchState);
        }

        // Pickup
        if (m_context.DesiredActions.Contains(PlayerActionsType.PICKUP))
        {
            m_context.MasterOfState.OnTransitionState(m_context.StateContainer.PickupState);
        }

        // Throw
        if (m_context.DesiredActions.Contains(PlayerActionsType.THROW))
        {
            m_context.MasterOfState.OnTransitionState(m_context.StateContainer.ThrowState);
        }

        //Look at target menu
        if (m_context.DesiredActions.Contains(PlayerActionsType.LOOKATTARGET))
        {
            m_context.MasterOfState.OnTransitionState(m_context.StateContainer.LookTargetState);
        }

    }
}

[Serializable]
public class State_PlayerMoving : State_Origin<Entity_Player>
{
    public State_PlayerMoving(Entity_Player context): base(context)
    {
    }

    public override void HandleStateTransition()
    {
    }

    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        if (m_context.IsDead)
        {
            m_context.MasterOfState.OnTransitionState(m_context.StateContainer.DeadState);
        }
        // Move
        Vector3 playermove = m_context.transform.forward * InputReceiver.Instance.moveDirection.y + m_context.transform.right * InputReceiver.Instance.moveDirection.x;
        m_context.MoveVelo = new Vector3(playermove.x * m_context.MoveSpeed, 0, playermove.z * m_context.MoveSpeed);    
        
        // Jump
        if(m_context.DesiredActions.Contains(PlayerActionsType.JUMP))
        {
            m_context.MasterOfState.OnTransitionState(m_context.StateContainer.JumpingState);
        }

        // Weapon
        if (m_context.DesiredActions.Contains(PlayerActionsType.WEAPONATTACK))
        {
           m_context.MasterOfState.OnTransitionState(m_context.StateContainer.AttackState);
        }

        // CROUCH
        if(m_context.DesiredActions.Contains(PlayerActionsType.CROUCH))
        {
            m_context.MasterOfState.OnTransitionState(m_context.StateContainer.CrouchState);
        }

        // Pickup
        if (m_context.DesiredActions.Contains(PlayerActionsType.PICKUP))
        {
            m_context.MasterOfState.OnTransitionState(m_context.StateContainer.PickupState);
        }

        // Throw
        if (m_context.DesiredActions.Contains(PlayerActionsType.THROW))
        {
            if (m_context.Throwable)
            {
                m_context.MasterOfState.OnTransitionState(m_context.StateContainer.ThrowState);
            }
        }

        //Look at target menu
        if (m_context.DesiredActions.Contains(PlayerActionsType.LOOKATTARGET))
        {
            m_context.MasterOfState.OnTransitionState(m_context.StateContainer.LookTargetState);
        }
    }
}
[Serializable]
public class State_PlayerJumping : State_Origin<Entity_Player>
{
    public State_PlayerJumping(Entity_Player context) : base(context)
    {
    }

    public override void HandleStateTransition()
    {
        
    }

    public override void OnEnter()
    {
        if(m_context.IsGrounded())
        {
            m_context.JumpVelo = Vector3.up * m_context.JumpHeight;
            //m_context.GetComponent<Rigidbody>().AddForce(new Vector3(0, m_context.JumpHeight, 0), ForceMode.Impulse);
            m_context.DesiredActions.ConsumeAllActions(PlayerActionsType.JUMP);
        }
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        if (m_context.IsDead)
        {
            m_context.MasterOfState.OnTransitionState(m_context.StateContainer.DeadState);
        }
        m_context.MasterOfState.OnTransitionState(m_context.StateContainer.MovingState);

        if (m_context.DesiredActions.Contains(PlayerActionsType.WEAPONATTACK))
        {
            m_context.MasterOfState.OnTransitionState(m_context.StateContainer.AttackState);
        }
    }
}
[Serializable]
public class State_PlayerAttack : State_Origin<Entity_Player>
{
    public State_PlayerAttack(Entity_Player context) : base(context)
    {
    }

    public override void HandleStateTransition()
    {
    }

    public override void OnEnter()
    {
        if (m_context.Weapon)
        {
            if(!m_context.Weapon.Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                m_context.Weapon.OnAttack();
                Debug.Log("ATTACK");

            }
            else
            {
                Debug.Log("CANT ATTACK");
            }
        }
        m_context.DesiredActions.ConsumeAllActions(PlayerActionsType.WEAPONATTACK);
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        if (m_context.IsDead)
        {
            m_context.MasterOfState.OnTransitionState(m_context.StateContainer.DeadState);
        }
        if (m_context.DesiredActions.Contains(PlayerActionsType.WEAPONATTACK))
        {
            if (m_context.Weapon)
            {
                m_context.MasterOfState.OnTransitionState(m_context.StateContainer.AttackState);
            }
        }
        else
        {
            m_context.MasterOfState.OnTransitionState(m_context.StateContainer.MovingState);
        }
    }
}

[Serializable]
public class State_PlayerCrouch : State_Origin<Entity_Player>
{
    public State_PlayerCrouch(Entity_Player context) : base(context)
    {
    }

    public override void HandleStateTransition()
    {
    }

    public override void OnEnter()
    {
        if(m_context.canCrouch)
        {
            if (!m_context.IsCrouch)
            {
                m_context.GetComponent<Animator>().Play("Crouch");
                m_context.IsCrouch = true;
                m_context.MoveSpeed = m_context.CrouchSpeed;
                m_context.JumpHeight = m_context.CrouchJumpHeight;
            }
            else
            {
                m_context.GetComponent<Animator>().Play("UnCrouch");
                m_context.IsCrouch = false;
                m_context.MoveSpeed = m_context.UncrouchSpeed;
                m_context.JumpHeight = m_context.UncrouchJumpHeight;
            }
        }
        m_context.DesiredActions.ConsumeAllActions(PlayerActionsType.CROUCH);
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        if (m_context.IsDead)
        {
            m_context.MasterOfState.OnTransitionState(m_context.StateContainer.DeadState);
        }
        if (m_context.DesiredActions.Contains(PlayerActionsType.WEAPONATTACK))
        {
            m_context.MasterOfState.OnTransitionState(m_context.StateContainer.AttackState);
        }
        else
        {
            m_context.MasterOfState.OnTransitionState(m_context.StateContainer.MovingState);
        }
    }
}

//public class State_PlayerADS : State_Origin<Entity_Player>
//{
//    public State_PlayerADS(Entity_Player context) : base(context)
//    {
//    }

//    public override void HandleStateTransition()
//    {
//    }

//    public override void OnEnter()
//    {
//        Debug.Log("In ADS");
//        if (m_context.CurrentPrimaryWeapon.GetComponent<Animator>())
//        {
//            m_context.CurrentPrimaryWeapon.GetComponent<Animator>().Play("ADS_IN");
//            Debug.Log("In animADS");
//        }
//        m_context.WeaponRange = 5.0f;
//        m_context.DesiredActions.ConsumeAllActions(PlayerActionsType.WEAPONATTACK);
//    }

//    public override void OnExit()
//    {
//        if (m_context.CurrentPrimaryWeapon.GetComponent<Animator>())
//        {
//            m_context.CurrentPrimaryWeapon.GetComponent<Animator>().Play("ADS_OUT");
//        }
//    }

//    public override void OnUpdate()
//    {
//        if (m_context.DesiredActions.Contains(PlayerActionsType.WEAPONATTACK))
//        {
//            m_context.MasterOfState.OnTransitionState(m_context.StateContainer.AttackState);
//        }
//        else
//        {
//            m_context.MasterOfState.OnTransitionState(m_context.StateContainer.MovingState);
//        }


//    }
//}

[Serializable]
public class State_PlayerThrow : State_Origin<Entity_Player>
{
    public State_PlayerThrow(Entity_Player context) : base(context)
    {
    }

    public override void HandleStateTransition()
    {

    }

    public override void OnEnter()
    {
        m_context.DesiredActions.ConsumeAllActions(PlayerActionsType.THROW);
        if (m_context.Throwable)
        {
            m_context.Throwable.OnAttack();
        }
        if (m_context.IsDead)
        {
            m_context.MasterOfState.OnTransitionState(m_context.StateContainer.DeadState);
        }
        m_context.MasterOfState.OnTransitionState(m_context.StateContainer.IddleState);
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {


    }
}

[Serializable]
public class State_PlayerPickup : State_Origin<Entity_Player>
{
    public State_PlayerPickup(Entity_Player context) : base(context)
    {
    }

    public override void HandleStateTransition()
    {

    }

    public override void OnEnter()
    {

        m_context.DesiredActions.ConsumeAllActions(PlayerActionsType.PICKUP);
        // TODO: Add pickup to slot here
        if (m_context.LastSeenPickable != null)
        {
            m_context.LastSeenPickable.OnPickup();
        }
        else if (m_context.Throwable)
        {
            m_context.Throwable.OnDrop();
        }
        if (m_context.IsDead)
        {
            m_context.MasterOfState.OnTransitionState(m_context.StateContainer.DeadState);
        }
        m_context.MasterOfState.OnTransitionState(m_context.StateContainer.IddleState);
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
    }
}

[Serializable]
public class State_PlayerLookTarget : State_Origin<Entity_Player>
{
    public State_PlayerLookTarget(Entity_Player context) : base(context)
    {
    }

    public override void HandleStateTransition()
    {
    }

    public override void OnEnter()
    {
        UIManager uiManager = UIManager.Instance;

        m_context.DesiredActions.ConsumeAllActions(PlayerActionsType.LOOKATTARGET);
        if (uiManager.CurrentView is View_Crosshair)
        {
            uiManager.OnSwitchViewSequential(uiManager.View_Target);
        }
        else
        {
            uiManager.OnSwitchViewSequential(uiManager.View_Crosshair);
            m_context.MasterOfState.OnTransitionState(m_context.StateContainer.MovingState);
        }
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        if (m_context.IsDead)
        {
            m_context.MasterOfState.OnTransitionState(m_context.StateContainer.DeadState);
        }
        //Look at target menu
        if (m_context.DesiredActions.Contains(PlayerActionsType.LOOKATTARGET))
        {
            m_context.MasterOfState.OnTransitionState(m_context.StateContainer.LookTargetState);
        }
        // Move
        Vector3 playermove = m_context.transform.forward * InputReceiver.Instance.moveDirection.y + m_context.transform.right * InputReceiver.Instance.moveDirection.x;
        m_context.MoveVelo = new Vector3(playermove.x * m_context.MoveSpeed, 0, playermove.z * m_context.MoveSpeed);
    }
}

[Serializable]
public class State_PlayerDead : State_Origin<Entity_Player>
{
    public State_PlayerDead(Entity_Player context) : base(context)
    {
    }

    public override void HandleStateTransition()
    {
    }

    public override void OnEnter()
    {
        Debug.Log("We Dead, dog!");
        EnemyManager.Instance.DeactivateAlert();
        m_context.InputReceiver.moveDirection = Vector2.zero;
        SceneLoadManager.Instance.OnClearBeforeLeavingLevel();
        UIManager _uIManager = UIManager.Instance;
        _uIManager.OnSwitchViewSynchronous(_uIManager.View_deathScreen);
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        
    }
}

