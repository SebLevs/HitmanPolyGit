using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupQuest : BaseQuest
{
    public string itemName = "Undefined";
    public string locationName = "Undefined";
    public Entity_Object_PickupQuest test;
    public PickupQuest(string questDescription, string itemName, string locationName) : base("Pickup Item", questDescription)
    {
        this.itemName= itemName;
        this.locationName=locationName;
    }

    public override void OnCompleteQuest()
    {
        Debug.Log(QuestManager.Instance.currentPickupQuest.itemName + "-----" + this.itemName);
        if (test.itemName == QuestManager.Instance.currentPickupQuest.itemName)
        {
            base.OnCompleteQuest();
            QuestManager.Instance.currentPickupQuest.questCompeted = true;
            HighScoreManager.Instance.QuestPickupDone = true;
            HighScoreManager.Instance.QuestsDone++;
            UIManager.Instance.View_QuestCompletion.OnPopupQuestCompletion(true, questTitle);
            UIManager.Instance.View_highscore.SectionPickupElement.SetQuestSuccessState(HighScoreManager.Instance.QuestPickupDone);
            Debug.Log("Pickup Quest complete");
        }
    }
}
