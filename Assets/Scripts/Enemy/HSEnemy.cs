using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HSEnemy : MonoBehaviour
{
    // Delegate
    /// <summary>
    /// ���ݽ� �����ϴ� ��������Ʈ
    /// </summary>
    Action onAttack;

    // Components
    Player player;
    WeaponControl weapon;
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
    public float parryingChanceTime = 0.5f; // �� ������ ���� �и� ������ ��� �ð� ��
    [SerializeField] float StepBackTime = 0f;

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

    int toughness = 0; // ���μ� (0�̵Ǹ� ����)
    public int Toughness
    {
        get => toughness;
        set
        {
            toughness = value;
            //Debug.Log($"���� ���μ� : [{toughness}]");

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

            if (!isAttack)
            {
                speed = baseSpeed;
            }
        }
    }
    bool isDamaged = false; // �ǰ� ����
    [SerializeField] bool isDead => HP <= 0; // ��� ����
    [SerializeField] bool isFaint => Toughness <= 0; // ���� ���� 
    //float playerDefenceTIme = 0f; // player�� ����� �ð� ���� ����
    //bool alreadyParrying = false; // �̹� �и����ߴ��� Ȯ���ϴ� ����

    void Awake()
    {
        player = FindAnyObjectByType<Player>();
        animator = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        weapon = GetComponentInChildren<WeaponControl>();

        // setting values
        baseSpeed = speed; // speed �� ����
        HP = maxHp;
        Toughness = maxToughness;

        onAttack += weapon.ChangeColliderEnableState;
    }

    void FixedUpdate()
    {
        direction = player.transform.position - transform.position; // �÷��̾� ���� ����
        animator.SetFloat(SpeedToHash, speed);

        // �� �ൿ �Լ���
        if(!isDead && !isFaint)
        {
            MoveToPlayer();
            RotateToPlayer();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isDead)
            return;

        if(other.CompareTag("PlayerAttack") && !isDamaged)
        {
            HP--;
            animator.SetTrigger(DamagedToHash);
            StartCoroutine(HitDelay());

            //if (isFaint) return;
            //animator.SetTrigger(DamagedToHash);
        }
    }

    /// <summary>
    /// �÷��̾����� �̵��ϴ� �Լ�
    /// </summary>
    void MoveToPlayer()
    {
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * direction.normalized * speed);

        if(direction.magnitude <= attackRange) // �÷��̾� ��ó�� ����
        {
            if (!IsAttack)
            {
                //alreadyParrying = false; // �и� ���� Ȱ��ȭ

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
        speed = 0f;
        // ���� �ִϸ��̼� ����
        animator.SetTrigger(AttackToHash); // ���� �ִϸ��̼� �÷���

        IsAttack = true;
        onAttack?.Invoke(); // ���� �ݶ��̴� Ȱ��ȭ

        attackAnimTime = GetAnimClipLength("1HSEnemy_Attack"); // �����ؾ���
        Debug.Log(attackAnimTime);
        yield return new WaitForSeconds(attackAnimTime);

        onAttack?.Invoke(); // ���� �ݶ��̴� ��Ȱ��ȭ

        // �ڷ� ��������
        StepBackTime = UnityEngine.Random.Range(1, attackDelay - attackAnimTime); // �ڷ� �������� ���� �ð�
        yield return new WaitForSeconds(1f);

        speed = baseSpeed * -1 / 2f;
        yield return new WaitForSeconds(StepBackTime);

        // ����
        speed = 0f;
        yield return new WaitForSeconds(attackDelay - attackAnimTime - StepBackTime);

        // ���� ������ ��
        IsAttack = false;
    }

    /// <summary>
    /// �ǰ� ���� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator HitDelay()
    {
        isDamaged = true;
        yield return new WaitForSeconds(0.7f);
        isDamaged = false;
    }

    /// <summary>
    /// ������ ����Ǵ� �Լ�
    /// </summary>
    void Die()
    {
        animator.SetTrigger(DieToHash);
    }

/*    /// <summary>
    /// �÷��̾ �и��� �������� Ȯ���ϴ� �Լ�
    /// </summary>
    public void CheckParrying()
    {
        playerDefenceTIme = player.GetDefenceTime();

        if (IsAttack && playerDefenceTIme > 0 
            && playerDefenceTIme <= parryingChanceTime
            && direction.magnitude <= attackRange
            && !alreadyParrying)
        {
            alreadyParrying = true;
            // ���� ����
            StopCoroutine(Attack());
            Toughness -= 20;
            animator.SetTrigger(DamagedToHash); // ���� ����
        }
    }*/

    /// <summary>
    /// ���� �� ������ �Լ�
    /// </summary>
    void AfterFaint()
    {
        Toughness = maxToughness;
        animator.SetBool(isFaintToHash, isFaint);
        speed = baseSpeed;
    }

    /// <summary>
    /// Ư�� �ִϸ��̼� �ð��� ã�� �Լ�
    /// </summary>
    /// <param name="clipName">ã�� �ִϸ��̼� �̸�</param>
    /// <returns>clipName �ִϸ��̼� �ð�</returns>
    float GetAnimClipLength(string clipName)
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
