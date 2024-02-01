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
    GameObject root;

    Player player;
    Rigidbody rigid;
    Animator anim;

    // enemy info
    public float speed = 5.0f;
    public float range = 5.0f;

    readonly int Forward = Animator.StringToHash("Move");

    void Awake()
    {
        player = FindAnyObjectByType<Player>();
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
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
            anim.SetBool(Forward, true);
        }
        else if(moveDirection.magnitude <= range)
        {
            anim.SetBool(Forward, false);
        }

    }
}
