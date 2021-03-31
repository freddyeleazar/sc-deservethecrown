using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskInteractionController : MonoBehaviour
{
    public GameObject maskUI;

    private VirusInteractionController virusInteraction;

    private void Start()
    {
        virusInteraction = GetComponent<VirusInteractionController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsMask(other))
        {
            GameObject newMask = other.gameObject;
            if (HasMask())
            {
                //¿Acumular máscaras?
            }
            else
            {
                TakeMask(newMask);
            }
        }
    }

    private void TakeMask(GameObject newMask)
    {
        virusInteraction.mask = newMask;
        newMask.SetActive(false);
        if(maskUI != null)
        {
            maskUI.SetActive(true);
        }
        else
        {
            Debug.Log("Debe asignarse una UI para el icono de la mascarilla");
        }
    }

    private bool HasMask()
    {
        bool hasMask = false;
        if(virusInteraction.mask != null)
        {
            hasMask = true;
        }
        return hasMask;
    }

    private bool IsMask(Collider collider)
    {
        bool isMask = false;
        if(collider.gameObject.layer == 12)
        {
            isMask = true;
        }
        return isMask;
    }
}
