using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastPointer : MonoBehaviour
{
    public GameObject towerToSpawn;

    void Update()
    {


        // Start is called before the first frame update
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("MouseDown");
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    //hit.rigidbody.AddForceAtPosition(ray.direction * pokeForce, hit.point);
                    Debug.Log(hit.point);
                    GameObject newTower = Instantiate(towerToSpawn, hit.point, Quaternion.identity, this.transform) as GameObject;
                }
            }
        }
    }
}
