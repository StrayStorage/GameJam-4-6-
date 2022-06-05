using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [Header("Live Variables")]
    public int currentResources;

    [Header("Game Properties")]
    public int totalResourceGiven = 24;
    public List<int> costOfTower;
    public int playerLife = 3;

    [Header("UI")]
    public TextMeshProUGUI tmp_Announcement;

    [Header("Reference")]
    public int totalEnemies;
    public float timeTillNextWave;
    


    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        currentResources = totalResourceGiven;
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("p"))
        {
            StartTheWaves();
        }
    }

    public void minusLife(int number)
    {
        playerLife -= 1;
        SelectTowerSpawn.Instance.LivesUpdate(playerLife);
    }

    public void minusEnemies(int number)
    {
        totalEnemies -= 1;
        SelectTowerSpawn.Instance.EnemiesUpdate(totalEnemies);
    }


    public void StartTheWaves()
    {
        SpawnScript.Instance.StartWaves();
        SelectTowerSpawn.Instance.EnemiesUpdate(totalEnemies);
    }
}
