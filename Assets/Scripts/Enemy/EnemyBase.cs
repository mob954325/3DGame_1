using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

/// <summary>
/// 현재 적 상태
/// </summary>
enum State
{
    Idle = 0,
    Attack,
    ReadyToAttack,
    Die,
    Faint // 기절
}

/// <summary>
/// 모든 적이 가질 클래스
/// </summary>
public class EnemyBase : MonoBehaviour, EnemyState
{
    public string currentState; // 상태를 인스펙터에 표시
    State state;
    // 플레이어 따라가기
    // 공격
    // 공격 맞았을 때

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
    /// 현재 상태 보여주는 함수
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
