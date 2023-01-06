using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPC_Player : SpawningPointsContainer
{
    public override void Subscribe()
    {
        SceneLoadManager.Instance.SceneStartingPoint = this;
    }
}
