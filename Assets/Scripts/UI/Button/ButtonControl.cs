using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ButtonControl : MonoBehaviour
{
    public enum ButtonEvent // 버튼이벤트 종류
    {
        Start,
        End
    }
    public ButtonEvent buttonEvent;

    Button button;
    GameObject UImanager;

    void Start()
    {
        button = gameObject.GetComponent<Button>();
        UImanager = FindAnyObjectByType<GameUIManager>().gameObject;
        GetClickEvent(buttonEvent);
    }

    void OnEnable()
    {
        if(UImanager != null)
            GetClickEvent(buttonEvent);
    }

    /// <summary>
    /// 클릭 이벤트를 등록하는 함수
    /// </summary>
    void GetClickEvent(ButtonEvent eventName)
    {
        switch (eventName)
        {
            case ButtonEvent.Start:
                button.onClick.AddListener(UImanager.GetComponent<GameUIManager>().StartGame);
                break;
            case ButtonEvent.End:
                button.onClick.AddListener(UImanager.GetComponent<GameUIManager>().EndGame);
                break;
        }
    }
}