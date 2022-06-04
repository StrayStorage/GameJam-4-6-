using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectTowerSpawn : MonoBehaviour
{
    public int currentTowerIndex;


    public List<Toggle> toggleList;

    public List<Sprite> spriteList;


    public Image activeTower;


    // Start is called before the first frame update
    void Start()
    {
        toggleList[0].isOn = true;
        activeTower.sprite = spriteList[0];
    }

    // Update is called once per frame
    void Update()
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

    public void MakeFocus(int num)
    {
        currentTowerIndex = num;
        toggleList[num].isOn = true;
        activeTower.sprite = spriteList[num];
    }
    public void CheckFocus(Toggle togRef)
    {
        if (togRef.isOn)
        {
            for (int i = 0; i < toggleList.Count; i++)
            {
                if (togRef == toggleList[i])
                {
                    MakeFocus(i);
                }
            }
        }
    }
}
