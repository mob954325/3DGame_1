using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Component
    Animator animator;
    // Delegate
    public Action onDamaged;

    // Player Stats
    public int maxhp = 5;
    int hp;
    public int HP // �÷��̾� ü�� ������Ƽ
    {
        get => hp;
        set
        {
            hp = value;
            Debug.Log($"�÷��̾��� ü���� [{hp}]��ŭ ���ҽ��ϴ�");

            if(hp <= 0)
            {
                hp = 0;
                Die();
            }
        }
    }

    // Player Parameters
    public float defenceTime = 0f;

    // Flags
    bool isDamaged = false;
    bool isEnemyAttack = false; // ������ �����ϴ��� Ȯ���ϴ� boolean
    /// <summary>
    /// PlayerController.cs���� isDefence�� �Ű������� BroadCast�� �޴� �Լ�
    /// </summary>
    /// <param name="boolean">PlayerController.cs�� isDefence flag ����</param>
    bool GetCanDefence(bool canDefence) => this.canDefence = canDefence;
    bool canDefence = false;
    bool GetIsDefence(bool isDefence) => this.isDefence = isDefence;
    bool isDefence = false;

    void Awake()
    {
        animator = GetComponent<Animator>();

        hp = maxhp;
    }

    void Update()
    {
        if(isDefence)
        {
            defenceTime += Time.deltaTime;
        }
        else
        {
            defenceTime = 0f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("EnemyAttack") &&
          !isDamaged && isEnemyAttack && !canDefence)
        {
            onDamaged?.Invoke(); // ���� ��������Ʈ �Լ� ����
            StartCoroutine(HitDelay());
        }
    }

    IEnumerator HitDelay()
    {
        isDamaged = true;


        HP--;
        yield return new WaitForSeconds(2f);
        isDamaged = false;
    }

    /// <summary>
    /// �÷��̾� ��� �Լ�
    /// </summary>
    void Die()
    {
        animator.SetTrigger("Die");
        Debug.Log($"�÷��̾ ����߽��ϴ�.");
    }

    /// <summary>
    /// ���� �÷��̾����� ������ �� �� �ִ��� ������ ���� ��ȯ�ϴ� �Լ�(true : ���� ���� , false : ���� �Ұ�)
    /// </summary>
    public void Player_ChangeAttackFlag()
    {
        isEnemyAttack = !isEnemyAttack;
    }

    public float GetDefenceTime()
    {
        return defenceTime;
    }
}
