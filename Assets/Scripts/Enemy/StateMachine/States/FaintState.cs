using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Toughness�� 0�̵Ǹ� ����Ǵ� ��ũ��Ʈ
/// </summary>
public class FaintState : EnemyStateBase
{
    public bool isFaint = true;

    float timer = 0;

    public override EnemyStateBase EnterCurrentState()
    {
        timer = 0f;
        isFaint = true;

        enemy.Anim.SetTrigger(enemy.faintToHash); // ���� �ִϸ��̼�
        enemy.Anim.SetBool(enemy.isFaintToHash, true); // ���� bool �ִϸ��̼�

        return this;
    }

    public override EnemyStateBase ExitCurrentState()
    {
        throw new System.NotImplementedException();
    }

    public override EnemyStateBase RunCurrentState()
    {
        timer = Time.deltaTime;

        if(timer > 2f)
        {
            isFaint = false;
        }

        if(!isFaint)
        {
            enemy.Anim.SetBool(enemy.isFaintToHash, false); // ���� bool �ִϸ��̼�
            return enemy.SetEnemyState(EnemyBase.State.Chasing);
        }

        return this;
    }
}
