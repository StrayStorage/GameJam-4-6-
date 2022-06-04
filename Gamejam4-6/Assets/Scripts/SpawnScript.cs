using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpawnScript : MonoBehaviour
{
    
    public int waves;
    public List<WaveData> waveList = new List<WaveData>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

