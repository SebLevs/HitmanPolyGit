using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : Manager<HighScoreManager>
{
    public HighScoreScriptable highScoreScriptable;

    [SerializeField] private float currentTimer;
    public bool pauseTimer = true;

    private int questsDone = 0;
    private bool questExtractDone = false; //Changed in baseQuests
    private bool questKillDone = false;
    private bool questPickupDone = false;

    public bool QuestExtractDone { get => questExtractDone; set => questExtractDone = value; }
    public bool QuestKillDone { get => questKillDone; set => questKillDone = value; }
    public bool QuestPickupDone { get => questPickupDone; set => questPickupDone = value; }
    public int QuestsDone { get => questsDone; set => questsDone = value; }
    public float CurrentTimer { get => currentTimer; set => currentTimer = value; }

    public void ResetGameScore()
    {
        currentTimer = 0f;
        questsDone = 0;
        questExtractDone = false;
        questKillDone = false;
        questPickupDone = false;
}

    public void StartTimer()
    {
        pauseTimer = false;
    }

    public void PauseTimer()
    {
        pauseTimer = true;
    }

    public void ResetTimer()
    {
        currentTimer = 0.0f;
    }

    private void Update()
    {
        if(!pauseTimer)
        {
            currentTimer += Time.deltaTime;
        }
    }

    public void CheckHighScore()
    {
        PauseTimer();
        if (this.questsDone > highScoreScriptable.questsDone || (this.questsDone == highScoreScriptable.questsDone && (this.currentTimer < highScoreScriptable.bestTime || highScoreScriptable.bestTime == 0)))
        {
            UpdateHighScore();
        }
        ResetGameScore();
    }

    private void UpdateHighScore()
    {
        highScoreScriptable.bestTime = this.currentTimer;
        highScoreScriptable.questsDone = this.questsDone;
        highScoreScriptable.questExtractDone = this.questExtractDone;
        highScoreScriptable.questKillDone = this.questKillDone;
        highScoreScriptable.questPickupDone = this.questPickupDone;
    }


}
