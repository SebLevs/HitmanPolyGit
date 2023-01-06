using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View_Quest : View_Base
{
    [Header("Quest section")]
    private const float m_popupTime = 1;

    [Space(10)]
    [SerializeField] private GameObject _questPrefabRef;
    [SerializeField] private GameObject m_questsContainer;

    [Header("Quests")]
    [SerializeField] private UIElement_Quest m_mainQuest;
    [SerializeField] private List<UIElement_Quest> m_sideQuests = new List<UIElement_Quest>();

    public UIElement_Quest MainQuest { get => m_mainQuest; }

    protected override void Init()
    {
        base.Init();
        //m_sideQuests = new List<UIElement_Quest>();
    }

    public UIElement_Quest GetLastSideQuest()
    {
        return m_sideQuests[m_sideQuests.Count];
    }

    public UIElement_Quest GetSideQuest(int index)
    {
        return m_sideQuests[index];
    }

    /// <summary>
    /// Add a quest UI element to the list of printed quest and returns it.
    /// </summary>
    /// <returns>Returns the newly added quest</returns>
    public UIElement_Quest AddSideQuest(string _title, string _body, int _requiredCompletion = 0)
    {
        UIElement_Quest _newQuest = Instantiate(_questPrefabRef, m_questsContainer.transform).GetComponentInChildren<UIElement_Quest>();
        _newQuest.SetTitle(_title);
        _newQuest.SetBody(_body);
        m_sideQuests.Add(_newQuest);
        if (_requiredCompletion > 0)
        {
            _newQuest.SetRequiredCompletion(_requiredCompletion);
            _newQuest.SetActiveCompletionObject(true);
        }

        if(_newQuest.isActiveAndEnabled)
        {
            _newQuest.StartCoroutine(_newQuest.PopupQuestIE());
        }

        return _newQuest;
    }
    
    public void ResetQuestsTexts()
    {
        m_mainQuest.ResetQuest();
        m_sideQuests.ForEach(x => x.ResetQuest());
    }

    public void ClearQuests()
    {
        m_mainQuest.ResetQuest();
        m_sideQuests.Clear();
    }

    public void OnResetDestroyAllUIElement()
    {
        m_sideQuests.ForEach(x => 
        {
            Destroy(x.gameObject);
        });
        m_sideQuests.Clear();
    }
}
