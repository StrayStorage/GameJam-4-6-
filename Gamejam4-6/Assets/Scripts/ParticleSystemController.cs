using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    public static ParticleSystemController Instance;

    public List<GameObject> particleSystemList;

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
    }

    public void SpawnParticle(int index, Vector3 location)
    {
        GameObject newParticleObj = Instantiate(particleSystemList[index], location, Quaternion.identity) as GameObject;
    }
}
