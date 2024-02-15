using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Interaction tag를 가진 오브젝트의 정보를 Panel로 보여주는 스크립트
/// </summary>
public class UI_Info : MonoBehaviour
{
    /// <summary>
    /// 현재 플레이어가 상호작용중인 목표 오브젝트
    /// </summary>
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

        GameManager.Instance.player.controller.OnInteractionAction += ActiveUI;
    }


    void LateUpdate()
    {
        if(targetObj != null)
        {
            enemyName.text = targetObj.GetComponent<InfoFromObject>().name;
            Rank.text = targetObj.GetComponent<InfoFromObject>().Rank;
            Desc.text = targetObj.GetComponent<InfoFromObject>().Desc;        
        }
    }

    public void ActiveUI()
    {
        isActive = !isActive;
        gameObject.SetActive(isActive);
    }
}
