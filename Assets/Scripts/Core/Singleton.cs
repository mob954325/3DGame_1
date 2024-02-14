using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // 싱글톤
    // 1. 생성 -> 로드 될 때 생성?
    // 2. 초기화
    // - 만약 싱글톤이 없으면 임시 오브젝트를 생성해서 싱글톤 오브젝트 생성
    // 3. 게임이 종료되면 제거

    private static T instance;

    /// <summary>
    /// 싱글톤 객체를 읽기 위한 프로퍼티
    /// </summary>
    public static T Instance
    {
        get
        {
            if(instance = null)
            {
                T singleton = FindAnyObjectByType<T>(); // 싱글톤 오브젝트 찾기

                if(singleton == null) // 없으면 임시 오브젝트 생성
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
    /// 싱글톤이 초기화 되어있는지 확인하는 bool
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
