using UnityEngine;

public abstract class Entity_Collider : Entity_Origin
{
    public Rigidbody Rigidbody { get; protected set; }
    public Collider Collider { get; protected set; }

    protected override void OnAwake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnCollisionEnterAct(collision);
    }
    private void OnCollisionStay(Collision collision)
    {
        OnCollisionStayAct(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        OnCollisionExitAct(collision);
    }

    private void OnTriggerEnter(Collider collision)
    {
        OnTriggerEnterAct(collision);
    }
    private void OnTriggerStay(Collider collision)
    {
        OnTriggerStayAct(collision);
    }

    private void OnTriggerExit(Collider collision)
    {
        OnTriggerExitAct(collision);
    }

    protected virtual void OnCollisionEnterAct(Collision collision) { }
    protected virtual void OnCollisionStayAct(Collision collision) { }
    protected virtual void OnCollisionExitAct(Collision collision) { }
    protected virtual void OnTriggerEnterAct(Collider collision) { }
    protected virtual void OnTriggerStayAct(Collider collision) { }
    protected virtual void OnTriggerExitAct(Collider collision) { }
}


