using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Quest section<br/>
/// 
/// Contains: <br/>
/// . Succcess Icon (X or V)<br/>
/// . Section title (e.g.: Kill quest)
/// </summary>
public class UIElement_HighScoreQuestSection : UIElement_Base
{
    [Header("Image")]
    [SerializeField] private Image m_successStateIcon;
    [SerializeField] private Sprite m_completedSprite;
    [SerializeField] private Sprite m_failedSprite;

    [Header("Underline")]
    [SerializeField] private Image m_underlineBar;

    public Image SuccessStateIcon { get => m_successStateIcon; }

    public void SetQuestSuccessState(bool _isCompleted, string _newTitle = null)
    {
        SetSucccessIcon(_isCompleted);
        if (_newTitle != null)
        {
            SetTitle(_newTitle);
        }
    }

    private void SetSucccessIcon(bool _isCompleted)
    {
        if (_isCompleted)
        {
            m_successStateIcon.sprite = m_completedSprite;
        }
        else
        {
            m_successStateIcon.sprite = m_failedSprite;
        }
    }
}
