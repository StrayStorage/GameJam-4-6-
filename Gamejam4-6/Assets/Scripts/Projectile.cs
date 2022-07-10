using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public enum PROJECTILETYPE
    {
        Arrow,
        Cannon
    }
    public PROJECTILETYPE projectileType;

    public float projectileLifetime = 3f;

    public List<GameObject> hitVisualList;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("DelayDestroy", projectileLifetime);
    }


    void DelayDestroy()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            switch (projectileType)
            {
                case PROJECTILETYPE.Arrow:
                    SoundController.Instance.PlaySoundEffect(2);
                    if (other.gameObject.GetComponent<EnemyScript>())
                    {
                        other.gameObject.GetComponent<EnemyScript>().damageFunction(1);
                        //GameObject newParticleObj = Instantiate(hitVisualList[0], this.transform.position, Quaternion.identity) as GameObject;

                        ParticleSystemController.Instance.SpawnParticle(2, this.transform.position);

                    }
                    Destroy(this.gameObject);
                    break;
                case PROJECTILETYPE.Cannon:
                    SoundController.Instance.PlaySoundEffect(3);
                    if (other.gameObject.GetComponent<EnemyScript>())
                    {
                        other.gameObject.GetComponent<EnemyScript>().damageFunction(2);
                        //GameObject newParticleObj = Instantiate(hitVisualList[1], this.transform.position, Quaternion.identity) as GameObject;
                        ParticleSystemController.Instance.SpawnParticle(0, this.transform.position);
                    }
                    break;
                default:
                    break;
            }

        }
    }
}
