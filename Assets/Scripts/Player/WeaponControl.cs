using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 무기 콜라이더를 설정하는 스크립트
/// </summary>
public class WeaponControl : MonoBehaviour
{
    Collider coll;

    bool isEnable = false;

    void Awake()
    {
        coll = GetComponent<Collider>();
        coll.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Shield"))
        {
            Debug.Log("공격이 방패에 닿았음 Trigger ");
        }
    }

    /// <summary>
    /// 무기 콜라이더를 활성화, 비활성화 하는 함수 (실행하면 bool값이 전환됨, 초기값: false)
    /// </summary>
    public void ChangeColliderEnableState()
    {
        isEnable = !isEnable;
        coll.enabled = isEnable;
    }
}
