using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : Manager<QuestManager>
{
    public BaseQuest mainQuest = null;
    public ExtractQuest currentExtractQuest = null;
    public KillQuest currentKillQuest = null;
    public PickupQuest currentPickupQuest = null;

    public void CreateAllQuests()
    {
        CreateMainQuest();
        CreateExtractQuest();
        CreateKillQuest();
        CreatePickupQuest();
    }

    public void CreateMainQuest()
    {
        BaseQuest mainquest = new BaseQuest
            (
            "Main Quest",
            "Kill the target and extract from the city"
            );
        mainQuest = mainquest;
    }

    public void CreateExtractQuest()
    {
        ExtractPoints randompoint = ExtractManager.Instance.activeExtractPoints[Random.Range(0, ExtractManager.Instance.activeExtractPoints.Count)];

        ExtractQuest newQuest = new ExtractQuest
        (
        "Extract at the " + randompoint.ExtractName + " extraction point",
        randompoint.ExtractName
        );
        Debug.Log("--------------------" + randompoint.ExtractName);
        this.currentExtractQuest = newQuest;
    }
    public void CreateKillQuest()
    {
        Entity_Object_Weapon randomWeapon = WeaponManager.Instance.weaponList[Random.Range(0, WeaponManager.Instance.weaponList.Count)];
        KillQuest newQuest = new KillQuest
            (
            "Kill the target with a " + randomWeapon.weaponName,
            randomWeapon
            );
        Debug.Log("-------------------" + randomWeapon.weaponName);
        this.currentKillQuest = newQuest;
    }
    public void CreatePickupQuest()
    {
        Entity_Object_PickupQuest randomPickable = PickupManager.Instance.pickableList[Random.Range(0, PickupManager.Instance.pickableList.Count)];
        print("---------------------------------------" + PickupManager.Instance.pickableList.Count + "------------------------------------");
        randomPickable.LightMesh.enabled = true;
        PickupQuest newQuest = new PickupQuest
            (
            "Pick up the " + randomPickable.itemName + " from the " + randomPickable.locationName,
            randomPickable.itemName,
            randomPickable.locationName
            );
        Debug.Log("-------------------" + randomPickable.itemName);
        this.currentPickupQuest = newQuest;
    }
}
