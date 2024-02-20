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

    [Header("Panels")]
    ResultPanel resultPanel;
    [HideInInspector]public UI_Info info;
    [HideInInspector]public bool isPlayerInteraction = false;


    protected override void OnInitialize()
    {
        playerBar = FindAnyObjectByType<PlayerBar>();
        enemyBar = FindAnyObjectByType<EnemyBar>();

        info = FindAnyObjectByType<UI_Info>();
        resultPanel = FindAnyObjectByType<ResultPanel>();
    }

    /// <summary>
    /// ��� �г��� �����ִ� �Լ�
    /// </summary>
    public void ShowResult()
    {
        resultPanel.Show(); // �����ֱ�
        resultPanel.SetResultText(); // ��������
    }
}
