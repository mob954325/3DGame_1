using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾ �����ϴ� ����
/// </summary>
public class AttackState : EnemyStateBase
{
    public ChasingState chasingState;

    public bool isAttack = true;

    public override EnemyStateBase EnterCurrentState()
    {
        isAttack = true;
        enemy.speed = 0f;

        StartCoroutine(AttackCombo()); // ���� ����
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
