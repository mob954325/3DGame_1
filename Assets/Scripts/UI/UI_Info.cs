using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Info : MonoBehaviour
{
    public GameObject targetObj;

    TextMeshProUGUI enemyName;
    TextMeshProUGUI Rank;
    TextMeshProUGUI Desc;

    bool isActive = false;

    void Awake()
    {
        enemyName = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        Rank = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        Desc = transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
    }


    void LateUpdate()
    {
        enemyName.text = targetObj.GetComponent<InfoFromObject>().enemyName;
        Rank.text = targetObj.GetComponent<InfoFromObject>().Rank;
        Desc.text = targetObj.GetComponent<InfoFromObject>().Desc;        
    }

    public void ActiveUI()
    {
        isActive = !isActive;
        gameObject.SetActive(isActive);
    }
}
