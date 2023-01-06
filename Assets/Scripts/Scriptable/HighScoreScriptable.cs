using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HighScore", menuName = "ScriptableObjects/HighScoreSciptable")]
public class HighScoreScriptable : ScriptableObject
{
    public float bestTime;
    public int questsDone;
    public bool questExtractDone;
    public bool questKillDone;
    public bool questPickupDone;
}
