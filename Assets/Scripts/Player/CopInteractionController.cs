using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class CopInteractionController : MonoBehaviour
{
    public GameObject authorization;
    public GameObject authorizationUI;
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
        if(IsCop(collision))
        {
            if (HasAuthorization())
            {
                UseAuthorization(collision.transform);
            }
            else
            {
                GetArrested(collision.collider.gameObject.transform);
            }
        }
    }

    private bool IsCop(Collision collision)
    {
        bool isCop = false;
        if (collision.gameObject != null)
        {
            if (collision.collider.gameObject.layer == 9)
            {
                isCop = true;
            }
        }
        return isCop;
    }

    private bool HasAuthorization()
    {
        bool hasAuthorization = false;
        if (authorization != null)
        {
            hasAuthorization = true;
        }
        return hasAuthorization;
    }

    private void UseAuthorization(Transform policeman)
    {
        policeman.gameObject.SetActive(false);
        authorization = null;
        if(authorizationUI != null)
        {
            authorizationUI.SetActive(false);
        }
        else
        {
            Debug.Log("Debe asignarse una UI para el icono del permiso");
        }
    }

    public void GetArrested(Transform cop)
    {
        playerMovement.isStopped = true;
        playerMovement.Stop();
        playerMovement.PushBack(cop);
        playerAnimator.SetBool("IsStopped", true);
        StartCoroutine(StartCinematic());
    }

    private IEnumerator StartCinematic()
    {
        yield return new WaitUntil(() => playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Using Cell Phone"));
        mainCam.gameObject.SetActive(false);
        farCam.gameObject.SetActive(false);
        nearCam.gameObject.SetActive(true);
        StartCoroutine(cellPhoneCinematic.PlayCinematic(EndType.GetArrested));
    }
}
