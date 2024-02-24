using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Toughness가 0이되면 실행되는 스크립트
/// </summary>
public class FaintState : EnemyStateBase
{
    public bool isFaint = true;

    float timer = 0;

    public override EnemyStateBase EnterCurrentState()
    {
        timer = 0f;
        isFaint = true;

        enemy.Anim.SetTrigger(enemy.faintToHash); // 기절 애니메이션
        enemy.Anim.SetBool(enemy.isFaintToHash, true); // 기절 bool 애니메이션

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
            enemy.Anim.SetBool(enemy.isFaintToHash, false); // 기절 bool 애니메이션
            return enemy.SetEnemyState(EnemyBase.State.Chasing);
        }

        return this;
    }
}
