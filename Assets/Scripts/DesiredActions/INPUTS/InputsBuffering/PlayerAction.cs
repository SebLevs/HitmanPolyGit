using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction
{
    public PlayerActionsType actionType;
    private float m_currentLifeTime = 0.00f;
    private const float MAX_LIFETIME = 0.08f;

    public PlayerAction(PlayerActionsType _actionType)
    {
        this.actionType = _actionType;
    }
    public void GetOlder()
    {
        this.m_currentLifeTime += Time.deltaTime;
    }
    public bool IsTooOld()
    {
        return this.m_currentLifeTime >= MAX_LIFETIME;
    }
}

