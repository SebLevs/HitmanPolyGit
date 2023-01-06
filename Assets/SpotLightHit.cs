using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightHit : Entity_Collider
{
    private GameObject spotlight;
    private GameObject lightSphere;

    protected override void OnStart()
    {
        base.OnStart();
        spotlight = GetComponentInChildren<Light>().gameObject;
        lightSphere = GetComponentInChildren<SphereCollider>().gameObject;
    }
    
    public void BreakLight()
    {
        spotlight.SetActive(false);
        lightSphere.SetActive(false);
        GetComponent<MoveSpotLight>().enabled = false;
    }


}
