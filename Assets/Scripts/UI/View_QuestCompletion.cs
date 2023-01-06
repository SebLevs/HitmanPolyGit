using UnityEngine;

public class View_QuestCompletion : View_Base
{
    //[SerializeField] private int _defaultPoolObjectQuantity = 5;
    [SerializeField] private PoolPattern<UIElement_QuestCompletion> m_questCompletionPool;

    [Header("Audio")]
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private AudioElement_Single m_validClip;
    [SerializeField] private AudioElement_Single m_invalidClip;

    protected override void Init()
    {
        base.Init();
        m_questCompletionPool.Init();
    }

    public void OnPopupQuestCompletion(bool _questSuccess, string _questTitle)
    {
        UIElement_QuestCompletion _newQuest = m_questCompletionPool.DequeueFromAvailable();

        string finalTitle = "Quest ";
        if (_questSuccess)
        {
            _newQuest.SetTitle(Color.green);
            finalTitle += "COMPLETED";
            m_audioSource.PlayOneShot(m_validClip.GetClip());
        }
        else
        {
            _newQuest.SetTitle(Color.red);
            finalTitle += "FAILED";
            m_audioSource.PlayOneShot(m_invalidClip.GetClip());
        }

        _newQuest.SetTitle(finalTitle);
        _newQuest.SetBody(_questTitle);

        _newQuest.OnPopup(callback: () =>
        {
            _newQuest.OnPopout(callback: () =>
            {
                m_questCompletionPool.DequeueFromCurrentlyUsed();
            });
        });
    }
}


//  TEST: GameObject pool pattern
/*




    [SerializeField] private GameObject _questPrefabRef;
    [SerializeField] private Transform m_questsContainer;
    [SerializeField] private Transform m_availablePoolContainer;

    [SerializeField] private PoolPattern<GameObject> m_questCompletionPool;

    protected override void Init()
    {
        base.Init();
        m_questCompletionPool.Init(10);
    }

    public void OnPopupQuestCompletion(bool _questSuccess, string _questTitle)
    {
        GameObject _newQuest = m_questCompletionPool.DequeueFromAvailable();
        // Can do some stuff with the dequeued quest here

        string finalTitle = "Quest ";
        if (_questSuccess)
        {
            _newQuest.GetComponent<UIElement_QuestCompletion>().SetTitle(Color.green);
            finalTitle += "COMPLETED";
        }
        else
        {
            _newQuest.GetComponent<UIElement_QuestCompletion>().SetTitle(Color.red);
            finalTitle += "FAILED";
        }

        _newQuest.GetComponent<UIElement_QuestCompletion>().SetTitle(finalTitle);
        _newQuest.GetComponent<UIElement_QuestCompletion>().SetBody(_questTitle);

        _newQuest.GetComponent<UIElement_QuestCompletion>().OnPopup(callback: () =>
        {
            _newQuest.GetComponent<UIElement_QuestCompletion>().OnPopout(callback: () =>
            {
                m_questCompletionPool.DequeueFromCurrentlyUsed();
            });
        });
    }
}*/
