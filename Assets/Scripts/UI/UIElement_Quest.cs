using TMPro;
using UnityEngine;

public class UIElement_Quest : UIElement_QuestBase
{
    [Header("Completion")]
    [SerializeField] private TextMeshProUGUI m_currentCompletion;
    [SerializeField] private TextMeshProUGUI m_RequiredCompletion;

    protected override void OnStart()
    {
        base.OnStart();
        OnPopup();
    }

    public void SetActiveCompletionObject(bool _setAs)
    {
        m_currentCompletion.transform.parent.gameObject.SetActive(_setAs);
    }

    public void SetCurrentCompletion(int _current)
    {
        m_currentCompletion.text = _current.ToString("D2");
    }

    public void SetRequiredCompletion(int _required)
    {
        m_RequiredCompletion.text = _required.ToString("D2");
    }

    public void OnCompletion()
    {
        m_title.color = Color.green;
        string completionBody = "<s>" + m_body.text + "</s>";
        m_body.text = completionBody;
    }

    public void OnFaillure()
    {
        m_title.color = Color.red;
        string completionBody = "<s>" + m_body.text + "</s>";
        m_body.text = completionBody;
    }

    public void ResetQuest(bool _setActiveCompletion = false)
    {
        m_title.color = Color.white;
        m_title.text = "Foo";
        m_body.text = "Bar";
        SetActiveCompletionObject(_setActiveCompletion);
    }
}
