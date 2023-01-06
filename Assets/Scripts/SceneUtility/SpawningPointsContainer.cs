using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawningPointsContainer : MonoBehaviour
{
    [SerializeField] private Transform[] spawningPoints;

    public Transform[] SpawningPoints { get => spawningPoints; }

    private void Awake()
    {
    }

    private void Start()
    {
        Subscribe();
    }

    public abstract void Subscribe();

    public Transform GetRandomPoint()
    {
        int index = Random.Range(0, spawningPoints.Length);
        return spawningPoints[index];
    }

    public void ClearSpawnPoints()
    {
        spawningPoints = null;
    }
}
