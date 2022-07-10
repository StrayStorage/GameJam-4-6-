using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashingBehaviour : MonoBehaviour
{
    public Animator slashingAnimatorRef;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (slashingAnimatorRef.GetBool("Slash"))
            {
                SoundController.Instance.PlaySoundEffect(4);
                
                if (other.gameObject.GetComponent<EnemyScript>())
                {
                    ParticleSystemController.Instance.SpawnParticle(3, this.transform.position);
                    other.gameObject.GetComponent<EnemyScript>().damageFunction(1);
                    Debug.Log("HIT ENEMY WITH SWORD");
                }
            }
        }
    }
}
