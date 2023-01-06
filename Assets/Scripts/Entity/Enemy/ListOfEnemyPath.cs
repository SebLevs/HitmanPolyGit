using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[Serializable]
//public class PatrollingPoint
//{
//    public Transform[] Point;
//    public Transform FirstPoint;
//    public Transform LastPoint;
//    public float WaitTime;
//    public task[] from task manager
//}
public class ListOfEnemyPath : Entity_Origin
{
    [SerializeField]
    private List<Transform> m_listOfWayPoint;
    private int m_currentPoint;
    private Vector3 currentWayPoint;

    public Vector3 GetNextWayPoint()
    {
        if(m_listOfWayPoint.Count == 1)
        {
            return m_listOfWayPoint[0].position;
        }
        m_currentPoint++;
        if (m_currentPoint > m_listOfWayPoint.Count -1)
        {
            m_currentPoint = 0;
        }
        currentWayPoint = m_listOfWayPoint[m_currentPoint].position;
        return m_listOfWayPoint[m_currentPoint].position;
    }

    public Vector3 SetGuardPosition()
    {
        return m_listOfWayPoint[0].position;
    }
    public Vector3 GetRandomNextWayPoint()
    {
        var _index = UnityEngine.Random.Range(0,m_listOfWayPoint.Count -1);
        while (m_listOfWayPoint[_index].position == currentWayPoint)
        {
            _index = UnityEngine.Random.Range(0, m_listOfWayPoint.Count - 1);
        }
        currentWayPoint = m_listOfWayPoint[_index].position;
        return m_listOfWayPoint[_index].position;
    }
    protected override void Init()
    {
    }

    protected override void OnAwake()
    {
        m_currentPoint = -1;

    }

    protected override void OnFixedUpdate()
    {
    }

    protected override void OnStart()
    {
    }
}
