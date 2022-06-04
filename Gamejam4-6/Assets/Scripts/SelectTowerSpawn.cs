using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class SelectTowerSpawn : MonoBehaviour
{
    public int currentTowerIndex;

    public Image activeTower;

    public Text resourceCountRef;

    public bool stopPlacing;

    public bool enableDelete;

    public GameObject placingCoverPanel;

    public List<TowerData> towerDataList = new List<TowerData>();

    public Text deleteButtonText;
    public Text stopPlacingButtonText;

    // Start is called before the first frame update
    void Start()
    {
        towerDataList[0].ToggleRef.isOn = true;
        activeTower.sprite = towerDataList[0].SpriteRef;
    }

    // Update is called once per frame
    void Update()
    {
        resourceCountRef.text = GameController.Instance.currentResources + "/" + GameController.Instance.totalResourceGiven;

        
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("MouseDown");
                RaycastHit hit;
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null)
                    {
                        if (!enableDelete)
                        {
                            if (!stopPlacing)
                            {
                                if (GameController.Instance.currentResources > 0)
                                {
                                    if ((GameController.Instance.currentResources - GameController.Instance.costOfTower[currentTowerIndex]) >= 0)
                                    {
                                        GameController.Instance.currentResources -= GameController.Instance.costOfTower[currentTowerIndex];
                                        GameObject newTower = Instantiate(towerDataList[currentTowerIndex].PrefabRef, hit.point, Quaternion.identity) as GameObject;
                                    }
                                    else
                                    {
                                        Debug.Log("Insufficient Resource");
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (hit.collider.tag == "Tower")
                            {
                                //refund resources based on type
                                GameController.Instance.currentResources += GameController.Instance.costOfTower[(int)hit.collider.gameObject.GetComponent<TowerBehaviour>().towerType];

                                Destroy(hit.collider.gameObject);
                            }

                            Debug.Log("HERE");
                        }
                    }
                }
            }


        if (!stopPlacing)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                MakeFocus(0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                MakeFocus(1);
            }
        }

        
    }

    public void MakeFocus(int num)
    {
        currentTowerIndex = num;
        towerDataList[num].ToggleRef.isOn = true;
        activeTower.sprite = towerDataList[num].SpriteRef;
    }
    public void CheckFocus(Toggle togRef)
    {
        if (togRef.isOn)
        {
            for (int i = 0; i < towerDataList.Count; i++)
            {
                if (togRef == towerDataList[i].ToggleRef)
                {
                    MakeFocus(i);
                }
            }
        }
    }
    public void TogglePlacing()
    {
        stopPlacing = !stopPlacing;
        placingCoverPanel.SetActive(stopPlacing);
        enableDelete = false;

        stopPlacingButtonText.text = "Stop " + "(" + stopPlacing.ToString() + ")";
        deleteButtonText.text = "Delete " + "(" + enableDelete.ToString() + ")";
        
        if (enableDelete)
        {
            placingCoverPanel.SetActive(enableDelete);
        }
    }

    public void EnableDelete()
    {
        enableDelete = !enableDelete;
        placingCoverPanel.SetActive(enableDelete);

        stopPlacingButtonText.text = "Stop " + "(" + stopPlacing.ToString() + ")";
        deleteButtonText.text = "Delete " + "(" + enableDelete.ToString() + ")";

        if (stopPlacing)
        {
            placingCoverPanel.SetActive(stopPlacing);
        }

    }
}

[Serializable]
public class TowerData
{
    public Toggle ToggleRef;

    public Sprite SpriteRef;

    public GameObject PrefabRef;

    public TowerData(Toggle toggleRef, Sprite spriteRef, GameObject prefabRef)
    {
        ToggleRef = toggleRef;

        SpriteRef = spriteRef;

        PrefabRef = prefabRef;
    }
}

