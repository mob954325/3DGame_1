using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // �̱���
    // 1. ���� -> �ε� �� �� ����?
    // 2. �ʱ�ȭ
    // - ���� �̱����� ������ �ӽ� ������Ʈ�� �����ؼ� �̱��� ������Ʈ ����
    // 3. ������ ����Ǹ� ����

    private static T instance;

    /// <summary>
    /// �̱��� ��ü�� �б� ���� ������Ƽ
    /// </summary>
    public static T Instance
    {
        get
        {
            if(instance = null)
            {
                T singleton = FindAnyObjectByType<T>(); // �̱��� ������Ʈ ã��

                if(singleton == null) // ������ �ӽ� ������Ʈ ����
                {
                    GameObject obj = new GameObject();
                    obj.name = "Singleton";
                    obj.AddComponent<Singleton<T>>();
                }

                instance = singleton; 
                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    // flag

    /// <summary>
    /// �̱����� �ʱ�ȭ �Ǿ��ִ��� Ȯ���ϴ� bool
    /// </summary>
    bool isInitionalize = false;

    void Awake()
    {
        if(instance == null)
        {
            instance = this.GetComponent<T>();
            DontDestroyOnLoad(instance.gameObject);
        }
    }

    void OnEnable()
    {
        
    }
}
