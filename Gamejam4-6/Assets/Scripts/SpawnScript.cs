using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpawnScript : MonoBehaviour
{
    public static SpawnScript Instance;

    public GameObject footSoldier;
    public GameObject defenseSoldier;
    public List<WaveData> waveList = new List<WaveData>();

    private int currentWaveNumber;
    private WaveData currentWave;

    private int totalEnemy;
    private int currTotalEnemy;
    private bool[] areYouAlive;
    private int currNum;
    private int numOfFootSoldier;
    private int numOfDefenseSoldier;

    private float targetTime;
    private float spawnTime;
    private bool safeToCountdown;
    private bool spawnCountdown;
    
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


        // Start is called before the first frame update
        void Start()
    {
        ResetWaves();
    }

    // Update is called once per frame
    void Update()
    {
        if(safeToCountdown)
        {
            targetTime -= Time.deltaTime;
            GameController.Instance.timeTillNextWave = targetTime;
            GameController.Instance.UpdateTheTimer(targetTime.ToString(".0"));
            if (targetTime < 0)
            {
                StartWave(currentWaveNumber);
                safeToCountdown = false;
            }
        }
        if (spawnCountdown && currNum < totalEnemy)
        {
            spawnTime -= Time.deltaTime;
            if (spawnTime < 0)
            {
                spawnCountdown = false;
                int rnd = Random.Range(0, 2);
                if (rnd == 0)
                {
                    if (numOfFootSoldier != 0)
                    {
                        numOfFootSoldier -= 1;
                        spawn(footSoldier);
                    }
                    else
                    {
                        numOfDefenseSoldier -= 1;
                        spawn(defenseSoldier);
                    }
                }
                else
                {
                    if (numOfDefenseSoldier != 0)
                    {
                        numOfDefenseSoldier -= 1;
                        spawn(defenseSoldier);
                    }
                    else
                    {
                        numOfFootSoldier -= 1;
                        spawn(footSoldier);
                    }
                }
                
            }
        }
        if(Input.GetKeyUp("s"))
        {
            ResetWaves();
        }
        
    }

    public void StartWaves()
    {
        StartWave(currentWaveNumber);

        GameController.Instance.UpdateTheEnemies();
    }

    void ResetWaves()
    {
        currentWaveNumber = 0;
        safeToCountdown = false;
        areYouAlive = new bool[1];
        spawnCountdown = false;
    }

    private void StartWave(int waveNumber)
    {

        GameController.Instance.UpdateTheTimer("Ongoing!");
        currentWave = waveList[waveNumber];
        numOfFootSoldier = currentWave.footSoldiersNum;
        numOfDefenseSoldier = currentWave.defenseSoldiersNum;
        totalEnemy = numOfFootSoldier + numOfDefenseSoldier;
        currTotalEnemy = totalEnemy;
        GameController.Instance.totalEnemies = totalEnemy;
        areYouAlive = new bool[totalEnemy];
        currNum = 0;
        spawn(footSoldier);
        numOfFootSoldier -= 1;
        GameController.Instance.UpdateTheEnemies();
    }

    void spawn (GameObject enemy)
    {
        GameObject thisEnemy = Instantiate(enemy, this.transform);
        thisEnemy.GetComponent<EnemyScript>().queueNum = currNum;
        thisEnemy.GetComponent<EnemyScript>().theChild.GetComponent<Animator>().SetBool("Spawn", true);
        areYouAlive[currNum] = true;
        currNum += 1;
        spawnTime = currentWave.timeBetweenSpawn;
        spawnCountdown = true;
    }

    public void imDead (int num)
    {
        areYouAlive[num] = false;
        GameController.Instance.minusEnemies(1);
        currTotalEnemy -= 1;
        //check if all are dead
        for (int i = 0; i < areYouAlive.Length; i ++)
        {
            if(areYouAlive[i])
            {
                break;
            }
            else if (areYouAlive[i] == false)
            {
                if(i == areYouAlive.Length - 1 && currentWaveNumber < waveList.Count - 1 && currTotalEnemy == 0)
                {
                    safeToCountdown = true;
                    targetTime = currentWave.timeToNextWave;
                    currentWaveNumber += 1;
                }
                else if (i == areYouAlive.Length - 1 && currentWaveNumber == waveList.Count - 1 && currTotalEnemy == 0)
                {
                    GameController.Instance.YouWin();
                }
            }
        }

    }

    [System.Serializable]
    public class WaveData
    {
        public int footSoldiersNum;
        public int defenseSoldiersNum;
        public float timeBetweenSpawn;
        public float timeToNextWave;
    }
   

}

