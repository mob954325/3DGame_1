using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    //완료
    //1. player tag를 가진 오브젝트 찾기 
    //2. 해당 오브젝트에게 다가가기 : 무기 사정거리 내까지 
    //3. 공격하기 -> 대기 -> 공격 순으로 작동 

    // components
    Player player;
    Rigidbody rigid;
    Animator anim;

    // Values
    Vector3 direction = Vector3.zero;
    float lookAngle;

    // Flags
    bool isAttack = false;

    // enemy info
    float curSpeed;
    public float speed = 3.0f;
    public float range = 2.0f;
    public float rotateSpeed = 5.0f;
    public float attackDelay = 2.5f;


    // Hashes
    readonly int ForwardToHash = Animator.StringToHash("Move");
    readonly int AttackToHash = Animator.StringToHash("Attack");

    void Awake()
    {
        player = FindAnyObjectByType<Player>();
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();

        curSpeed = speed; // speed 값 저장
    }

    void FixedUpdate()
    {
        direction = player.transform.position - transform.position; // 플레이어 방향 백터
        MoveToPlayer();
        RotateToPlayer();
        checkAttacking();
    }

    /// <summary>
    /// 플레이어한테 이동하는 함수
    /// </summary>
    void MoveToPlayer()
    {
        if(direction.magnitude > range)
        {
            rigid.MovePosition(rigid.position + Time.fixedDeltaTime * direction * speed);
            anim.SetBool(ForwardToHash, true);
        }
        else if(direction.magnitude <= range) // 플레이어 근처에 도달
        {
            anim.SetBool(ForwardToHash, false);
            if(!isAttack)
            {
                StopAllCoroutines();
                StartCoroutine(Attack());
            }
        }
    }

    void checkAttacking()
    {
        if(isAttack)
        {
            speed = 0f;
        }
        else
        {
            speed = curSpeed;
        }
    }

    /// <summary>
    /// 플레이어를 향해 회전하는 함수
    /// </summary>
    void RotateToPlayer()
    {
        Vector3 rotDirection = Vector3.zero;
        rotDirection.x = direction.x;
        rotDirection.z = direction.z;
        rotDirection.Normalize();


        if (rotDirection.magnitude > 0.01f)
        {
            lookAngle = Mathf.Atan2(rotDirection.x, rotDirection.z) * Mathf.Rad2Deg; // 회전할 방향
        }
        float angle = Mathf.LerpAngle(transform.localRotation.eulerAngles.y, lookAngle, rotateSpeed * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(0, angle, 0); // rotate Player model
    }

    /// <summary>
    /// 플레이어를 공격할 코루틴 (AttackDelay시간마다 계속 함수를 불러옴)
    /// </summary>
    /// <returns></returns>
    IEnumerator Attack()
    {
        anim.SetTrigger(AttackToHash);
        isAttack = true;
        yield return new WaitForSeconds(attackDelay);
        // 공격후 경직 내용 넣기
        isAttack = false;
    }
}
