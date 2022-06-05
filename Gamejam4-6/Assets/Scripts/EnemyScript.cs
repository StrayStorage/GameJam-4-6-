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
    public int minusHp = 1;
    public int queueNum;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        theWaypoints = WaypointSystem.Instance.theWaypoints;
        maxNumber = theWaypoints.Count;
        currentNumber = 0;
        startMoving(currentNumber);
    }

    public void startMoving(int number)
    {
        agent.destination = theWaypoints[number].transform.position;
    }

    public void assignQNum(int qNumber)
    {
        queueNum = qNumber;
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
        enemyHp -= number;
        if(enemyHp < 0)
        {
            deathFunction();
        }
    }

    public void deathFunction()
    {
        print("Ded");
        SpawnScript.Instance.imDead(queueNum);
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
