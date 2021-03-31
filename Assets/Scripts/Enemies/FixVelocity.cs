using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FixVelocity : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent navMeshAgent;

    private void OnEnable()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerMovementController>().gameObject.transform;
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, player.position) < 50)
        {
            navMeshAgent.speed = 8;
        }
        else if(Vector3.Distance(transform.position, player.position) >= 50)
        {
            navMeshAgent.speed = 12;
        }
    }
}
