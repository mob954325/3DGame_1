using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 적 프로퍼티 클래스
/// </summary>
public class EnemyBase : MonoBehaviour
{
    // Components
    Player player;
    WeaponControl weapon;
    Rigidbody rigid;
    Animator animator;
    public  EnemyStateBase[] enemyStates;

    // 프로퍼티
    public Player Player => player;
    public WeaponControl Weapon => weapon;
    public Rigidbody Rigid => rigid;
    public Animator Anim => animator;

    public enum State
    {
        Idle = 0,
        Chasing,
        Attack,
        Faint,
        Death
    }

    public State states;


    // Values
    public Vector3 direction = Vector3.zero;
    public float lookAngle;

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

    // Hashes
    public readonly int SpeedToHash = Animator.StringToHash("Speed");
    public readonly int AttackToHash = Animator.StringToHash("Attack");
    public readonly int DamagedToHash = Animator.StringToHash("Damaged");
    public readonly int DieToHash = Animator.StringToHash("Die");
    public readonly int faintToHash = Animator.StringToHash("Faint"); // 기절 trigger 
    public readonly int isFaintToHash = Animator.StringToHash("isFaint"); // 기절이 끝나기 전까지 대기하게 하는 animator bool값

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        player = FindAnyObjectByType<Player>();
        animator = GetComponent<Animator>();
        weapon = GetComponentInChildren<WeaponControl>();

        enemyStates = new EnemyStateBase[transform.childCount - 2]; // stat 배열 초기화
        for(int i = 0; i < enemyStates.Length; i++)
        {
            enemyStates[i] = transform.GetChild(i).gameObject.GetComponent<EnemyStateBase>();
        }
    }


    /// <summary>
    /// 상태를 받는 함수
    /// </summary>
    /// <param name="state">받을 상태 입력</param>
    public void SetEnemyState(State state)
    {
        switch(state)
        {
            case State.Idle:
                enemyStates[(int)State.Idle].GetComponent<IdleState>();
                break;

            case State.Chasing:
                break;

            case State.Attack:
                break;

            case State.Faint:
                break;

            case State.Death:
                break;
        }
    }
}
