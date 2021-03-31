using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriggerBuyAlcoholGel : MonoBehaviour
{
    public Animator playerAnimator;
    public PlayerMovementController playerMovement;
    public GameObject buyAlcoholGelCinematic;
    public CinemachineVirtualCamera nearCam;
    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera farCam;
    public GameObject enemiesS1;
    public GameObject enemiesS2;
    public TriggerEnd triggerEnd;
    public GameController gameController;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovementController>();
        playerAnimator = playerMovement.gameObject.GetComponent<Animator>();
        mainCam = FindObjectsOfType<CinemachineVirtualCamera>().ToList().Find(t => t.gameObject.name == "CM vcam1");
        nearCam = FindObjectsOfType<CinemachineVirtualCamera>().ToList().Find(t => t.gameObject.name == "CM vcam2");
        farCam = FindObjectsOfType<CinemachineVirtualCamera>().ToList().Find(t => t.gameObject.name == "CM vcam3");
        triggerEnd = FindObjectOfType<TriggerEnd>();
        gameController = FindObjectOfType<GameController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player" || other.gameObject.name == "Player (1)")
        {
            StartCoroutine(PlayBuyAlcoholGelCinematic());
        }
    }

    private IEnumerator PlayBuyAlcoholGelCinematic()
    {
        playerMovement.isStopped = true;
        playerAnimator.SetBool("StopWalking", true);
        nearCam.enabled = true;
        farCam.enabled = false;
        mainCam.enabled = false;
        yield return new WaitUntil(() => playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Wiping Sweat"));
        float lastAnimationDuration = playerAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(lastAnimationDuration);
        buyAlcoholGelCinematic.SetActive(true);
        enemiesS1.SetActive(false);
        enemiesS2.SetActive(true);
        triggerEnd.enabled = true;
        gameObject.SetActive(false);
        gameController.currentStage = 2;
    }
}
