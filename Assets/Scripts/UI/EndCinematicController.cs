using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndCinematicController : MonoBehaviour
{
    public List<Sprite> endSprites = new List<Sprite>();
    public Image endImage;
    public float secondsPerImage;
    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera nearCam;
    public CinemachineVirtualCamera farCam;
    public PlayerMovementController playerMovement;
    public Animator playerAnimator;
    public TriggerEnd triggerEnd;
    public GameObject playAgain;
    public GameObject exit;

    private void Awake()
    {
        endImage = GetComponent<Image>();
        playerMovement = FindObjectOfType<PlayerMovementController>();
        playerAnimator = playerMovement.gameObject.GetComponent<Animator>();
        triggerEnd = FindObjectOfType<TriggerEnd>();
        triggerEnd.endCinematic = gameObject;
    }

    private void Start()
    {
        CameraController cameraController = FindObjectOfType<CameraController>();
        mainCam = cameraController.cameras.Find(t => t.name == "CM vcam1");
        nearCam = cameraController.cameras.Find(t => t.name == "CM vcam2");
        farCam = cameraController.cameras.Find(t => t.name == "CM vcam3");
        playAgain = GetComponentInChildren<PressTryAgain>().gameObject;
        playAgain.SetActive(false);
        exit = GetComponentInChildren<PressExit>().gameObject;
        exit.SetActive(false);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(PlayBuyAlcoholGelCinematic());
    }

    private IEnumerator PlayBuyAlcoholGelCinematic()
    {
        endImage.color = Color.white;
        foreach (Sprite endSprite in endSprites)
        {
            endImage.sprite = endSprite;
            if(endSprites.IndexOf(endSprite) == endSprites.Count - 1)
            {
                playAgain.SetActive(true);
                exit.SetActive(true);
            }
            yield return new WaitForSeconds(secondsPerImage);
        }
        //farCam.enabled = true;
        //nearCam.enabled = false;
        //mainCam.enabled = false;
        //yield return new WaitForSeconds(2);
        //endImage.color = new Color(0, 0, 0, 0);
        //playerAnimator.SetBool("StopWalking", false);
        //playerMovement.isStopped = false;
        triggerEnd.gameObject.SetActive(false);
    }
}
