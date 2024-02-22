using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

/// <summary>
/// ���� �� ����
/// </summary>
enum State
{
    Idle = 0,
    Attack,
    ReadyToAttack,
    Die,
    Faint // ����
}

/// <summary>
/// ��� ���� ���� Ŭ����
/// </summary>
public class EnemyBase : MonoBehaviour, EnemyState
{
    public string currentState; // ���¸� �ν����Ϳ� ǥ��
    State state;
    // �÷��̾� ���󰡱�
    // ����
    // ���� �¾��� ��

    protected virtual void Awake()
    {
        state = State.Idle;
    }

    protected virtual void FixedUpdate()
    {
        
    }

    void LateUpdate()
    {
        ShowState();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        
    }

    /// <summary>
    /// ���� ���� �����ִ� �Լ�
    /// </summary>
    void ShowState()
    {
        switch (state)
        {
            case State.Idle:
                currentState = "Idle";
                break;
            case State.Attack:
                currentState = "Attack";
                break;
            case State.ReadyToAttack: 
                currentState = "ReadyToAttack";
                break;
            case State.Die: 
                currentState = "Die";
                break;
            case State.Faint: 
                currentState = "Faint";
                break;
        }
    }

    public void BeforeAction()
    {
        throw new System.NotImplementedException();
    }

    public void AfterAction()
    {
        throw new System.NotImplementedException();
    }

    public void InAction()
    {
        throw new System.NotImplementedException();
    }
}
