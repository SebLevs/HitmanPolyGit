using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHandler : MonoBehaviour
{
    [SerializeField] private float pickableDistance;

    private void FixedUpdate()
    {
        OnSeePickable();
    }

    /// <summary>
    /// Set lastPickable of player instance to allow for pickup
    /// </summary>
    private void OnSeePickable()
    {
        RaycastHit hit;
        Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out hit, pickableDistance);
        Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + gameObject.transform.forward * pickableDistance, Color.red);

        //Debug.Log(hit.transform.name);
        if (hit.transform != null)
        {
            Entity_Object_Pickable isPickable = hit.transform.GetComponent<Entity_Object_Pickable>();
            if (isPickable && isPickable != Entity_Player.Instance.Throwable && isPickable != Entity_Player.Instance.Weapon)
            {
                UIManager.Instance.View_Crosshair.OnHoverItemOn(); // TODO: Replace when implementing proper cue
                Entity_Player.Instance.LastSeenPickable = isPickable;
            }
            else
            {
                Entity_Player.Instance.LastSeenPickable = null;
                UIManager.Instance.View_Crosshair.OnHoverItemOut(); // TODO: Replace when implementing proper cue
            }
        }
        else
        {
            UIManager.Instance.View_Crosshair.OnHoverItemOut(); // TODO: Replace when implementing proper cue
        }
    }
}
