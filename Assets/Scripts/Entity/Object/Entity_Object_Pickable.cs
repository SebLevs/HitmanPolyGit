using UnityEngine;

public abstract class Entity_Object_Pickable : Entity_Object_Physical
{
    public virtual void OnPickup() { transform.SetPositionAndRotation(Vector3.zero, Quaternion.Euler(0, 0, 0)); }

    public abstract void OnAttack(Transform _direction = null);

    public virtual void OnDrop()
    {
        transform.parent = GameObject.Find("Lost Pickables").transform;
        Rigidbody.isKinematic = false;
        Collider.isTrigger = false;

        if (this is Entity_Object_Throwable)
        {
            Entity_Player.Instance.Throwable = null;
        }
        else if (this is Entity_Object_Weapon)
        {
            Entity_Player.Instance.Weapon = null;
        }
    }

    public void PickupHelper(Transform socket)
    {
        transform.position = socket.position;
        transform.rotation = socket.rotation;
        transform.parent = socket;
        Rigidbody.isKinematic = true;
        Collider.isTrigger = true;
    }
}
