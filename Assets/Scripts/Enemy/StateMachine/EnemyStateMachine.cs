using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    /// <summary>
    /// 현재 상태 스크립트 ( Null이면 행동을 안함 )
    /// </summary>
    public EnemyStateBase currentState;

    void Update()
    {
        RunStateMachine();
    }

    /// <summary>
    /// 해당 State에서 실행할 내용
    /// </summary>
    private void RunStateMachine()
    {
        EnemyStateBase nextState = currentState?.RunCurrentState();

        if(nextState != null)
        {
            ChangeStateMachine(nextState);
        }
    }

    /// <summary>
    /// CurrentState를 nextState로 바꾸는 함수
    /// </summary>
    /// <param name="nextState">바뀔 state이름</param>
    private void ChangeStateMachine(EnemyStateBase nextState)
    {
        //if (currentState != nextState)
           // currentState?.ExitCurrentState();

        currentState = nextState; // 다음 state로 변경
        currentState.enemy = GetComponent<EnemyBase>(); // state의 enemy 컴포넌트 받기

        //if (currentState != null)
            //currentState?.EnterCurrentState();
    }
}
