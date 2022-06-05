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
    public TextMeshProUGUI livesCountRef;
    public TextMeshProUGUI stateRef;
    public TextMeshProUGUI enemiesRef;
    public TextMeshProUGUI tmp_AnnouncementRef;
    public TextMeshProUGUI changeViewRef;
    public TextMeshProUGUI waveTimerRef;

    public GameObject winPanel;
    public GameObject losePanel;

    public bool stopPlacing;

    public bool enableDelete;

    public List<TowerData> towerDataList = new List<TowerData>();
    

    public string currentToggledState;
    private string nextToggledState;

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



       for (int i = 0; i < towerDataList.Count; i++)
        {
            chosenToggle = towersChoosingGameObject.transform.GetChild(i).gameObject;
            chosenToggle.GetComponent<UItowerSelection>().fillUpDetails(towerDataList[i]);
            chosenToggle.GetComponent<UItowerSelection>().assignedNum = i;
            toggleRef[i] = chosenToggle;
            if(i == 0)
            {
                chosenToggle.GetComponent<Toggle>().isOn = true;
            }
            chosenToggle.GetComponent<Toggle>().group = towersChoosingGameObject.GetComponent<ToggleGroup>();
        }

        nextToggledState = "Freelook";
        CheckStates();
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

        //CheckStates();
    }

    public void MakeFocus(int num)
    {
        currentTowerIndex = num;
        //towerDataList[num].ToggleRef.isOn = true;
        //activeTower.sprite = towerDataList[num].SpriteRef;
    }

    public void CheckStates()
    {
        currentToggledState = nextToggledState;
        switch (currentToggledState)
        {
            case "Freelook":
                TowerButtonStatus(false);
                stateRef.text = "Free look mode";
                stopPlacing = true;
                enableDelete = false;
                nextToggledState = "Build";
                break;

            case "Build":
                TowerButtonStatus(true);
                stateRef.text = "Building mode";
                stopPlacing = false;
                enableDelete = false;
                nextToggledState = "Delete";
                break;

            case "Delete":

                TowerButtonStatus(false);
                stateRef.text = "Delete mode";
                enableDelete = true;
                nextToggledState = "Freelook";
                break;
            default:
                break;
        }
    }

    private void TowerButtonStatus(bool state)
    {
        for(int i = 0; i<toggleRef.Length;i++)
        {
            toggleRef[i].GetComponent<Toggle>().interactable = state;
        }
    }

    /*public void ToggleThreeStates(Toggle togRef)
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
    }*/

    public void LivesUpdate(int number)
    {
        livesCountRef.text = number.ToString();
    }

    public void EnemiesUpdate(int number)
    {
        enemiesRef.text = number.ToString();
    }

    public void changeTextCamera(bool close)
    {
        if (close)
        {
            changeViewRef.text = "Eyes in the skies";
        }
        else
        {
            changeViewRef.text = "Boots on the ground";
        }
    }
    
    public void waveTimerUpdate(string msg)
    {
        waveTimerRef.text = "Time till \n next wave: \n" + msg;
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

