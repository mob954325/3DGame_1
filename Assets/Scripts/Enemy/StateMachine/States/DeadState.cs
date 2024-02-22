using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : EnemyStateBase
{
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
        return this;
    }
}
