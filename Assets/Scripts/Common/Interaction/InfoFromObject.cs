using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoFromObject : MonoBehaviour
{
    PlayerController player;
    Collider coll;

    public string name; // �̸�

    public int Rank; // ���

    [TextArea]
    public string Desc; // ����

    void Awake()
    {
        player = null;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerController>();
            player.OnInteractionAction += Getinfo;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.OnInteractionAction -= Getinfo;
            player = null;
        }
    }

    // ������Ʈ ���� �Լ�
    void Getinfo()
    {
        string info = name + Rank + Desc;
        //return info;
    }
}
