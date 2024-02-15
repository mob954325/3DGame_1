using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI���� ��ũ��Ʈ
/// </summary>
public class GameUIManager : Singleton<GameUIManager>
{
    [Header("Gauge UI")]
    PlayerBar playerBar;
    EnemyBar enemyBar;

    protected override void OnInitialize()
    {
        playerBar = FindAnyObjectByType<PlayerBar>();
        enemyBar = FindAnyObjectByType<EnemyBar>();
    }
}
