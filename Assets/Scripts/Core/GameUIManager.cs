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

    [Header("Panels")]
    ResultPanel resultPanel;
    [HideInInspector]public bool isPlayerInteraction = false;


    protected override void OnInitialize()
    {
        playerBar = FindAnyObjectByType<PlayerBar>();
        enemyBar = FindAnyObjectByType<EnemyBar>();

        resultPanel = FindAnyObjectByType<ResultPanel>();
    }

    /// <summary>
    /// 결과 패널을 보여주는 함수
    /// </summary>
    public void ShowResult()
    {
        resultPanel.Show(); // 보여주기
        resultPanel.SetResultText(); // 정보저장
    }
}
