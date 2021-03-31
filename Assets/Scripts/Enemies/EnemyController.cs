using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float visualRange = 20;
    public float followDistance = 30;
    private Transform player;
    private NavMeshAgent navMeshAgent;
    private PlayerMovementController playerMovement;

    public bool isPlayerFar;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovementController>().transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerMovement = player.gameObject.GetComponent<PlayerMovementController>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if(distance < followDistance)
        {
            if (navMeshAgent.isActiveAndEnabled)
            {
                navMeshAgent.SetDestination(player.position);
            }
        }

        if(distance > 5)
        {
            isPlayerFar = true;
        }

        if (!isPlayerFar)
        {
            transform.LookAt(player);
        }

        if (playerMovement.isStopped)
        {
            if (navMeshAgent.isActiveAndEnabled)
            {
                navMeshAgent.isStopped = true;
            }
        }
        else
        {
            navMeshAgent.isStopped = false;
        }
    }
}
