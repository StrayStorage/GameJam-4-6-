using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UItowerSelection : MonoBehaviour
{
    
    public GameObject towerNameGO;
    public GameObject towerResourceGO;
    public int assignedNum;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void fillUpDetails(TowerData tD)
    {
        this.GetComponent<Image>().sprite = tD.SpriteRef;
        towerNameGO.GetComponent<TextMeshProUGUI>().text = tD.TowerName;
        towerResourceGO.GetComponent<TextMeshProUGUI>().text = tD.TowerResource + " resources ";
    }
    public void onClick()
    {
        SelectTowerSpawn.Instance.MakeFocus(assignedNum);
    }

}
