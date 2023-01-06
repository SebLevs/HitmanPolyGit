using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : Manager<PickupManager>
{
    public List<Entity_Object_PickupQuest> pickableList = new List<Entity_Object_PickupQuest>();
}
