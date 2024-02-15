using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI관리 스크립트
/// </summary>
public class GameUIManager : Singleton<GameUIManager>
{
    [Header("Gauge UI")]
    PlayerBar playerBar;
    EnemyBar enemyBar;

    [Header("Info UI")]
    [HideInInspector]public UI_Info info;
    [HideInInspector]public bool isPlayerInteraction = false;


    protected override void OnInitialize()
    {
        playerBar = FindAnyObjectByType<PlayerBar>();
        enemyBar = FindAnyObjectByType<EnemyBar>();

        info = FindAnyObjectByType<UI_Info>();
    }
}
