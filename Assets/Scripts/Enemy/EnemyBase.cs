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
    Player player;
    Rigidbody rigid;
    Animator anim;

    // Values
    Vector3 direction = Vector3.zero;
    float lookAngle;

    // Flags
    bool isAttack = false;

    // enemy info
    float curSpeed;
    public float speed = 3.0f;
    public float range = 2.0f;
    public float rotateSpeed = 5.0f;
    public float attackDelay = 2.5f;


    // Hashes
    readonly int ForwardToHash = Animator.StringToHash("Move");
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
        MoveToPlayer();
        RotateToPlayer();
        checkAttacking();
    }

    /// <summary>
    /// �÷��̾����� �̵��ϴ� �Լ�
    /// </summary>
    void MoveToPlayer()
    {
        if(direction.magnitude > range)
        {
            rigid.MovePosition(rigid.position + Time.fixedDeltaTime * direction * speed);
            anim.SetBool(ForwardToHash, true);
        }
        else if(direction.magnitude <= range) // �÷��̾� ��ó�� ����
        {
            anim.SetBool(ForwardToHash, false);
            if(!isAttack)
            {
                StopAllCoroutines();
                StartCoroutine(Attack());
            }
        }
    }

    void checkAttacking()
    {
        if(isAttack)
        {
            speed = 0f;
        }
        else
        {
            speed = curSpeed;
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
        anim.SetTrigger(AttackToHash);
        isAttack = true;
        yield return new WaitForSeconds(attackDelay);
        // ������ ���� ���� �ֱ�
        isAttack = false;
    }
}
