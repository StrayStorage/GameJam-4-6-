using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointSystem : MonoBehaviour
{
    public static WaypointSystem Instance { get; private set; }
    public List<GameObject> theWaypoints;

    public void Awake()
    {
        if (Instance != null && Instance != this )
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        foreach (Transform child in transform)
        {
            theWaypoints.Add(child.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
