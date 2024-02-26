using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적 베이스 클래스
/// </summary>
public class EnemyBase : MonoBehaviour
{
    // Delegate
    Action onAttack;

    // Components
    public Player player;
    WeaponControl weapon;
    Rigidbody rigid;
    Animator animator;
    public  EnemyStateBase[] enemyStates;

    // 프로퍼티
    public Player Player => player;
    public WeaponControl Weapon => weapon;
    public Rigidbody Rigid => rigid;
    public Animator Anim => animator;

    // 상태
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
    public float StepBackTime = 0f;

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
                isDie = true;
            }
        }
    }

    int toughness = 0; // 강인성 (0이되면 기절)
    public int maxToughness = 100;

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

            // 기절
            if (toughness <= 0)
            {
                toughness = 0;
                speed = 0;
            }
        }
    }

    // bool
    public bool isAttackBlocked => weapon.CheckIsDefenced(); // 공격이 막혔는지 확인하는 변수

    private bool isDamaged = false;
    public bool IsDamaged => isDamaged;
    bool isDie = false;
    public bool IsDie => isDie;

    // 적이 공동으로 가져야할 애니메이션 파라미터
    // Hashes
    public readonly int SpeedToHash = Animator.StringToHash("Speed");
    public readonly int AttackToHash = Animator.StringToHash("Attack");
    public readonly int DamagedToHash = Animator.StringToHash("Damaged");
    public readonly int DieToHash = Animator.StringToHash("Die");
    public readonly int faintToHash = Animator.StringToHash("Faint"); // 기절 trigger 
    public readonly int isFaintToHash = Animator.StringToHash("isFaint"); // 기절이 끝나기 전까지 대기하게 하는 animator bool값

    void Awake()
    {
        // 초기화
        HP = maxHp;
        Toughness = maxToughness;

        // component
        rigid = GetComponent<Rigidbody>();
        player = FindAnyObjectByType<Player>();
        animator = GetComponent<Animator>();
        weapon = GetComponentInChildren<WeaponControl>();

        // delegate
        onAttack += weapon.ChangeColliderEnableState;

        // states
        enemyStates = new EnemyStateBase[5]; // stat 배열 초기화
        for(int i = 0; i < enemyStates.Length; i++)
        {
            enemyStates[i] = transform.GetChild(i).gameObject.GetComponent<EnemyStateBase>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlayerAttack") && !IsDamaged) // 플레이어의 공격을 받았으면 데미지 받기
        {
            isDamaged = true;
            Anim.SetTrigger(DamagedToHash);
            HP--;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("PlayerAttack") && IsDamaged) // 공격에 벗어나면 피격 비활성화
        {
            isDamaged = false;  
        }
    }

    /// <summary>
    /// 무기 콜라이더의 상태를 변경하는 함수 ( 이벤트 애니메이션 )
    /// </summary>
    public void changeWeaponCollider()
    {
        onAttack?.Invoke();
    }

    /// <summary>
    /// 상태를 받는 함수
    /// </summary>
    /// <param name="state">받을 상태 입력</param>
    public EnemyStateBase SetEnemyState(State state)
    {
        EnemyStateBase selectState = null;
        switch(state)
        {
            case State.Idle:
                selectState = enemyStates[(int)State.Idle].GetComponent<IdleState>();
                break;
            case State.Chasing:
                selectState = enemyStates[(int)State.Chasing].GetComponent<ChasingState>();
                break;
            case State.Attack:
                selectState = enemyStates[(int)State.Attack].GetComponent<AttackState>();
                break;
            case State.Faint:
                selectState = enemyStates[(int)State.Faint].GetComponent<FaintState>();
                break;
            case State.Death:
                selectState = enemyStates[(int)State.Death].GetComponent<DeadState>();
                break;
        }
        return selectState;
    }

    /// <summary>
    /// 특정 애니메이션 시간을 찾는 함수
    /// </summary>
    /// <param name="clipName">찾는 애니메이션 이름</param>
    /// <returns>clipName 애니메이션 시간</returns>
    public float GetAnimClipLength(string clipName)
    {
        float time = 0;
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;

        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == clipName)
            {
                time = ac.animationClips[i].length;
            }
        }
        return time;
    }
}
