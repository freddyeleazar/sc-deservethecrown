using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class VirusInteractionController : MonoBehaviour
{
    public GameObject mask;
    public GameObject maskUI;
    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera nearCam;
    public CinemachineVirtualCamera farCam;
    public CellPhoneCinematicController cellPhoneCinematic;

    private Animator playerAnimator;
    private PlayerMovementController playerMovement;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovementController>();
        cellPhoneCinematic = FindObjectOfType<CellPhoneCinematicController>();
        mainCam = FindObjectsOfType<CinemachineVirtualCamera>().ToList().Find(t => t.gameObject.name == "CM vcam1");
        nearCam = FindObjectsOfType<CinemachineVirtualCamera>().ToList().Find(t => t.gameObject.name == "CM vcam2");
        farCam = FindObjectsOfType<CinemachineVirtualCamera>().ToList().Find(t => t.gameObject.name == "CM vcam3");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsVirus(collision))
        {
            if (HasMask())
            {
                UseMask(collision.transform);
            }
            else
            {
                GetSick(collision.gameObject.transform);
            }
        }
    }

    private bool IsVirus(Collision collision)
    {
        bool isVirus = false;
        if(collision.gameObject.layer == 11)
        {
            isVirus = true;
        }
        return isVirus;
    }

    private bool HasMask()
    {
        bool hasMask = false;
        if (mask != null)
        {
            hasMask = true;
        }
        return hasMask;
    }

    private void UseMask(Transform virus)
    {
        virus.gameObject.SetActive(false);
        mask = null;
        if (maskUI != null)
        {
            maskUI.SetActive(false);
        }
        else
        {
            Debug.Log("Debe asignarse una UI para el icono de la mascarilla");
        }
    }

    public void GetSick(Transform virus)
    {
        virus.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
        playerMovement.isStopped = true;
        playerMovement.Stop();
        playerMovement.PushBack(virus);
        playerAnimator.SetBool("IsStopped", true);
        StartCoroutine(StartCinematic());
    }

    private IEnumerator StartCinematic()
    {
        yield return new WaitUntil(() => playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Using Cell Phone"));
        mainCam.enabled = false;
        farCam.enabled = false;
        nearCam.enabled = true;
        StartCoroutine(cellPhoneCinematic.PlayCinematic(EndType.GetSick));
    }
}
