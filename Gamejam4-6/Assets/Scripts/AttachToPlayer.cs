using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AttachToPlayer : MonoBehaviour
{

    public bool enableFollow;

    public GameObject objectToFollow;

    public float speed = 2.0f;

    public float yOffset = 2.5f;

    public float xOffset = -4f;

    public float zOffset = 0f;

    public Vector3 cacheOriginalPosition;

    private void Start()
    {
        cacheOriginalPosition = this.transform.position;
    }


    void Update()
    {
        float interpolation = speed * Time.deltaTime;
        Vector3 position = this.transform.position;
        
        if (enableFollow)
        {
            position.y = Mathf.Lerp(this.transform.position.y, objectToFollow.transform.position.y + yOffset, interpolation);
            position.x = Mathf.Lerp(this.transform.position.x, objectToFollow.transform.position.x + xOffset, interpolation);
            position.z = Mathf.Lerp(this.transform.position.z, objectToFollow.transform.position.z + zOffset, interpolation);
        }
        else
        {
            position = Vector3.Lerp(this.transform.position, cacheOriginalPosition, interpolation);
        }

        this.transform.position = position;
    }


    public void ResetToOrigin()
    {
        enableFollow = !enableFollow;
        SelectTowerSpawn.Instance.changeTextCamera(enableFollow);
    }
}
