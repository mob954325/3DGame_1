using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Delegate
    public Action onDamaged;

    // Player Stats
    public int Maxhp = 5;
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

    // flags
    bool isDamaged = false;
    bool isEnemyAttack = false; // ������ �����ϴ��� Ȯ���ϴ� boolean

    void Awake()
    {
        hp = Maxhp;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("EnemyAttack") && !isDamaged && !isEnemyAttack)
        {
            onDamaged?.Invoke(); // ���� ��������Ʈ �Լ� ����
            HP--;
            StartCoroutine(HitDelay());
        }
    }

    IEnumerator HitDelay()
    {
        isDamaged = true;
        yield return new WaitForSeconds(2f);
        isDamaged = false;
    }

    /// <summary>
    /// �÷��̾� ��� �Լ�
    /// </summary>
    void Die()
    {
        Debug.Log($"�÷��̾ ����߽��ϴ�.");
    }

    /// <summary>
    /// ���� �÷��̾����� ������ �� �� �ִ��� ������ ���� ��ȯ�ϴ� �Լ�(true : ���� ���� , false : ���� �Ұ�)
    /// </summary>
    public void ChangeAttackFlag()
    {
        isEnemyAttack = !isEnemyAttack;
    }
}
