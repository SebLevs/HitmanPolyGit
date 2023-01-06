using System;
using UnityEngine;

[Serializable]
public class IntReference
{
    [SerializeField] private int m_constValue;
    [SerializeField] private IntVariable m_intVariable;

    [SerializeField] private bool isConstant = true;
    public int Value
    {
        get => (isConstant) ? m_constValue : m_intVariable.Value;
        set
        {
            if (isConstant)
            {
                m_constValue = value;
            }
            else
            {
                m_intVariable.Value = value;
            }
        }
    }

    public void ClampValue(int min, int max)
    {
        Value = (Value > max) ? max : Value;
        Value = (Value < min) ? min : Value;
    }

    public bool ClampMin(int min)
    {
        bool clamped = Value <= min;
        Value = (clamped) ? min : Value;
        return clamped;
    }

    public bool ClampMax(int max)
    {
        bool clamped = Value > max;
        Value = (clamped) ? max : Value;
        return clamped;
    }
}

[Serializable]
public class FloatReference
{
    [SerializeField] private float m_constValue;
    [SerializeField] private FloatVariable m_floatVariable;

    [SerializeField] private bool isConstant = true;
    public float Value
    {
        get => (isConstant) ? m_constValue : m_floatVariable.Value;
        set
        {
            if (isConstant)
            {
                m_constValue = value;
            }
            else
            {
                m_floatVariable.Value = value;
            }
        }
    }

    public void ClampValue(float min, float max)
    {
        Value = (Value > max) ? max : Value;
        Value = (Value < min) ? min : Value;
    }

    public bool ClampMin(float min)
    {
        bool clamped = Value < min;
        Value = (clamped) ? min : Value;
        return clamped;
    }

    public bool ClampMax(float max)
    {
        bool clamped = Value > max;
        Value = (clamped) ? max : Value;
        return clamped;
    }
}