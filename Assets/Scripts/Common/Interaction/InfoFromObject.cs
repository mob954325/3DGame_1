using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoFromObject : MonoBehaviour
{
    PlayerController player;
    Collider coll;

    public string name; // 이름

    public int Rank; // 등급

    [TextArea]
    public string Desc; // 설명

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

    // 오브젝트 정보 함수
    void Getinfo()
    {
        string info = name + Rank + Desc;
        //return info;
    }
}
