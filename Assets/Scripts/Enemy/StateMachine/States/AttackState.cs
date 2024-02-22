using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyStateBase
{
    public ChasingState chasingState;

    public bool isAttack => enemy.isAttack;

    public override EnemyStateBase EnterCurrentState()
    {
        throw new System.NotImplementedException();
    }

    public override EnemyStateBase ExitCurrentState()
    {
        throw new System.NotImplementedException();
    }

    public override EnemyStateBase RunCurrentState()
    {
        StartCoroutine(AttackCombo());
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
        enemy.isAttack = false;
    }
}
