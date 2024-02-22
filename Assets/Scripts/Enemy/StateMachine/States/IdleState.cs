using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : EnemyStateBase
{
    [SerializeField] ChasingState chasingState;

    [Tooltip("���� ����(Chasing)�� ��ȯ�� ������ ��� �ð�")]
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
            Debug.Log("idle���� ���� ����");
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
