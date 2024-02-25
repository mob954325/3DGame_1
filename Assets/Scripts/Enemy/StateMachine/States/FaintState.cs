using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Toughness�� 0�̵Ǹ� ����Ǵ� ��ũ��Ʈ
/// </summary>
public class FaintState : EnemyStateBase
{
    float timer = 0;
    float faintTime = 2f; // 24.02.25 - �ִϸ��̼� �ð��� ���� 

    public override EnemyStateBase EnterCurrentState()
    {
        timer = 0f;

        enemy.Anim.SetTrigger(enemy.faintToHash); // ���� �ִϸ��̼�
        enemy.Anim.SetBool(enemy.isFaintToHash, true); // ���� bool �ִϸ��̼�

        return this;
    }

    public override EnemyStateBase ExitCurrentState()
    {
        return this;
    }

    public override EnemyStateBase RunCurrentState()
    {
        timer += Time.deltaTime;

        if(timer > faintTime)
        {
            enemy.Anim.SetBool(enemy.isFaintToHash, false); // ���� bool �ִϸ��̼�
            return enemy.SetEnemyState(EnemyBase.State.Chasing);
        }

        return this;
    }
}
