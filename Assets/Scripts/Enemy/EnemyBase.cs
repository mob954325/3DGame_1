using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    //완료
    //1. player tag를 가진 오브젝트 찾기 
    //2. 해당 오브젝트에게 다가가기 : 무기 사정거리 내까지 
    // 미완료
    //3. 공격하기 -> 대기 -> 공격 순으로 작동 

    // components
    Player player;
    Rigidbody rigid;

    // enemy info
    public float speed = 5.0f;
    public float range = 5.0f;



    void Awake()
    {
        player = FindAnyObjectByType<Player>();
        rigid = GetComponent<Rigidbody>();
        Debug.Log($"감지된 플레이어 오브젝트 이름 : {player.gameObject.name}");
    }

    void FixedUpdate()
    {
        MoveToPlayer();
    }

    void MoveToPlayer()
    {
        Vector3 moveDirection = player.transform.position - transform.position; // player

        if(moveDirection.magnitude > range)
        {
            rigid.MovePosition(rigid.position + Time.fixedDeltaTime * moveDirection * speed);
        }

        Debug.Log($"플레이어더미 사이의 거리 : {moveDirection.magnitude}");
    }
}
