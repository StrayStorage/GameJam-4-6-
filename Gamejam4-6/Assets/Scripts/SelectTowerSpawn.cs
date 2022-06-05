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

    public List<Toggle> stateToggleList;

    public string currentToggledState;

    // Start is called before the first frame update
    void Start()
    {
        towerDataList[0].ToggleRef.isOn = true;
        activeTower.sprite = towerDataList[0].SpriteRef;

        currentToggledState = stateToggleList[0].name.Replace("Toggle", "");
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
                                if (hit.collider.tag != "Tower")
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

        CheckStates();
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
    void CheckStates()
    {
        switch (currentToggledState)
        {
            case "Freelook":
                stopPlacing = true;
                placingCoverPanel.SetActive(true);
                break;

            case "Build":
                stopPlacing = false;
                enableDelete = false;
                placingCoverPanel.SetActive(false);
                break;

            case "Delete":
                enableDelete = true;
                placingCoverPanel.SetActive(true);
                break;
            default:
                break;

        }
    }

    public void ToggleThreeStates(Toggle togRef)
    {
        if (togRef.isOn)
        {
            for (int i = 0; i < stateToggleList.Count; i++)
            {
                if (togRef == stateToggleList[i])
                {
                    currentToggledState = stateToggleList[i].name.Replace("Toggle", "");
                }
            }
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

