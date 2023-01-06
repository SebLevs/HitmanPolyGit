using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpotLight : Entity_Collider
{
    public Vector3 startRotation;
    public Vector3 endRotation;
    private bool comingBack = false;
    public AnimationCurve moveCurve;
    public float moveDuration = 5f;
    private float moveStopWatch = 0;


    protected override void OnUpdate()
    {
        base.OnUpdate();
        moveStopWatch += Time.deltaTime;

        if (moveStopWatch >= moveDuration && !comingBack)
        {
            comingBack = true;
            moveStopWatch = 0;
            transform.rotation = Quaternion.Euler(endRotation);
        }
        if (moveStopWatch >= moveDuration && comingBack)
        {
            comingBack = false;
            moveStopWatch = 0;
            transform.rotation = Quaternion.Euler(startRotation);
        }
        if (!comingBack)
        {
            transform.rotation = Quaternion.Euler(Vector3.Slerp(startRotation, endRotation, moveCurve.Evaluate(moveStopWatch / moveDuration)));
        }
        else
        {
            transform.rotation = Quaternion.Euler(Vector3.Slerp(endRotation, startRotation, moveCurve.Evaluate(moveStopWatch / moveDuration)));
        }
    }
    protected override void OnTriggerEnterAct(Collider collision)
    {
        base.OnTriggerEnterAct(collision);
        if(collision.GetComponent<Entity_Player>()) 
        {
            EnemyManager.Instance.SetAlert();
        }
    }

}

