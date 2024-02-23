using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어를 공격하는 상태
/// </summary>
public class AttackState : EnemyStateBase
{
    public ChasingState chasingState;

    public bool isAttack = true;

    public override EnemyStateBase EnterCurrentState()
    {
        isAttack = true;
        enemy.speed = 0f;

        StartCoroutine(AttackCombo()); // 공격 실행
        return this;
    }

    public override EnemyStateBase ExitCurrentState()
    {
        return this;
    }

    public override EnemyStateBase RunCurrentState()
    {
        if(!isAttack)
        {
            enemy.speed = enemy.baseSpeed;
            return chasingState;
        }
        return this;
    }

    IEnumerator AttackCombo()
    {
        yield return new WaitForSeconds(2f);
        isAttack = false;
    }
}
