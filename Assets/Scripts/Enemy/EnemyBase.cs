using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    // Delegate
    /// <summary>
    /// ���ݽ� �����ϴ� ��������Ʈ
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

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        player = FindAnyObjectByType<Player>();
        animator = GetComponent<Animator>();
        weapon = GetComponentInChildren<WeaponControl>();
    }

    void FixedUpdate()
    {
        direction = player.transform.position - transform.position; // �÷��̾� ���� ����
        //animator.SetFloat(SpeedToHash, speed);

        MoveToPlayer();
        RotateToPlayer();
    }

    /// <summary>
    /// �÷��̾����� �̵��ϴ� �Լ� ( AttackRange ���� �����ϸ� ���� )
    /// </summary>
    void MoveToPlayer()
    {
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * direction.normalized * speed);

        if (direction.magnitude <= attackRange) // �÷��̾� ��ó�� ����
        {
            isAttack = true;
        }
    }

    /// <summary>
    /// �÷��̾ ���� ȸ���ϴ� �Լ�
    /// </summary>
    void RotateToPlayer()
    {
        Vector3 rotDirection = Vector3.zero;
        rotDirection.x = direction.x;
        rotDirection.z = direction.z;
        rotDirection.Normalize();


        if (rotDirection.magnitude > 0.01f)
        {
            lookAngle = Mathf.Atan2(rotDirection.x, rotDirection.z) * Mathf.Rad2Deg; // ȸ���� ����
        }
        float angle = Mathf.LerpAngle(transform.localRotation.eulerAngles.y, lookAngle, rotateSpeed * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(0, angle, 0); // rotate Player model
    }
}
