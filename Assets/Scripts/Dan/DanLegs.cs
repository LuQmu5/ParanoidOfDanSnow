using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DanLegs : MonoBehaviour
{
    [SerializeField] private float _baseSpeed = 2;
    [SerializeField] private NavMeshAgent _agent;

    private const float MaxSpeed = 4;

    public void TryMoveTo(Vector3 position, float speedMultiplier = 1)
    {
        float extraCoeff = 0.1f;
        float speed = _baseSpeed + (extraCoeff * speedMultiplier);

        if (speed > MaxSpeed)
            speed = MaxSpeed;

        _agent.speed = speed;

        if (_agent.destination == position)
            return;

        _agent.isStopped = true;
        _agent.SetDestination(position);
        _agent.isStopped = false;
    }
}
