using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultPanel : MonoBehaviour
{
    GameManager manager;

    TextMeshProUGUI timeText;
    TextMeshProUGUI gradeText;

    void Start()
    {
        manager = GameManager.Instance;

        gameObject.SetActive(false);
    }

    public void SetResultText()
    {
        float resultTime = manager.timer;
        timeText.text = $"{resultTime / 60} : {resultTime % 60}"; // 분 : 초
        gradeText.text = $"S";

    }

    /// <summary>
    /// 패널 활성화
    /// </summary>
    public void Show()
    {
        gameObject.SetActive(true);
    }
}
