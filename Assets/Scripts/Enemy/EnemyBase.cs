using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    //�Ϸ�
    //1. player tag�� ���� ������Ʈ ã�� 
    //2. �ش� ������Ʈ���� �ٰ����� : ���� �����Ÿ� ������ 
    //3. �����ϱ� -> ��� -> ���� ������ �۵� 

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

        curSpeed = speed; // speed �� ����
    }

    void FixedUpdate()
    {
        direction = player.transform.position - transform.position; // �÷��̾� ���� ����
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
    /// �÷��̾����� �̵��ϴ� �Լ�
    /// </summary>
    void MoveToPlayer()
    {
        if(direction.magnitude > range)
        {
            rigid.MovePosition(rigid.position + Time.fixedDeltaTime * direction.normalized * speed);
        }
        else if(direction.magnitude <= range) // �÷��̾� ��ó�� ����
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
    /// �÷��̾ ������ �ڷ�ƾ (AttackDelay�ð����� ��� �Լ��� �ҷ���)
    /// </summary>
    /// <returns></returns>
    IEnumerator Attack()
    {
        // ���� �ִϸ��̼� ����
        anim.SetTrigger(AttackToHash);
        IsAttack = true;
        attackAnimTime = anim.GetCurrentAnimatorStateInfo(0).length + 0.5f; // ���� ��� �ִϸ��̼� ����ð�
        yield return new WaitForSeconds(attackAnimTime);

        // �ڷ� ��������
        StepBackTime = Random.Range(1, attackDelay - attackAnimTime); // �ڷ� �������� ���� �ð�
        speed = (speed * -1) / 2;
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * direction * speed);
        yield return new WaitForSeconds(StepBackTime);

        // ����
        speed = 0f;
        yield return new WaitForSeconds(attackDelay - attackAnimTime - StepBackTime);

        // ���� ������ ��
        IsAttack = false;
    }
}
