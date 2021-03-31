using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriggerEnd : MonoBehaviour
{
    public Animator playerAnimator;
    public PlayerMovementController playerMovement;
    public GameObject endCinematic;
    public CinemachineVirtualCamera nearCam;
    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera farCam;
    public GameObject enemiesS1;
    public GameObject enemiesS2;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovementController>();
        playerAnimator = playerMovement.gameObject.GetComponent<Animator>();
        mainCam = FindObjectsOfType<CinemachineVirtualCamera>().ToList().Find(t => t.gameObject.name == "CM vcam1");
        nearCam = FindObjectsOfType<CinemachineVirtualCamera>().ToList().Find(t => t.gameObject.name == "CM vcam2");
        farCam = FindObjectsOfType<CinemachineVirtualCamera>().ToList().Find(t => t.gameObject.name == "CM vcam3");
        this.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (enabled)
        {
            if (other.gameObject.name == "Player")
            {
                StartCoroutine(PlayEndCinematic());
            }
        }
    }

    private IEnumerator PlayEndCinematic()
    {
        playerMovement.isStopped = true;
        playerAnimator.SetBool("StopWalking", true);
        nearCam.enabled = true;
        farCam.enabled = false;
        mainCam.enabled = false;
        yield return new WaitUntil(() => playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Wiping Sweat"));
        float lastAnimationDuration = playerAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(lastAnimationDuration);
        endCinematic.SetActive(true);
        enemiesS1.SetActive(false);
        enemiesS2.SetActive(false);
    }
}
