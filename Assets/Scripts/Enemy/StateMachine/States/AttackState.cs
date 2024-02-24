using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾ �����ϴ� ����
/// </summary>
public class AttackState : EnemyStateBase
{
    public bool isAttack = true;
    bool isBlock = false;

    public override EnemyStateBase EnterCurrentState()
    {
        isBlock = false;
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
        if (enemy.isAttackBlocked && !isBlock)
        {
            isBlock = true;
            OnPlayerParrying();

            if(enemy.Toughness == 0)
                return enemy.SetEnemyState(EnemyBase.State.Faint);
        }

        if (!isAttack)
            return enemy.SetEnemyState(EnemyBase.State.Chasing);

        return this;
    }

    IEnumerator AttackCombo()
    {
        enemy.Anim.SetTrigger(enemy.AttackToHash);
        yield return new WaitForSeconds(2f);
        isAttack = false;
    }

    void OnPlayerParrying()
    {
        enemy.Toughness -= 20;
        enemy.Anim.SetTrigger(enemy.DamagedToHash);
    }
}
