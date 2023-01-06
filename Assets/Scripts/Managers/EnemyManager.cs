using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : Manager<EnemyManager>
{
    public List<Entity_Enemy> EnemyList;

    public List<Mesh> ListOfMeshSkin; //for civilian and target

    public List<Mesh> GuardSkin; // for patroller and guard

    //player stats
    public int killCount = 0;
    public bool canKillOther = false;
    private int m_TargetIndex;
    public bool IsAlertActivated { get; private set; }
    //target
    public GameObject TargetToKill { get; private set; }
    public Mesh TargetMeshSkin { get; private set; }

    protected override void OnAwake()
    {
        EnemyList = new List<Entity_Enemy>();
        IsAlertActivated = false;
    }

    protected override void OnStart()
    {
        //SetGameTarget();
        //SetEnemyListSkin();
    }
    /// <summary>
    /// when alert every civilizian will update for patroller
    /// </summary>
    public void SetAlert()
    {
        if (Entity_Player.Instance.IsDead) { return; }
        if (IsAlertActivated)
        {
            return;
        }
        IsAlertActivated = true;

        // Set UI view for alert
        UIManager.Instance.View_Alert.OnShow();

        foreach (var enemy in EnemyList)
        {
            if(enemy.Type == EnemyType.CIVILIAN && !enemy.IsDead)//maybe annd a check for not dead
            {
                var _skinComponent = enemy.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
                enemy.Type = EnemyType.PATROLLER;
                _skinComponent.sharedMesh = GuardSkin[RandomIndex(0, GuardSkin.Count)];
                _skinComponent.rootBone = enemy.Root;
                enemy.WeaponSocket.gameObject.SetActive(true);
                enemy.FlashLight.SetActive(true);
            }
        }
    }

    public void DeactivateAlert()
    {
        IsAlertActivated = false;
    }

    /// <summary>
    /// This is fo initialise enemy skin
    /// </summary>
    public void InitEnemySkin()
    {
        if (EnemyList.Count == 0)
        {
            return;
        }
        foreach(var enemy in EnemyList)
        {

            var _skinComponent = enemy.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
            int _randonSkinIndex;
            if(enemy.Type == EnemyType.NULL)
            {
                print("YOUR ENEMY TYPE IS NULL CANT SET ANYSKIN TO IT");
                return;
            }
            if (enemy.Type == EnemyType.TARGET)
            {
                enemy.FlashLight.SetActive(false);
                _randonSkinIndex = RandomIndex(0, ListOfMeshSkin.Count);
                _skinComponent.sharedMesh = ListOfMeshSkin[_randonSkinIndex];
                TargetMeshSkin = ListOfMeshSkin[_randonSkinIndex];
                TargetToKill = enemy.gameObject;
                //ListOfMeshSkin.Remove(TargetMeshSkin);
                ListOfMeshSkin.RemoveAt(_randonSkinIndex);
            }
            else if(enemy.Type == EnemyType.CIVILIAN)
            {
                enemy.FlashLight.SetActive(false);
                _randonSkinIndex = RandomIndex(0, ListOfMeshSkin.Count);
                _skinComponent.sharedMesh = ListOfMeshSkin[_randonSkinIndex];
                enemy.WeaponSocket.gameObject.SetActive(false);

            }
            else if(enemy.Type == EnemyType.GUARD || enemy.Type == EnemyType.PATROLLER)
            {
                enemy.FlashLight.SetActive(true);
                _randonSkinIndex = RandomIndex(0, GuardSkin.Count);
                _skinComponent.sharedMesh = GuardSkin[_randonSkinIndex];
            }
            _skinComponent.rootBone = enemy.Root;
        }

        ListOfMeshSkin.Add(TargetMeshSkin);
    }

    private int RandomIndex(int _min, int _max)
    {
        return Random.Range(_min, _max);
    }

    public void SetEnemyListSkin()
    {
        foreach (var enemy in EnemyList)
        {
            if(enemy.gameObject != TargetToKill)
            {
                var _skinComponent = enemy.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
                var _randonSkinIndex = RandomIndex(0, ListOfMeshSkin.Count);

                _skinComponent.sharedMesh = ListOfMeshSkin[_randonSkinIndex];
                _skinComponent.rootBone = enemy.Root;
            }
           
        }
        ListOfMeshSkin.Add(TargetMeshSkin);
        //UIManager.Instance.View_Target.SetTargetMeshOnUI();
    }
    public void Subscribe(Entity_Enemy _entity)
    {
        EnemyList.Add(_entity);
    }
    public void SetGameTarget()
    {
        if (EnemyList.Count == 0)
        {
            return;
        }

        //get unique skin
        int _skinIndex = RandomIndex(0, ListOfMeshSkin.Count );
        TargetMeshSkin = ListOfMeshSkin[_skinIndex];
        ListOfMeshSkin.Remove(TargetMeshSkin);
        //get random enemy
        int _targetIndex = RandomIndex(0, EnemyList.Count);
        TargetToKill = EnemyList[_targetIndex].gameObject;

        //set skin visual and root to the target
        var _skinTarget = TargetToKill.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        _skinTarget.sharedMesh = TargetMeshSkin;
        _skinTarget.rootBone = TargetToKill.GetComponent<Entity_Enemy>().Root;
        print("TARGET IS " + _targetIndex);
    }
    
    public bool IsTargetDead()
    {
       return TargetToKill.GetComponent<Entity_Enemy>().IsDead;
    }

    public void ResetManager() 
    {
        killCount = 0;
        canKillOther = false;
        IsAlertActivated = false;
    }
}
