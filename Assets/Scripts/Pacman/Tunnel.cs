using UnityEngine;

public class Tunnel : MonoBehaviour
{
    [SerializeField] private Tunnel other;

    private void OnTriggerEnter2D(Collider2D entity)
    {
        if (other.transform.position.x > 0)
            entity.transform.position = other.transform.position - new Vector3(1f, 0f, 0f);
        else
            entity.transform.position = other.transform.position + new Vector3(1f, 0f, 0f);
    }
}
