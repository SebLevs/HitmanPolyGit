using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Object_PickupQuest : Entity_Object_Pickable
{
    public string itemName;
    public string locationName;
    private MeshRenderer lightMesh;

    public MeshRenderer LightMesh { get => lightMesh; set => lightMesh = value; }

    protected override void OnStart()
    {
        LightMesh = this.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        LightMesh.enabled = false;
        PickupManager.Instance.pickableList.Add(this);
    }

    public override void OnAttack(Transform _direction = null)
    {
        
    }

    public override void OnPickup()
    {
        QuestManager.Instance.currentPickupQuest.test = this;
        QuestManager.Instance.currentPickupQuest.OnCompleteQuest();
        Destroy(this.gameObject);
    }
}
