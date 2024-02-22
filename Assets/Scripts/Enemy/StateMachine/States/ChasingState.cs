using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingState : EnemyStateBase
{
    public AttackState attackState;

    bool isAttack => enemy.isAttack;

    void Start()
    {
        EnterCurrentState();
    }

    public override EnemyStateBase EnterCurrentState()
    {
        Debug.Log("chasing enter");
        return this;
    }

    public override EnemyStateBase ExitCurrentState()
    {
        throw new System.NotImplementedException();
    }

    public override EnemyStateBase RunCurrentState()
    {
        //if (enemy != null)
        //{
        //    Debug.Log("¼º°ø");
        //}
        Debug.Log("chasing perform");

        if (isAttack)
        {
            enemy.speed = 0f;
            //return attackState;
        }

        return this;
    }
}
