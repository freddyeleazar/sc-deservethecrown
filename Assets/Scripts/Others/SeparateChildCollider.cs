using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparateChildCollider : MonoBehaviour
{
    public PlayerSprayController playerSprayController;

    private void Start()
    {
        playerSprayController = GetComponentInParent<PlayerSprayController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsVirus(other))
        {
            playerSprayController.virusInImpactZone.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsVirus(other))
        {
            if (playerSprayController.virusInImpactZone.Contains(other.gameObject))
            {
                playerSprayController.virusInImpactZone.Remove(other.gameObject);
            }
        }
    }

    private bool IsVirus(Collider collider)
    {
        bool isVirus = false;
        if (collider.gameObject.layer == 11)
        {
            isVirus = true;
        }
        return isVirus;
    }
}
