using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �� ������Ƽ Ŭ����
/// </summary>
public class EnemyBase : MonoBehaviour
{
    // Components
    Player player;
    WeaponControl weapon;
    Rigidbody rigid;
    Animator animator;
    public  EnemyStateBase[] enemyStates;

    // ������Ƽ
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
    public float ToughnessDelay = 2f;  // ���μ� ���� ������ �ð�
    [SerializeField] float StepBackTime = 0f;

    int hp;
    public int maxHp = 10;
    public int HP
    {
        get => hp;
        set
        {
            hp = value;
            //Debug.Log($"���� ü���� [{hp}]��ŭ ���ҽ��ϴ�");

            if (hp <= 0)
            {
                hp = 0;
                //Die();
            }
        }
    }

    int toughness = 0; // ���μ� (0�̵Ǹ� ����)

    /// <summary>
    /// ���μ�(toughness)�� �����ϴ� �Ķ���� (0�̵Ǹ� ���� �ִϸ��̼��� �����Ѵ�)
    /// </summary>
    public int Toughness
    {
        get => toughness;
        set
        {
            toughness = value;
            //Debug.Log($"���� ���μ� : [{toughness}]");
            //animator.SetBool(isFaintToHash, isFaint);

            if (toughness <= 0)
            {
                toughness = 0;
                speed = 0;

                // �ִϸ��̼� ����
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
    public readonly int faintToHash = Animator.StringToHash("Faint"); // ���� trigger 
    public readonly int isFaintToHash = Animator.StringToHash("isFaint"); // ������ ������ ������ ����ϰ� �ϴ� animator bool��

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        player = FindAnyObjectByType<Player>();
        animator = GetComponent<Animator>();
        weapon = GetComponentInChildren<WeaponControl>();

        enemyStates = new EnemyStateBase[transform.childCount - 2]; // stat �迭 �ʱ�ȭ
        for(int i = 0; i < enemyStates.Length; i++)
        {
            enemyStates[i] = transform.GetChild(i).gameObject.GetComponent<EnemyStateBase>();
        }
    }


    /// <summary>
    /// ���¸� �޴� �Լ�
    /// </summary>
    /// <param name="state">���� ���� �Է�</param>
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
