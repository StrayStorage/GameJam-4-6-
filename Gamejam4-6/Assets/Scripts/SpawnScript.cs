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
    private int currentAmtOfEnemy;
    private IEnumerator coroutine;
    private float targetTime;
    private bool safeToCountdown;
    private bool[] areYouAlive;
    private int currNum;
    private float spawnTime;

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
        currentWaveNumber = 0;
        safeToCountdown = false;
        areYouAlive = new bool[1];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("p"))
        {
            StartWave(currentWaveNumber);
        }
        if(safeToCountdown)
        {
            targetTime -= Time.deltaTime;
            if (targetTime < 0)
            {
                StartWave(currentWaveNumber);
                safeToCountdown = false;
            }
        }
        
    }

    private void StartWave(int waveNumber)
    {
       
        currentWave = waveList[waveNumber];
        totalEnemy = currentWave.defenseSoldiersNum + currentWave.footSoldiersNum;
        areYouAlive = new bool[totalEnemy];
        print("startwave: " + areYouAlive.Length);
        currNum = 0;

        /*for (int i = 0; i < totalEnemy; i++ )
        {
            //Need to spawn foot soldiers before defense soldiers
            
            
        }*/

        coroutine = spawnStuff();
        StartCoroutine(coroutine);
        targetTime = currentWave.timeToNextWave;
        currentWaveNumber += 1;
    }

    IEnumerator spawnStuff()
    {
        print("I live");
        while ( currNum < totalEnemy )
        {
            spawn(footSoldier);
            yield return new WaitForSeconds(currentWave.timeBetweenSpawn);

        }
           
    }

    void spawn (GameObject enemy)
    {
        print("leggo");
        Instantiate(enemy, this.transform);
        enemy.GetComponent<EnemyScript>().assignQNum(currNum);
        areYouAlive[currNum] = true;
        currNum += 1;
    }

    public void imDead (int num)
    {
        areYouAlive[num] = false;

        print(num + " reporting as ded");
        //check if all are dead
        for (int i = 0; i < areYouAlive.Length; i ++)
        {
            if(areYouAlive[i])
            {
                break;
            }
            else if (areYouAlive[i] == false)
            {
                if(i == areYouAlive.Length - 1)
                {
                    print("all ded");
                    safeToCountdown = true;
                    StopCoroutine(coroutine);
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

