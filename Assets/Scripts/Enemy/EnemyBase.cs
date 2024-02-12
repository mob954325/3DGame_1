using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    // Delegate
    /// <summary>
    /// 플레이어한테 공격을 하는지 확인하는 델리게이트
    /// </summary>
    Action OnEnemyAttackToPlayer;

    // Components
    Player player;

    /// <summary>
    /// player를 가진 오브젝트가 있는지 확인하기 위한 프로퍼티
    /// </summary>
    Player Player
    {
        get => player;
        set
        {
            player = value;
            if (player == null)
            {
                Debug.LogError("Player 스크립트를 가진 오브젝트가 존재하지 않습니다.");
    
                // 존재하지 않으면 빈 오브젝트 스크립트 생성
                GameObject emptyScriptObject = new GameObject("EmptyScript");
                emptyScriptObject.transform.parent = transform;
                emptyScriptObject.AddComponent<Player>();
                player = emptyScriptObject.GetComponent<Player>();
    
                emptyScriptObject.SetActive(false);
            }
        }
    }


    Rigidbody rigid;
    Animator animator;

    // Values
    Vector3 direction = Vector3.zero;
    float lookAngle;
    float attackAnimTime;

    // Enemy stats
    float baseSpeed;
    [Header ("Enemy Stats")]
    public float speed = 3.0f;
    public float rotateSpeed = 5.0f;
    [Space (10f)]
    public float attackRange = 2.0f;
    public float attackDelay = 2.5f;
    public float parryingChanceTime = 0.5f; // 방어를 시작한 순간 패링 찬스를 얻는 시간 값
    [SerializeField] float StepBackTime = 0f;

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

    int toughness = 0; // 강인성 (0이되면 기절)
    public int Toughness
    {
        get => toughness;
        set
        {
            toughness = value;
            Debug.Log($"남은 강인성 : [{toughness}]");

            if(toughness <= 0)
            {
                toughness = 0;
                speed = 0;

                animator.SetBool(isFaintToHash, isFaint);
                Invoke("AfterFaint", 5f);
            }

        }
    }
    public int maxToughness = 100;

    // Hashes
    readonly int SpeedToHash = Animator.StringToHash("Speed");
    readonly int AttackToHash = Animator.StringToHash("Attack");
    readonly int DamagedToHash = Animator.StringToHash("Damaged");
    readonly int DieToHash = Animator.StringToHash("Die");
    readonly int isFaintToHash = Animator.StringToHash("isFaint");

    // Flags
    [Header("Enemy Flag")]
    [SerializeField] bool isAttack = false;
    /// <summary>
    /// 공격 했는지 확인하는 파라미터
    /// </summary>
    bool IsAttack
    {
        get => isAttack;
        set
        {
            isAttack = value;

            if (!isAttack)
            {
                speed = baseSpeed;
            }
        }
    }
    bool isPlayerAttack = false; // 플레이어 공격 여부
    [SerializeField] bool isDamaged = false; // 피격 여부
    [SerializeField] bool isDead => HP <= 0; // 사망 여부
    [SerializeField] bool isFaint => Toughness <= 0; // 기절 여부 
    float playerDefenceTIme = 0f; // player가 방어한 시간 저장 변수
    bool alreadyParrying = false; // 이미 패링당했는지 확인하는 변수

    void Awake()
    {
        Player = FindAnyObjectByType<Player>();
        animator = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();

        // setting values
        baseSpeed = speed; // speed 값 저장
        HP = maxHp;
        Toughness = maxToughness;

        // add function to delegate
        OnEnemyAttackToPlayer += () => Player.Player_ChangeAttackFlag();
    }

    void FixedUpdate()
    {
        direction = Player.transform.position - transform.position; // 플레이어 방향 백터
        animator.SetFloat(SpeedToHash, speed);

        // 적 행동 함수들
        if(!isDead && !isFaint)
        {
            MoveToPlayer();
            RotateToPlayer();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PlayerAttack") && !isDamaged && isPlayerAttack)
        {
            HP--;
            StartCoroutine(HitDelay());

            if (isFaint) return;
            animator.SetTrigger(DamagedToHash);
        }
    }

    /// <summary>
    /// 플레이어한테 이동하는 함수
    /// </summary>
    void MoveToPlayer()
    {
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * direction.normalized * speed);

        if(direction.magnitude <= attackRange) // 플레이어 근처에 도달
        {
            if (!IsAttack)
            {
                alreadyParrying = false; // 패링 조건 활성화

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
        speed = 0f;
        // 공격 애니메이션 실행
        animator.SetTrigger(AttackToHash);
        IsAttack = true;

        OnEnemyAttackToPlayer?.Invoke(); // Player.cs 적 공격 flag 변경

        attackAnimTime = animator.GetCurrentAnimatorStateInfo(0).length; // 공격 모션 애니메이션 재생시간
        yield return new WaitForSeconds(attackAnimTime);

        OnEnemyAttackToPlayer?.Invoke();

        // 뒤로 물러나기
        StepBackTime = UnityEngine.Random.Range(1, attackDelay - attackAnimTime); // 뒤로 물러가는 랜덤 시간
        yield return new WaitForSeconds(1f);

        speed = baseSpeed * -1 / 2f;
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
    public void Enemy_ChangeAttackFlag()
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
        animator.SetTrigger(DieToHash);
    }

    /// <summary>
    /// 플레이어가 패링이 가능한지 확인하는 함수
    /// </summary>
    public void CheckParrying()
    {
        playerDefenceTIme = Player.GetComponent<Player>().GetDefenceTime();

        if (IsAttack && playerDefenceTIme > 0 
            && playerDefenceTIme <= parryingChanceTime
            && direction.magnitude <= attackRange
            && !alreadyParrying)
        {
            alreadyParrying = true;
            // 공격 정지
            StopCoroutine(Attack());
            Toughness -= 20;
            animator.SetTrigger(DamagedToHash); // 피해 받음
        }
    }

    /// <summary>
    /// 기절 후 수행할 함수
    /// </summary>
    void AfterFaint()
    {
        Toughness = maxToughness;
        animator.SetBool(isFaintToHash, isFaint);
        speed = baseSpeed;
    }
}
