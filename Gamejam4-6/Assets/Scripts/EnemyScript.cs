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
    private GameObject targetGO;
    
    private float currAttackTime;
    private float currCDTime;
    

    [Header("References")]
    public int queueNum;
    public bool stopBool;
    public bool attackBool;
    public GameObject theChild;
    public bool currCooldownStatus;

    [Header("Stats")]
    public int enemyHp;
    public int minusHp = 1;
    public float maxDetection;

    public float attackTime;
    public float cooldownTime;



    [SerializeField]
    private LayerMask targetLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        theWaypoints = WaypointSystem.Instance.theWaypoints;
        maxNumber = theWaypoints.Count;
        currentNumber = 0;
        stopBool = false;
        attackBool = false;
        currCooldownStatus = false;
    }
    

    public void startMoving(int number)
    {
        agent.destination = theWaypoints[number].transform.position;
    }

    public void StopMoving(bool toMove)
    {
            stopBool = toMove;
            agent.isStopped = stopBool;

    }
    

    public void assignQNum(int qNumber)
    {
        queueNum = qNumber;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Flag" )
        {
            if(currentNumber != maxNumber - 1)
            {
                currentNumber += 1;
                startMoving(currentNumber);
            }
                
        }
    }

    public void StartAttackTimer()
    {
        currAttackTime = attackTime;
    }

    public void damageFunction(int number)
    {
        enemyHp -= number;
        if(enemyHp <= 0)
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
        if (!attackBool && !currCooldownStatus)
        {
            
            RangeCheck();
        }
        if (attackBool && currAttackTime > 0)
        {
            currAttackTime -= Time.deltaTime;
        }
        else if(attackBool && currAttackTime < 0)
        {
            currAttackTime = 0.0f;
            attackBool = false;
            theChild.GetComponent<Animator>().SetBool("Attack", false);
            currCooldownStatus = true;
            currCDTime = cooldownTime;
        }
        if (currCooldownStatus)
        {
            currCDTime -= Time.deltaTime;
            if(currCDTime < 0)
            {
                currCDTime = cooldownTime;
                currCooldownStatus = false;
                Debug.Log("Cooldown over");
            }
        }
        //A way to start the waves
        if (Input.GetKeyUp("x"))
        {
            attackBool = !attackBool;
            theChild.GetComponent<Animator>().SetBool("Attack", attackBool);
        }
        
    }
    void RangeCheck()
    {
        targetGO = null; 
        Collider[] hitColliderList = Physics.OverlapSphere(transform.position, maxDetection, targetLayerMask);
        for (int i = 0; i <hitColliderList.Length; i++)
        {
            if (hitColliderList[i].tag == "Player")
            {
                SetAttackTarget(hitColliderList[i].gameObject);
            }
            else if (hitColliderList[i].tag == "Tower")
            {
                SetAttackTarget(hitColliderList[i].gameObject);
            }
        }
        if (targetGO == null)
        {
                attackBool = false;
                theChild.GetComponent<Animator>().SetBool("Attack", attackBool);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, maxDetection);
    }

    void SetAttackTarget(GameObject target)
    {
        attackBool = true;
        theChild.GetComponent<Animator>().SetBool("Attack", attackBool);
        targetGO = target;
    }
}
