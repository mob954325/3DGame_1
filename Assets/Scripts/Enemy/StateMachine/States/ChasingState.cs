using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾ ���󰡴� ����
/// </summary>
public class ChasingState : EnemyStateBase
{
    public AttackState attackState;

    public override EnemyStateBase EnterCurrentState()
    {
        // �ڷ� ��������
        Debug.Log("chasing enter");
        enemy.speed = enemy.baseSpeed;
        return this;
    }



    public override EnemyStateBase RunCurrentState()
    {
        enemy.direction = enemy.Player.transform.position - transform.position; // �÷��̾� ���� ����
        enemy.Anim.SetFloat(enemy.SpeedToHash, enemy.speed); // �̵� �ִϸ��̼�
        MoveToPlayer();
        RotateToPlayer();

        if (enemy.direction.magnitude <= enemy.attackRange) // �÷��̾� ��ó�� ����
        {
            Debug.Log("Attack���� ��ȯ");
            return attackState;
        }

        return this;
    }
    public override EnemyStateBase ExitCurrentState()
    {
        return this;
    }

    /// <summary>
    /// �÷��̾����� �̵��ϴ� �Լ�
    /// </summary>
    void MoveToPlayer()
    {
        enemy.Rigid.MovePosition(enemy.Rigid.position + Time.fixedDeltaTime * enemy.direction.normalized * enemy.speed);
    }

    /// <summary>
    /// �÷��̾ ���� ȸ���ϴ� �Լ�
    /// </summary>
    void RotateToPlayer()
    {
        Vector3 rotDirection = Vector3.zero;
        rotDirection.x = enemy.direction.x;
        rotDirection.z = enemy.direction.z;
        rotDirection.Normalize();

        if (rotDirection.magnitude > 0.01f)
        {
            enemy.lookAngle = Mathf.Atan2(rotDirection.x, rotDirection.z) * Mathf.Rad2Deg; // ȸ���� ����
        }

        float angle = Mathf.LerpAngle(enemy.transform.localRotation.eulerAngles.y, enemy.lookAngle, enemy.rotateSpeed * Time.fixedDeltaTime);
        enemy.transform.localRotation = Quaternion.Euler(0, angle, 0); // rotate Player model
    }
}
