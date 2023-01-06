using UnityEngine;

public class LookAtPlayerAlways : MonoBehaviour
{
    private void Update()
    {
        LookAtPlayer();
    }

    private void LookAtPlayer()
    {
        Vector3 Position = new Vector3(Entity_Player.Instance.transform.position.x, transform.position.y, Entity_Player.Instance.transform.position.z);
        transform.LookAt(Position);
    }
}
