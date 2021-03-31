using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int currentStage = 1;

    public GameObject enemiesStage1Prefab;
    public GameObject enemiesStage2Prefab;
    public GameObject currentEnemiesStage1;
    public GameObject currentEnemiesStage2;

    public GameObject itemsStage1Prefab;
    public GameObject itemsStage2Prefab;
    public GameObject currentItemsStage1;
    public GameObject currentItemsStage2;

    public Transform player;
    public Transform stage2player;
    public TriggerBuyAlcoholGel triggerBuyAlcoholGel;
    public TriggerEnd triggerEnd;

    public GameObject squadCarPrefab;
    public GameObject squadCar;

    private void Start()
    {
        currentStage = 1;
        currentEnemiesStage1.SetActive(true);
        currentEnemiesStage2.SetActive(false);
        currentItemsStage1.SetActive(true);
        currentItemsStage2.SetActive(false);
        triggerBuyAlcoholGel.gameObject.SetActive(true);
    }

    public void LoadStage1()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadStage2()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        /*
        currentStage = 2;
        Destroy(currentEnemiesStage1);
        Destroy(currentItemsStage1);
        Instantiate(enemiesStage2Prefab);
        Instantiate(itemsStage2Prefab);
        player.position = stage2player.position;
        triggerBuyAlcoholGel.gameObject.SetActive(false);
        triggerEnd.gameObject.SetActive(true);
        squadCar.gameObject.SetActive(false);
        GameObject newSquadCar = Instantiate(squadCarPrefab);
        newSquadCar.SetActive(true);
        squadCar.GetComponent<EnemyController>().followDistance = 0;
        player.gameObject.GetComponent<Animator>().SetBool("StopWalking", false);
        player.gameObject.GetComponent<PlayerMovementController>().isStopped = false;
        */
    }
}
