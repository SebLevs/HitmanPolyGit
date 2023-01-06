using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillQuest : BaseQuest
{
    public Entity_Object_Weapon weapon = null;
    public KillQuest(string questDescription, Entity_Object_Weapon weapon) : base("Weapon kill", questDescription)
    {
        this.weapon = weapon;
    }

    public override void OnCompleteQuest()
    {
        Debug.Log(QuestManager.Instance.currentKillQuest.weapon.weaponName + "-----" + Entity_Player.Instance.Weapon.weaponName);
        if (QuestManager.Instance.currentKillQuest.weapon.weaponName == Entity_Player.Instance.Weapon.weaponName)
        {
            base.OnCompleteQuest();
            QuestManager.Instance.currentKillQuest.questCompeted = true;
            HighScoreManager.Instance.QuestKillDone = true;
            HighScoreManager.Instance.QuestsDone++;
            UIManager.Instance.View_QuestCompletion.OnPopupQuestCompletion(true, questTitle);
            UIManager.Instance.View_highscore.SectionKillElement.SetQuestSuccessState(HighScoreManager.Instance.QuestKillDone);
            Debug.Log("Kill Quest Completed");
        }
        else
        {
            elem.OnFaillure();
            UIManager.Instance.View_QuestCompletion.OnPopupQuestCompletion(false, questTitle);
            UIManager.Instance.View_highscore.SectionKillElement.SetQuestSuccessState(HighScoreManager.Instance.QuestKillDone);
            HighScoreManager.Instance.QuestKillDone = false;
        }
        ExtractManager.Instance.activeExtractPoints.ForEach(p => {p.OnActivate(); });
    }
}
