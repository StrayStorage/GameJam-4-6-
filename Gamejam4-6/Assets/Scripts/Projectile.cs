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

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DelayDestroy", 3f);
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
                    break;
                case PROJECTILETYPE.Cannon:
                    SoundController.Instance.PlaySoundEffect(3);
                    break;
                default:
                    break;
            }

        }
    }
}
