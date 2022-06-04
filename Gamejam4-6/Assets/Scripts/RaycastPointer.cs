using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastPointer : MonoBehaviour
{
    public SelectTowerSpawn selectTowerSpawnRef;

    public List<GameObject> towerPrefabList;

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
                    GameObject newTower = Instantiate(towerPrefabList[selectTowerSpawnRef.currentTowerIndex], hit.point, Quaternion.identity, this.transform) as GameObject;
                }
            }
        }
    }
}
