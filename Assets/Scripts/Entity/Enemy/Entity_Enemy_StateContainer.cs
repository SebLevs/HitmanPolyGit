using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Enemy_StateContainer 
{
    public Enemy_StateIdle State_Idle { get; private set; } //idle
    public Enemy_StatePatrol State_Patrol { get; private set;} //patrol 
    public Enemy_StateDistract State_Distract { get; private set; } //distract
    public Enemy_StateInvestigate State_Investigate { get; private set;}
    public Enemy_StatePlayerFound State_PlayerFound { get; private set;} // playerFound
    public Enemy_StateChasse State_Chasse { get; private set;} //chasse
    public Enemy_StateAttack State_Attack { get; private set; } //Attack
    public Enemy_StateDead State_Dead { get; private set; } // just call dead method from parent
    public Enemy_StateDeadTarget State_DeadTarget { get; private set; } // check the actual target 



    public Entity_Enemy_StateContainer(Entity_Enemy _enemyRef)
    {
        State_Idle = new Enemy_StateIdle(_enemyRef);
        State_Patrol = new Enemy_StatePatrol(_enemyRef);
        State_Distract = new Enemy_StateDistract(_enemyRef);
        State_Investigate = new Enemy_StateInvestigate(_enemyRef);
        State_PlayerFound = new Enemy_StatePlayerFound(_enemyRef);
        State_Chasse = new Enemy_StateChasse(_enemyRef);
        State_Attack = new Enemy_StateAttack(_enemyRef);
        State_Dead = new Enemy_StateDead(_enemyRef);
        State_DeadTarget = new Enemy_StateDeadTarget(_enemyRef);
    }
}
public class Enemy_StateIdle : State_Origin<Entity_Enemy>
{

    public Enemy_StateIdle(Entity_Enemy context) : base(context)
    {
    }

    public override void HandleStateTransition()
    {
    }

    public override void OnEnter()
    {
        Debug.Log("Idle State Enter");
        base.m_context.OnIdleEnter();
    }

    public override void OnExit()
    {
        base.m_context.ResetIdleTimer();
    }

    public override void OnUpdate()
    {
        base.m_context.OnIdleUpdate();

    }
}

public class Enemy_StatePatrol : State_Origin<Entity_Enemy>
{
    public Enemy_StatePatrol(Entity_Enemy context) : base(context)
    {
    }

    public override void HandleStateTransition()
    {
    }

    public override void OnEnter()
    {
        //go to node
        base.m_context.OnPatrolEnter();
        Debug.Log("Patrol State Enter");
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        base.m_context.OnPatrolUpdate();

    }
}
public class Enemy_StateInvestigate : State_Origin<Entity_Enemy>
{
    public Enemy_StateInvestigate(Entity_Enemy context) : base(context)
    {
    }

    public override void HandleStateTransition()
    {
    }

    public override void OnEnter()
    {

        base.m_context.OnInvestigateEnter();
        Debug.Log("Investigate State Enter");
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        base.m_context.OnInvestigateUpdate();
    }
}

[Serializable]
public class Enemy_StateChasse : State_Origin<Entity_Enemy>
{
    public Enemy_StateChasse(Entity_Enemy context) : base(context)
    {
    }

    public override void HandleStateTransition()
    {
    }

    public override void OnEnter()
    {
        Debug.Log("Chasse player State");
        base.m_context.OnChasseEnter();
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        // Chasse STATE CAN -> DIE ,  NOT SEE_Player, Attack

        base.m_context.OnChasseUpdate();
    }
}



[Serializable]
public class Enemy_StateDeadTarget : State_Origin<Entity_Enemy>
{
    public Enemy_StateDeadTarget(Entity_Enemy context) : base(context)
    {
    }

    public override void HandleStateTransition()
    {
    }

    public override void OnEnter()
    {
        Debug.Log("dead target state");
        base.m_context.OnSeeDeadTargetEnter();
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        base.m_context.OnSeeDeadTargetUpdate();

    }
}
public class Enemy_StateDead : State_Origin<Entity_Enemy>
{
    public Enemy_StateDead(Entity_Enemy context) : base(context)
    {
    }

    public override void HandleStateTransition()
    {
    }

    public override void OnEnter()
    {
        Debug.Log("dead state");
        //base.m_context.OnDeath();
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
    }
}


//TArget is dead what the enemy need to do
public class Enemy_StatePlayerFound : State_Origin<Entity_Enemy>
{
    public Enemy_StatePlayerFound(Entity_Enemy context) : base(context)
    {
    }

    public override void HandleStateTransition()
    {
    }

    public override void OnEnter()
    {
        Debug.Log("See player State");
        base.m_context.OnPlayerFoundStart();
        base.m_context.EnemyCueHandler.View_sightedBar.OnShow();
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        base.m_context.OnPlayerFoundUpdate();
    }
}

public class Enemy_StateDistract : State_Origin<Entity_Enemy>
{
    public Enemy_StateDistract(Entity_Enemy context) : base(context)
    {
    }

    public override void HandleStateTransition()
    {
    }

    public override void OnEnter()
    {
        base.m_context.OnDistractEnter();
        Debug.Log("DISTRACT STATE ENTER");
    }

    public override void OnExit()
    {



    }

    public override void OnUpdate()
    {

        base.m_context.OnDistractUpdate();

    }
}
public class Enemy_StateAttack : State_Origin<Entity_Enemy>
{
    public Enemy_StateAttack(Entity_Enemy context) : base(context)
    {
    }

    public override void HandleStateTransition()
    {
    }

    public override void OnEnter()
    {
        Debug.Log("ATTACK STATE ENTER");
        base.m_context.OnShotEnter();
    }

    public override void OnExit()
    {


    }

    public override void OnUpdate()
    {
        base.m_context.OnShotUpdate();

        

    }
}
