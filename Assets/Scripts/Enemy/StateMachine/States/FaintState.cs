using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaintState : EnemyStateBase
{
    public ChasingState chasingState;

    public bool isFaint = true;

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
        Debug.Log("faint");
        if(!isFaint)
        {
            return chasingState;
        }

        return this;
    }
}
