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
    public GameObject enemyHpBar;
    Image currentEnemyHP;

    public GameObject playerHpBar;
    Image currentPlayerHP;



    void Awake()
    {
        enemy = FindAnyObjectByType<EnemyBase>();
        player = FindAnyObjectByType<Player>();

        // UI
        currentEnemyHP = enemyHpBar.transform.GetChild(1).gameObject.GetComponent<Image>();
        currentPlayerHP = playerHpBar.transform.GetChild(1).gameObject.GetComponent<Image>();
    }

    void Update()
    {
        currentEnemyHP.fillAmount = enemy.HP / (float)enemy.maxHp;
        currentPlayerHP.fillAmount = player.HP / (float)player.maxhp;   
    }
}
