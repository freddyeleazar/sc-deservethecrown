using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BuyAlcoholGelCinematicController : MonoBehaviour
{
    public List<Sprite> buyAlcoholGelSprites = new List<Sprite>();
    public Image buyAlcoholGelImage;
    public float secondsPerImage;
    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera nearCam;
    public CinemachineVirtualCamera farCam;
    public PlayerMovementController playerMovement;
    public Animator playerAnimator;
    public GameObject squadCar;
    public TriggerBuyAlcoholGel triggerBuyAlcoholGel;

    private void Awake()
    {
        buyAlcoholGelImage = GetComponent<Image>();
        playerMovement = FindObjectOfType<PlayerMovementController>();
        playerAnimator = playerMovement.gameObject.GetComponent<Animator>();
        triggerBuyAlcoholGel = FindObjectOfType<TriggerBuyAlcoholGel>();
        triggerBuyAlcoholGel.buyAlcoholGelCinematic = gameObject;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(PlayBuyAlcoholGelCinematic());
    }

    private IEnumerator PlayBuyAlcoholGelCinematic()
    {
        buyAlcoholGelImage.color = Color.white;
        foreach (Sprite buyAlcoholGelSprite in buyAlcoholGelSprites)
        {
            buyAlcoholGelImage.sprite = buyAlcoholGelSprite;
            yield return new WaitForSeconds(secondsPerImage);
        }
        farCam.enabled = true;
        nearCam.enabled = false;
        mainCam.enabled = false;
        yield return new WaitForSeconds(2);
        squadCar.SetActive(true);
        float previousFollowDistance = squadCar.GetComponent<EnemyController>().followDistance;
        squadCar.GetComponent<EnemyController>().followDistance = 0;
        buyAlcoholGelImage.color = new Color(0, 0, 0, 0);
        playerAnimator.SetBool("StopWalking", false);
        playerMovement.isStopped = false;

        yield return new WaitUntil(() => Vector3.Distance(playerAnimator.gameObject.transform.position, squadCar.transform.position) > 15);
        squadCar.GetComponent<EnemyController>().followDistance = previousFollowDistance;
        triggerBuyAlcoholGel.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
