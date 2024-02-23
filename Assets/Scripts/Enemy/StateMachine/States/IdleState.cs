using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ���� ������ �غ� �ð�
/// </summary>
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

    public override EnemyStateBase EnterCurrentState()
    {
        enemy.speed = 0f;

        return this;
    }

    public override EnemyStateBase RunCurrentState()
    {
        timer += Time.deltaTime;

        if(timer >= changeTime)
        {
            Debug.Log("chasingState�� ���� ����");

            return chasingState;
        }

        return this;
    }

    public override EnemyStateBase ExitCurrentState()
    {
        return this;
    }
}
