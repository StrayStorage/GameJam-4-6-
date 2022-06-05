using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class SelectTowerSpawn : MonoBehaviour
{
    public static SelectTowerSpawn Instance;

    public int currentTowerIndex;

    //public Image activeTower;
    public GameObject towersChoosingGameObject;

    public TextMeshProUGUI resourceCountRef;

    public bool stopPlacing;

    public bool enableDelete;

    public GameObject placingCoverPanel;

    public List<TowerData> towerDataList = new List<TowerData>();

    public List<Toggle> stateToggleList;

    public string currentToggledState;

    public Vector3 ghostPosition;

    public GameObject ghostObjectRef;

    private GameObject chosenToggle;
    private GameObject[] toggleRef;

    // Start is called before the first frame update
    void Start()
    {
        toggleRef = new GameObject[towerDataList.Count];
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        //towerDataList[0].ToggleRef.isOn = true;
        //activeTower.sprite = towerDataList[0].SpriteRef;

        currentToggledState = stateToggleList[0].name.Replace("Toggle", "");

       for (int i = 0; i < towerDataList.Count; i++)
        {
            chosenToggle = towersChoosingGameObject.transform.GetChild(i).gameObject;
            chosenToggle.GetComponent<UItowerSelection>().fillUpDetails(towerDataList[i]);
            chosenToggle.GetComponent<UItowerSelection>().assignedNum = i;
            toggleRef[i] = chosenToggle;
            print(toggleRef[i].name);
            if(i == 0)
            {
                chosenToggle.GetComponent<Toggle>().isOn = true;
            }
            chosenToggle.GetComponent<Toggle>().group = towersChoosingGameObject.GetComponent<ToggleGroup>();
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        resourceCountRef.text = GameController.Instance.currentResources + "/" + GameController.Instance.totalResourceGiven;

        RaycastHit hit2;
        var ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray2, out hit2))
        {
            if (hit2.collider != null)
            {
                if (!enableDelete)
                {
                    if (!stopPlacing)
                    {
                        if (hit2.collider.tag != "Tower")
                        {
                            if (GameController.Instance.currentResources > 0)
                            {
                                if ((GameController.Instance.currentResources - GameController.Instance.costOfTower[currentTowerIndex]) >= 0)
                                {
                                    ghostPosition = hit2.point;
                                    ghostObjectRef.transform.position = ghostPosition;

                                    for (int c = 0; c < ghostObjectRef.transform.childCount; c++)
                                    {
                                        if (c == currentTowerIndex)
                                        {
                                            ghostObjectRef.transform.GetChild(currentTowerIndex).gameObject.SetActive(true);
                                        }
                                        else
                                        {
                                            ghostObjectRef.transform.GetChild(c).gameObject.SetActive(false);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int c = 0; c < ghostObjectRef.transform.childCount; c++)
                                {
                                    ghostObjectRef.transform.GetChild(c).gameObject.SetActive(false);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                for (int c = 0; c < ghostObjectRef.transform.childCount; c++)
                {
                    ghostObjectRef.transform.GetChild(c).gameObject.SetActive(false);
                }
            }
        }
        else
        {
            for (int c = 0; c < ghostObjectRef.transform.childCount; c++)
            {
                ghostObjectRef.transform.GetChild(c).gameObject.SetActive(false);
            }
        }


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
                toggleRef[0].GetComponent<Toggle>().isOn = true;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                MakeFocus(1);
                toggleRef[1].GetComponent<Toggle>().isOn = true;
            }
        }

        CheckStates();
    }

    public void MakeFocus(int num)
    {
        currentTowerIndex = num;
        //towerDataList[num].ToggleRef.isOn = true;
        //activeTower.sprite = towerDataList[num].SpriteRef;
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
    public string TowerName;

    public int TowerResource;

    public Sprite SpriteRef;

    public GameObject PrefabRef;


    public TowerData(string towerName, int towerResource, Sprite spriteRef, GameObject prefabRef)
    {

        SpriteRef = spriteRef;

        PrefabRef = prefabRef;
    }
}

