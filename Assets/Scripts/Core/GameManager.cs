using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Player player;
    //private EnemyBase enemy;
    //
    //public EnemyBase Enemy
    //{
    //    get
    //    {
    //        if (enemy == null)
    //        {
    //            enemy = FindAnyObjectByType<EnemyBase>();
    //
    //            GameObject obj = new GameObject();
    //            obj.name = "EnemyBaseObj";
    //            obj.transform.parent = transform;
    //
    //            enemy = obj.AddComponent<EnemyBase>();
    //
    //            obj.SetActive(false);
    //        }
    //
    //        return enemy;
    //    }
    //}
    //
    //protected override void OnInitialize()
    //{
    //    player = FindAnyObjectByType<Player>();
    //
    //    enemy = FindAnyObjectByType<EnemyBase>();
    //    if (enemy == null)
    //    {
    //        GameObject obj = new GameObject();
    //        obj.name = "EnemyBaseObj";
    //        obj.transform.parent = transform;
    //
    //        enemy = obj.AddComponent<EnemyBase>();
    //
    //        obj.SetActive(false);
    //    }
    //}
}
