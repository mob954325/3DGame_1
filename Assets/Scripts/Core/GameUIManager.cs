using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// UI관리 스크립트
/// </summary>
public class GameUIManager : Singleton<GameUIManager>
{
    [Header("Gauge UI")]
    public PlayerBar playerBar;
    public EnemyBar enemyBar;
    public EnemyBar EnemyBar => enemyBar;

    [Header("Panels")]
    ResultPanel resultPanel;
    [HideInInspector]public bool isPlayerInteraction = false;


    protected override void OnInitialize()
    {
        playerBar = FindAnyObjectByType<PlayerBar>();
        enemyBar = FindAnyObjectByType<EnemyBar>();

        resultPanel = FindAnyObjectByType<ResultPanel>();

        Cursor.lockState = CursorLockMode.None; // 커서 고정해제
        Cursor.visible = true; // 커서 보여주기
    }

    /// <summary>
    /// 결과 패널을 보여주는 함수
    /// </summary>
    public void ShowResult(bool isPlayer = false)
    {
        Cursor.lockState = CursorLockMode.None; // 커서 고정해제
        Cursor.visible = true; // 커서 보여주기
        resultPanel.SetResultText(isPlayer); // 정보저장
        resultPanel.Show(); // 보여주기
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void EndGame()
    {
        SceneManager.LoadScene(0);
    }
}
