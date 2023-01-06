using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Entity_Object_Throwable : Entity_Object_Pickable
{
    [SerializeField] private float velocityMod = 20f;
    public LayerMask EntityTarget = 1 << 6;
    public bool CanDistract { get; private set; }
    

    [SerializeField][Range(5,25)]
    private float DistractRadius = 10;
    public float DistractRadiusSet { get => DistractRadius; set => DistractRadius = value; }
    public override void OnPickup()
    {
        if (Entity_Player.Instance.Throwable)
        {
            Entity_Player.Instance.Throwable.OnDrop();
        }

        Entity_Player.Instance.Throwable = this;
        PickupHelper(Entity_Player.Instance.SocketThrowable);
    }

    public override void OnAttack(Transform _direction = null)
    {
        OnDrop();
        // TODO: Might need change into a special camera for gun to avoid phasing in other objects
        this.Rigidbody.AddForce(Entity_Player.Instance.Camera.transform.forward * velocityMod, ForceMode.Impulse);
        CanDistract = true;
    }
    
    private void StartDetectEnemies()
    {
        if (CanDistract)
        {
            if(Rigidbody.velocity.magnitude <= 0.1f)
            {
                CanDistract = false;
            }
            Collider[] _hitCollider = Physics.OverlapSphere(transform.position, 20, EntityTarget);
            if (_hitCollider.Length > 0)
            {
                foreach (var coll in _hitCollider)
                {
                    Entity_Enemy _enemy = coll.gameObject.GetComponent<Entity_Enemy>();
                    if (_enemy)
                    {
                        _enemy.SetDistract(true, transform.position);
                    }
                }
            }
            
        }
    }

    protected override void OnCollisionEnterAct(Collision collision)
    {
        StartDetectEnemies();
    }

    protected override void OnStart()
    {
        CanDistract = false;
    }
}
