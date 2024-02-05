using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    // Delegate
    Action OnEnemyAttackToPlayer; // 플레이어한테 공격을 하는지 확인하는 델리게이트

    // Components
    Player player;
    Rigidbody rigid;
    Animator animator;

    // Values
    Vector3 direction = Vector3.zero;
    float lookAngle;
    float attackAnimTime;

    // Enemy stats
    float curSpeed;
    public float speed = 3.0f;
    public float attackRange = 2.0f;
    public float rotateSpeed = 5.0f;
    public float attackDelay = 2.5f;
    float StepBackTime = 0f;

    int hp;
    public int maxHp = 10;
    public int HP
    {
        get => hp;
        set
        {
            hp = value;
            Debug.Log($"적의 체력이 [{hp}]만큼 남았습니다");

            if (hp <= 0)
            {
                hp = 0;
                Die();
            }
        }
    }

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

    // Hashes
    readonly int SpeedToHash = Animator.StringToHash("Speed");
    readonly int AttackToHash = Animator.StringToHash("Attack");
    readonly int DamagedToHash = Animator.StringToHash("Damaged");


    // Flags
    bool isAttack = false;
    bool isPlayerAttack = false;
    bool isDamaged = false;

    void Awake()
    {
        player = FindAnyObjectByType<Player>();
        animator = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();

        // setting values
        curSpeed = speed; // speed 값 저장
        hp = maxHp;

        // add function to delegate
        OnEnemyAttackToPlayer += () => player.ChangeAttackFlag();
    }

    void FixedUpdate()
    {
        direction = player.transform.position - transform.position; // 플레이어 방향 백터
        animator.SetFloat(SpeedToHash, speed);

        MoveToPlayer();
        RotateToPlayer();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PlayerAttack") && !isDamaged && isPlayerAttack)
        {
            animator.SetTrigger(DamagedToHash);
            HP--;
            StartCoroutine(HitDelay());
        }
    }

    /// <summary>
    /// 플레이어한테 이동하는 함수
    /// </summary>
    void MoveToPlayer()
    {
        if(direction.magnitude > attackRange)
        {
            rigid.MovePosition(rigid.position + Time.fixedDeltaTime * direction.normalized * speed);
        }
        else if(direction.magnitude <= attackRange) // 플레이어 근처에 도달
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
    /// 플레이어를 공격하는 코루틴 (AttackDelay시간마다 계속 함수를 불러옴)
    /// </summary>
    /// <returns></returns>
    IEnumerator Attack()
    {
        // 공격 애니메이션 실행
        animator.SetTrigger(AttackToHash);
        IsAttack = true;
        attackAnimTime = animator.GetCurrentAnimatorStateInfo(0).length + 0.5f; // 공격 모션 애니메이션 재생시간
        OnEnemyAttackToPlayer?.Invoke();
        yield return new WaitForSeconds(attackAnimTime);
        OnEnemyAttackToPlayer?.Invoke();

        // 뒤로 물러나기
        StepBackTime = UnityEngine.Random.Range(1, attackDelay - attackAnimTime); // 뒤로 물러가는 랜덤 시간
        speed = (speed * -1) / 2;
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * direction * speed);
        yield return new WaitForSeconds(StepBackTime);

        // 정지
        speed = 0f;
        yield return new WaitForSeconds(attackDelay - attackAnimTime - StepBackTime);

        // 공격 딜레이 끝
        IsAttack = false;
    }
    /// <summary>
    /// 플레이어가 적한테 공격을 할 수 있는지 없는지 상태 전환하는 함수(true : 공격 가능 , false : 공격 불가)
    /// </summary>
    public void ChangeAttackFlag()
    {
        isPlayerAttack = !isPlayerAttack;
    }

    IEnumerator HitDelay()
    {
        isDamaged = true;
        yield return new WaitForSeconds(2f);
        isDamaged = false;
    }

    void Die()
    {
        Debug.Log("적이 사망했습니다.");
    }
}
