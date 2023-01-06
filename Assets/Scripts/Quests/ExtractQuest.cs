using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractQuest : BaseQuest
{
    public string extractname = "Undefined";
    public ExtractPoints point = null;
    public ExtractQuest(string questDescription, string extractname) : base("Extraction", questDescription)
    {
        this.extractname = extractname;
    }

    public override void OnCompleteQuest()
    {
        if (point.ExtractName == QuestManager.Instance.currentExtractQuest.extractname)
        {
            base.OnCompleteQuest();
            QuestManager.Instance.currentExtractQuest.questCompeted = true;
            HighScoreManager.Instance.QuestExtractDone = true;
            HighScoreManager.Instance.QuestsDone++;
            UIManager.Instance.View_QuestCompletion.OnPopupQuestCompletion(true, questTitle);
            UIManager.Instance.View_highscore.SectionExtractElement.SetQuestSuccessState(HighScoreManager.Instance.QuestExtractDone);
            Debug.Log("Extract Quest complete");
        }
        else
        {
            HighScoreManager.Instance.QuestExtractDone = false;
            UIManager.Instance.View_QuestCompletion.OnPopupQuestCompletion(false, questTitle);
            UIManager.Instance.View_highscore.SectionExtractElement.SetQuestSuccessState(HighScoreManager.Instance.QuestExtractDone);
            elem.OnFaillure();
        }
    }
}
