using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthorizationInteractionController : MonoBehaviour
{
    public GameObject authorizationUI;

    private CopInteractionController copInteraction;

    private void Start()
    {
        copInteraction = GetComponent<CopInteractionController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsAuthorization(other))
        {
            GameObject newAuthorization = other.gameObject;
            if (HasAuthorization())//Si el player ya tiene permiso para salir
            {
                //¿Acumular permisos?
            }
            else
            {
                TakeAuthorization(newAuthorization);
            }
        }
    }

    private bool IsAuthorization(Collider collider)
    {
        bool isAuthorization = false;
        if(collider.gameObject != null)
        {
            if(collider.gameObject.layer == 10)
            {
                isAuthorization = true;
            }
        }
        return isAuthorization;
    }

    private bool HasAuthorization()
    {
        bool hasAuthorization = false;
        if (copInteraction.authorization != null)
        {
            hasAuthorization = true;
        }
        return hasAuthorization;
    }

    private void TakeAuthorization(GameObject newAuthorization)
    {
        copInteraction.authorization = newAuthorization;
        newAuthorization.SetActive(false);
        if(authorizationUI != null)
        {
            authorizationUI.SetActive(true);
        }
        else
        {
            Debug.Log("Debe asignarse una UI para el icono del permiso");
        }
    }
}
