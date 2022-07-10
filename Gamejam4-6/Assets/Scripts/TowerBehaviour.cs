using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBehaviour : MonoBehaviour
{
    public enum TOWERTYPE
    {
        Arrow,
        Cannon
    }
    public TOWERTYPE towerType;

    [Header("LiveTowerProperties")]
    public bool rotationReached;
    public Transform updatedTarget;
    public float distanceToTarget;
    public float currentHealth;


    [Header("TowerProperties")]
    public float timeBeforeEachShot = 0.5f;
    private float cachedTimeBeforeEachShot;
    public float maxShootingRange = 5;
    public float rotationSpeed = 5.0f;
    private float damage;
    public float maxHealth;


    [Header("Projectile Properties")]
    private float projectileForce = 1000f;
    public Transform projectilePositionReference;
    public GameObject projectilePrefab;

    [Header("Tower UI Properties")]
    public GameObject canvasTower;
    public Slider sliderVal;


    [Header("EnemyProperties")]
    [SerializeField]
    private LayerMask enemyLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        distanceToTarget = maxShootingRange;
        cachedTimeBeforeEachShot = timeBeforeEachShot;

        switch (towerType)
        {
            case TOWERTYPE.Arrow:
                maxHealth = 100;
                break;
            case TOWERTYPE.Cannon:
                maxHealth = 100;
                break;
            default:
                maxHealth = 100;
                break;
        }
        currentHealth = maxHealth;

        canvasTower.name += " " + towerType;
        canvasTower.transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        HealthUpdate();
        RangeCheck();
        RotateTowardsTarget();
        Shoot();
    }

    void HealthUpdate()
    {
        float newVal = currentHealth / maxHealth;

        sliderVal.value = newVal;
    }
    public void TakeDamage(float takenDmg)
    {
        currentHealth -= takenDmg;
    }
    void RangeCheck()
    {
        Collider[] hitColliderList = Physics.OverlapSphere(transform.position, maxShootingRange, enemyLayerMask);
        
        for (int i = 0; i < hitColliderList.Length; i++)
        {
            //Debug.Log(hitColliderList[i].gameObject.name);

            if (hitColliderList[i].tag == "Enemy")
            {
                if (distanceToTarget > Vector3.Distance(hitColliderList[i].gameObject.transform.position, this.transform.position))
                {
                    updatedTarget = hitColliderList[i].gameObject.transform;

                    distanceToTarget = Vector3.Distance(updatedTarget.gameObject.transform.position, this.transform.position);
                }
            }
            else
            {
                updatedTarget = null;
                distanceToTarget = maxShootingRange;
            }
        }

        if (updatedTarget != null)
        {
            //checks if the current target is getting further away and if it exits the physics overlap sphere
            if (Vector3.Distance(updatedTarget.gameObject.transform.position, this.transform.position) > distanceToTarget || hitColliderList.Length == 0)
            {
                updatedTarget = null;
                distanceToTarget = maxShootingRange;
            }
        }

    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, maxShootingRange);
    }
    
    void RotateTowardsTarget()
    {
        if (updatedTarget != null)
        {
            Vector3 targetDirection = updatedTarget.position - transform.position;

            // The step size is equal to speed times frame time.
            float singleStep = rotationSpeed * Time.deltaTime;

            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

            if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(new Vector3(newDirection.x, 0, newDirection.z))) < 10)
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

    void Shoot()
    {
        if (rotationReached && updatedTarget)
        {
            if (timeBeforeEachShot <= 0)
            {
                switch (towerType)
                {
                    case TOWERTYPE.Arrow:
                        SoundController.Instance.PlaySoundEffect(0);
                        break;
                    case TOWERTYPE.Cannon:
                        SoundController.Instance.PlaySoundEffect(1);
                        break;
                    default:
                        break;
                }



                //SoundController.Instance.PlaySoundEffect(0);


                timeBeforeEachShot = cachedTimeBeforeEachShot;

                GameObject newProjectile = Instantiate(projectilePrefab, projectilePositionReference.position, projectilePositionReference.transform.rotation) as GameObject;

                newProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * projectileForce);
            }
            else
            {
                timeBeforeEachShot -= Time.deltaTime;
            }

        }
    }

    private void OnDestroy()
    {
        Destroy(canvasTower);
    }
}
