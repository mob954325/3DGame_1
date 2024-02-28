using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// EnemyBarObject에 들어갈 스크립트
/// </summary>
public class EnemyBar : BaseGauge
{
    // Enemy
    EnemyBase enemy;

    public GameObject currentEnemyHPObj;
    public GameObject currentEnemyToughnessObj;

    public Image currentEnemyHP;
    public Image currentEnemyToughness;

    public Transform hpChild;
    public Transform toughChild;


    protected override void Init()
    {
    }

    protected override void UpdateUI()
    {
        if (GameManager.Instance.GetIsBattle())
        {
            enemy = FindAnyObjectByType<EnemyBase>(); // enemy 스크립트가 있는 오브젝트 찾기

            hpChild = transform.GetChild(0);
            toughChild = transform.GetChild(1);

            currentEnemyHPObj = hpChild.gameObject;
            currentEnemyToughnessObj = toughChild.gameObject;

            currentEnemyHP = hpChild.GetChild(1).GetComponent<Image>();
            currentEnemyToughness = toughChild.GetChild(1).GetComponent<Image>();


            if (enemy != null)
            {
                currentEnemyHPObj.SetActive(true);
                currentEnemyToughnessObj.SetActive(true);

                if (enemy.HP < 1) // 적이 사망했거나 없으면
                {
                    gameObject.SetActive(false); // 숨기기
                }

            }
            currentEnemyHP.fillAmount = enemy.HP / (float)enemy.maxHp;
            currentEnemyToughness.fillAmount = enemy.Toughness / (float)enemy.maxToughness;
        }
    }
}
