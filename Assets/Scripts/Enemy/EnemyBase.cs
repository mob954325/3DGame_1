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
    public Player player;
    Rigidbody rigid;
    Animator anim;

    // Values
    Vector3 direction = Vector3.zero;
    float lookAngle;
    float attackAnimTime;

    // Flags
    bool isAttack = false;

    /// <summary>
    /// 공격 했는지 확인하는 파라미터
    /// </summary>
    bool IsAttack
    {
        get => isAttack;
        set
        {
            isAttack = value;

            if(!isAttack)
            {
                speed = curSpeed;
            }
        }
    }

    // enemy info
    float curSpeed;
    public float speed = 3.0f;
    public float range = 2.0f;
    public float rotateSpeed = 5.0f;
    public float attackDelay = 2.5f;
    float StepBackTime = 0f;

    // Hashes
    readonly int SpeedToHash = Animator.StringToHash("Speed");
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
        anim.SetFloat(SpeedToHash, speed);

        MoveToPlayer();
        RotateToPlayer();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PlayerAttack"))
        {
            Debug.Log("Attacked by Player !!");
        }
    }

    /// <summary>
    /// 플레이어한테 이동하는 함수
    /// </summary>
    void MoveToPlayer()
    {
        if(direction.magnitude > range)
        {
            rigid.MovePosition(rigid.position + Time.fixedDeltaTime * direction.normalized * speed);
        }
        else if(direction.magnitude <= range) // 플레이어 근처에 도달
        {
            if(!IsAttack)
            {
                StopAllCoroutines();
                StartCoroutine(Attack());
            }
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
        // 공격 애니메이션 실행
        anim.SetTrigger(AttackToHash);
        IsAttack = true;
        attackAnimTime = anim.GetCurrentAnimatorStateInfo(0).length + 0.5f; // 공격 모션 애니메이션 재생시간
        yield return new WaitForSeconds(attackAnimTime);

        // 뒤로 물러나기
        StepBackTime = Random.Range(1, attackDelay - attackAnimTime); // 뒤로 물러가는 랜덤 시간
        speed = (speed * -1) / 2;
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * direction * speed);
        yield return new WaitForSeconds(StepBackTime);

        // 정지
        speed = 0f;
        yield return new WaitForSeconds(attackDelay - attackAnimTime - StepBackTime);

        // 공격 딜레이 끝
        IsAttack = false;
    }
}
