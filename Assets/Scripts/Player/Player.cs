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
    public int HP // 플레이어 체력 프로퍼티
    {
        get => hp;
        set
        {
            hp = value;
            Debug.Log($"플레이어의 체력이 [{hp}]만큼 남았습니다");

            if(hp <= 0)
            {
                hp = 0;
                Die();
            }
        }
    }

    // flags
    bool isDamaged = false;
    bool isEnemyAttack = false; // 상대방이 공격하는지 확인하는 boolean

    void Awake()
    {
        hp = Maxhp;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("EnemyAttack") && !isDamaged && !isEnemyAttack)
        {
            onDamaged?.Invoke(); // 공격 델리게이트 함수 실행
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
    /// 플레이어 사망 함수
    /// </summary>
    void Die()
    {
        Debug.Log($"플레이어가 사망했습니다.");
    }

    /// <summary>
    /// 적이 플레이어한테 공격을 할 수 있는지 없는지 상태 전환하는 함수(true : 공격 가능 , false : 공격 불가)
    /// </summary>
    public void ChangeAttackFlag()
    {
        isEnemyAttack = !isEnemyAttack;
    }
}
