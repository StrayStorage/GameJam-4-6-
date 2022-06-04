using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [Header("Live Variables")]
    public int currentResources;

    [Header("Game Properties")]
    public int totalResourceGiven = 24;

    public List<int> costOfTower;

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
        
    }
}