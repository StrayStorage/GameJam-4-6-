using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{

    public bool rotationReached;

    [Header("TowerProperties")]
    public float damage;

    public float rotationSpeed;

    public float timeBeforeEachShot ;

    public Transform target;

    private float shootingRange = 3;
    // Angular speed in radians per sec.
    public float speed = 1.0f;

    [SerializeField]
    private LayerMask enemyLayerMask;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RangeCheck();
        RotateTowardsTarget();


    }


    void RangeCheck()
    {
        
        Collider[] hitColliderList = Physics.OverlapSphere(transform.position, shootingRange, ~enemyLayerMask);
        
        for (int i = 0; i < hitColliderList.Length; i++)
        {
            if (hitColliderList[i].tag == "Enemy")
            {

                target = hitColliderList[i].gameObject.transform;
                return;
            }
            else
            {
                target = null;
            }

        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }

    void RotateTowardsTarget()
    {
        if (target != null)
        {
            Vector3 targetDirection = target.position - transform.position;

            // The step size is equal to speed times frame time.
            float singleStep = speed * Time.deltaTime;

            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

            if (transform.rotation == Quaternion.LookRotation(new Vector3(newDirection.x, 0, newDirection.z)))
            {
                rotationReached = true;
            }
            else
            {
                rotationReached = false;
            }

            // Draw a ray pointing at our target in
            Debug.DrawRay(transform.position, newDirection, Color.red);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(new Vector3(newDirection.x, 0, newDirection.z));

        }
        else
        {
            rotationReached = false;
        }
    }
}
