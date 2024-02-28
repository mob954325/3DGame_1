using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : EnemyStateBase
{
    public override EnemyStateBase EnterCurrentState()
    {
        enemy.Anim.SetTrigger(enemy.DieToHash);

        if(enemy.type == EnemyBase.Type.Boss)
        {
            GameManager.Instance.BattleEnd(); // 게임 타이머 정지
            GameManager.Instance.player.DisablePlayerAction(); // 플레이어 입력 비활성화
            GameUIManager.Instance.ShowResult();
        }

        StartCoroutine(DisableObj());

        return this;
    }

    public override EnemyStateBase ExitCurrentState()
    {
        return this;
    }

    public override EnemyStateBase RunCurrentState()
    {
        return this;
    }

    IEnumerator DisableObj()
    {
        float time = enemy.GetAnimClipLength("Death");
        yield return new WaitForSeconds(time + 0.5f);
        enemy.gameObject.SetActive(false);
    }
}
