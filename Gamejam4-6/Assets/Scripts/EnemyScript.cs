using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{

    private NavMeshAgent agent;
    private GameObject waypointMaster;
    private int currentNumber;
    private int maxNumber;
    private List<GameObject> theWaypoints;
    public int enemyHp;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        theWaypoints = WaypointSystem.Instance.theWaypoints;
        maxNumber = theWaypoints.Count;
        currentNumber = 0;
        startMoving(currentNumber);
    }

    void startMoving(int number)
    {
        agent.destination = theWaypoints[number].transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Flag" && currentNumber != maxNumber - 1 )
        {
            currentNumber += 1;
            startMoving(currentNumber);
        }
    }

    public void damageFunction(int number)
    {
        enemyHp -= 1;
        if(enemyHp == 0)
        {
            deathFunction();
        }
    }

    private void deathFunction()
    {
        print("Ded");
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("e"))
        {
            damageFunction(1);
        }
    }
}
