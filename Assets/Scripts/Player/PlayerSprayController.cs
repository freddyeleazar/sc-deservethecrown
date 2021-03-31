using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSprayController : MonoBehaviour
{
    private Animator playerAnimator;
    private ParticleSystem sprayGFX;
    private bool isSprayed = false;
    public List<GameObject> virusInImpactZone;
    public float maxSprayCharge = 3;
    public float sprayCharge = 3;
    public Slider sprayChargeUI;
    public GameObject lyfodor;
    public PlayerMovementController playerMovementController;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        sprayGFX = GetComponentInChildren<ParticleSystem>();
        sprayChargeUI = FindObjectOfType<Slider>();
        sprayCharge = maxSprayCharge;
        sprayChargeUI.value = sprayCharge / maxSprayCharge;
        lyfodor = GetComponentsInChildren<Transform>().ToList().Find(t => t.gameObject.name == "Lyfodor").gameObject;
        lyfodor.SetActive(false);
        playerMovementController = FindObjectOfType<PlayerMovementController>();
    }

    private void Update()
    {
        if (!playerMovementController.isStopped)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (sprayCharge > 0)
                {
                    playerAnimator.SetTrigger("Shoot");
                    StartCoroutine(PlaySprayGFX());
                }
                else
                {
                    //Mostar aviso de que no hay carga de spray
                }
            }
            if (isSprayed)
            {
                sprayCharge -= 1 * Time.deltaTime;
                sprayChargeUI.value = sprayCharge / maxSprayCharge;
                ClearSprayedZone();
            }
        }
    }

    private void ClearSprayedZone()
    {
        foreach (GameObject virus in virusInImpactZone)
        {
            virus.SetActive(false);
        }
        virusInImpactZone.Clear();
    }

    private IEnumerator PlaySprayGFX()
    {
        sprayGFX.Play();
        lyfodor.SetActive(true);
        yield return new WaitForSeconds(1);
        isSprayed = true;
        lyfodor.SetActive(false);
        yield return new WaitForSeconds(1);
        sprayGFX.Stop();
        isSprayed = false;
    }
}