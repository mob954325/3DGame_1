using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    // Delegate
    Action changeAttackState; // �÷��̾����� ������ �ϴ��� Ȯ���ϴ� ��������Ʈ

    // components
    public Player player;
    Rigidbody rigid;
    Animator animator;

    // Values
    Vector3 direction = Vector3.zero;
    float lookAngle;
    float attackAnimTime;

    // Flags
    bool isAttack = false;

    /// <summary>
    /// ���� �ߴ��� Ȯ���ϴ� �Ķ����
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
            Debug.Log($"���� ü���� [{hp}]��ŭ ���ҽ��ϴ�");

            if (hp <= 0)
            {
                hp = 0;
                Die();
            }
        }
    }

    // Hashes
    readonly int SpeedToHash = Animator.StringToHash("Speed");
    readonly int AttackToHash = Animator.StringToHash("Attack");
    readonly int DamagedToHash = Animator.StringToHash("Damaged");

    void Awake()
    {
        player = FindAnyObjectByType<Player>();
        animator = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();

        // setting values
        curSpeed = speed; // speed �� ����
        hp = maxHp;

        // add function to delegate
        changeAttackState += () => player.ChangeAttackFlag();
    }

    void FixedUpdate()
    {
        direction = player.transform.position - transform.position; // �÷��̾� ���� ����
        animator.SetFloat(SpeedToHash, speed);

        MoveToPlayer();
        RotateToPlayer();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PlayerAttack"))
        {
            Debug.Log("Attacked by Player !!");
            animator.SetTrigger(DamagedToHash);
            HP--;
        }
    }

    /// <summary>
    /// �÷��̾����� �̵��ϴ� �Լ�
    /// </summary>
    void MoveToPlayer()
    {
        if(direction.magnitude > attackRange)
        {
            rigid.MovePosition(rigid.position + Time.fixedDeltaTime * direction.normalized * speed);
        }
        else if(direction.magnitude <= attackRange) // �÷��̾� ��ó�� ����
        {
            if(!IsAttack)
            {
                StopAllCoroutines();
                StartCoroutine(Attack());
            }
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

    /// <summary>
    /// �÷��̾ �����ϴ� �ڷ�ƾ (AttackDelay�ð����� ��� �Լ��� �ҷ���)
    /// </summary>
    /// <returns></returns>
    IEnumerator Attack()
    {
        // ���� �ִϸ��̼� ����
        animator.SetTrigger(AttackToHash);
        IsAttack = true;
        attackAnimTime = animator.GetCurrentAnimatorStateInfo(0).length + 0.5f; // ���� ��� �ִϸ��̼� ����ð�
        changeAttackState?.Invoke();
        yield return new WaitForSeconds(attackAnimTime);
        changeAttackState?.Invoke();

        // �ڷ� ��������
        StepBackTime = UnityEngine.Random.Range(1, attackDelay - attackAnimTime); // �ڷ� �������� ���� �ð�
        speed = (speed * -1) / 2;
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * direction * speed);
        yield return new WaitForSeconds(StepBackTime);

        // ����
        speed = 0f;
        yield return new WaitForSeconds(attackDelay - attackAnimTime - StepBackTime);

        // ���� ������ ��
        IsAttack = false;
    }

    void Die()
    {
        Debug.Log("���� ����߽��ϴ�.");
    }
}
