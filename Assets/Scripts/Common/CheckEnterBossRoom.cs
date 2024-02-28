using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnterBossRoom : MonoBehaviour
{
    Collider coll;
    GameObject childWall;
    public GameObject Boss;

    void Start()
    {
        coll = GetComponent<Collider>();
        childWall = transform.GetChild(0).gameObject;

        if(Boss == null)
        {
            Debug.LogError("���� ������Ʈ�� �������� �ʽ��ϴ�");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {   
            // �÷��̾ �����ϸ� �� Ȱ��ȭ
            coll.enabled = false;
            childWall.SetActive(true);
            Boss.SetActive(true);
            GameManager.Instance.BattleBegin();
        }
    }
}
