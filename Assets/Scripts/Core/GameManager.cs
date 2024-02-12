using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Characters in Games
    public Player player;
    public EnemyBase enemy;

    // UI
    [Header("UI")]
    [Header("Enemy")]
    public GameObject enemyHpBar;
    Image currentEnemyHP;

    public GameObject enemyToughnessBar;
    Image currentPlayerToughness;

    [Space (10f)]
    [Header("Player")]
    public GameObject playerHpBar;
    Image currentPlayerHP;



    void Awake()
    {
        enemy = FindAnyObjectByType<EnemyBase>();
        player = FindAnyObjectByType<Player>();

        // UI
        currentEnemyHP = enemyHpBar.transform.GetChild(1).gameObject.GetComponent<Image>();
        currentPlayerToughness = enemyToughnessBar.transform.GetChild(1).gameObject.GetComponent<Image>();
        currentPlayerHP = playerHpBar.transform.GetChild(1).gameObject.GetComponent<Image>();
    }

    void Update()
    {
        BarUI();
    }

    /// <summary>
    /// Bar UI ¸ðÀ½
    /// </summary>
    void BarUI()
    {
        // Enemy
        currentEnemyHP.fillAmount = enemy.HP / (float)enemy.maxHp;
        currentPlayerToughness.fillAmount = enemy.Toughness / (float)enemy.maxToughness;   

        // Player
        currentPlayerHP.fillAmount = player.HP / (float)player.maxhp;
    }
}
