using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    //�Ϸ�
    //1. player tag�� ���� ������Ʈ ã�� 
    //2. �ش� ������Ʈ���� �ٰ����� : ���� �����Ÿ� ������ 
    // �̿Ϸ�
    //3. �����ϱ� -> ��� -> ���� ������ �۵� 

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
