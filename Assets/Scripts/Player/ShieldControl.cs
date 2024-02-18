using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 방패 콜리이더를 설정하는 스크립트
/// </summary>
public class ShieldControl : MonoBehaviour
{
    Collider coll;

    bool isEnable = false;

    void Awake()
    {
        coll = GetComponent<Collider>();
    }

    public void ChangeColliderEnableState()
    {
        isEnable = !isEnable;
        coll.enabled = isEnable;
    }
}
