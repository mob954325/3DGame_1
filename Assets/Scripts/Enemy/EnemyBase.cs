using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    // Delegate
    /// <summary>
    /// 공격시 실행하는 델리게이트
    /// </summary>
    Action onAttack;

    // Components
    public Player player;
    public WeaponControl weapon;
    public Rigidbody rigid;
    public Animator animator;

    // Values
    public Vector3 direction = Vector3.zero;
    float lookAngle;
    float attackAnimTime;

    // Enemy stats
    [Header("Enemy Stats")]
    public float baseSpeed = 3.0f;
    public float speed = 3.0f;
    public float rotateSpeed = 5.0f;
    [Space(10f)]
    public float attackRange = 2.0f;
    public float attackDelay = 2.5f;
    public float ToughnessDelay = 2f;  // 강인성 감소 딜레이 시간
    [SerializeField] float StepBackTime = 0f;

    int hp;
    public int maxHp = 10;
    public int HP
    {
        get => hp;
        set
        {
            hp = value;
            //Debug.Log($"적의 체력이 [{hp}]만큼 남았습니다");

            if (hp <= 0)
            {
                hp = 0;
                //Die();
            }
        }
    }

    int toughness = 0; // 강인성 (0이되면 기절)

    /// <summary>
    /// 강인성(toughness)를 참조하는 파라미터 (0이되면 기절 애니메이션을 실행한다)
    /// </summary>
    public int Toughness
    {
        get => toughness;
        set
        {
            toughness = value;
            //Debug.Log($"남은 강인성 : [{toughness}]");
            //animator.SetBool(isFaintToHash, isFaint);

            if (toughness <= 0)
            {
                toughness = 0;
                speed = 0;

                // 애니메이션 실행
                //animator.SetTrigger(faintToHash);

                Invoke("AfterFaint", 3f);
                //StartCoroutine(AfterFaint());
            }
        }
    }

    // bool
    public bool isAttack;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        player = FindAnyObjectByType<Player>();
        animator = GetComponent<Animator>();
        weapon = GetComponentInChildren<WeaponControl>();
    }

    void FixedUpdate()
    {
        direction = player.transform.position - transform.position; // 플레이어 방향 백터
        //animator.SetFloat(SpeedToHash, speed);

        MoveToPlayer();
        RotateToPlayer();
    }

    /// <summary>
    /// 플레이어한테 이동하는 함수 ( AttackRange 내에 도달하면 공격 )
    /// </summary>
    void MoveToPlayer()
    {
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * direction.normalized * speed);

        if (direction.magnitude <= attackRange) // 플레이어 근처에 도달
        {
            isAttack = true;
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
}
