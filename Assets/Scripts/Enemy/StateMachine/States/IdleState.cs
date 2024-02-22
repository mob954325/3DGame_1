using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : EnemyStateBase
{
    [SerializeField] ChasingState chasingState;

    [Tooltip("다음 상태(Chasing)로 전환될 때까지 대기 시간")]
    public float changeTime;

    public float timer;

    void Awake()
    {
        chasingState = FindAnyObjectByType<ChasingState>();
    }

    public override EnemyStateBase RunCurrentState()
    {
        timer += Time.deltaTime;

        if(timer >= changeTime)
        {
            Debug.Log("idle에서 상태 변경");
            ExitCurrentState();

            return chasingState;
        }

        return this;
    }

    public override EnemyStateBase EnterCurrentState()
    {
        Debug.Log("Idle Enter");

        return this;
    }

    public override EnemyStateBase ExitCurrentState()
    {
        Debug.Log("Idle exit");

        return this;
    }
}
