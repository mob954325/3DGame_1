using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Interaction tag�� ���� ������Ʈ�� ������ Panel�� �����ִ� ��ũ��Ʈ
/// </summary>
public class UI_Info : MonoBehaviour
{
    /// <summary>
    /// ���� �÷��̾ ��ȣ�ۿ����� ��ǥ ������Ʈ
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
