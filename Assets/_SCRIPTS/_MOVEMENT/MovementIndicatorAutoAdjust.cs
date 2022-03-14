using UnityEngine;
using UnityEngine.AI;

public class MovementIndicatorAutoAdjust : MonoBehaviour
{
    private void Awake()
    {
        Vector3 pos = transform.position;
        float maxDistance = 10f;
        if (NavMesh.SamplePosition(pos, out NavMeshHit hit, maxDistance, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
    }
}
