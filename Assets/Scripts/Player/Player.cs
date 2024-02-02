using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("EnemyAttack"))
        {
            Debug.Log($"적의 공격을 받았습니다 !!!");
        }
    }
}
