using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    /// <summary>
    /// ���� ���� ��ũ��Ʈ ( Null�̸� �ൿ�� ���� )
    /// </summary>
    public EnemyStateBase currentState;

    void Update()
    {
        RunStateMachine();
    }

    /// <summary>
    /// �ش� State���� ������ ����
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
    /// CurrentState�� nextState�� �ٲٴ� �Լ�
    /// </summary>
    /// <param name="nextState">�ٲ� state�̸�</param>
    private void ChangeStateMachine(EnemyStateBase nextState)
    {
        //if (currentState != nextState)
           // currentState?.ExitCurrentState();

        currentState = nextState; // ���� state�� ����
        currentState.enemy = GetComponent<EnemyBase>(); // state�� enemy ������Ʈ �ޱ�

        //if (currentState != null)
            //currentState?.EnterCurrentState();
    }
}
