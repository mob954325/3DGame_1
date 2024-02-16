using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �ݶ��̴��� �����ϴ� ��ũ��Ʈ
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
            Debug.Log("������ ���п� ����� Trigger ");
        }
    }

    /// <summary>
    /// ���� �ݶ��̴��� Ȱ��ȭ, ��Ȱ��ȭ �ϴ� �Լ� (�����ϸ� bool���� ��ȯ��, �ʱⰪ: false)
    /// </summary>
    public void ChangeColliderEnableState()
    {
        isEnable = !isEnable;
        coll.enabled = isEnable;
    }
}
