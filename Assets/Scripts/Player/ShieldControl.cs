using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �ݸ��̴��� �����ϴ� ��ũ��Ʈ
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
