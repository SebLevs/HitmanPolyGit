using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseQuest
{
    public string questTitle;
    public string questDescription;
    public bool questCompeted = false;
    public UIElement_Quest elem;

    public BaseQuest(string questTitle, string questDescription)
    {
        this.questTitle = questTitle;
        this.questDescription = questDescription;
    }

    protected virtual void OnQuestUpdate()
    {

    }

    public virtual void OnCompleteQuest()
    {
        elem.OnCompletion();
    }
}
