using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultPanel : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI gradeText;

    void Start()
    {

        timeText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        gradeText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        gameObject.SetActive(false);
    }

    public void SetResultText()
    {
        float resultTime = GameManager.Instance.timer;
        timeText.text = $"{(int)resultTime / 60}M {(int)resultTime % 60}S"; // �� : ��
        gradeText.text = $"{SetGrade((int)resultTime)}";

    }

    char SetGrade(int timeSec)
    {
        char c = 'a';
        return c;
        // ���� ���� ����
    }

    /// <summary>
    /// �г� Ȱ��ȭ
    /// </summary>
    public void Show()
    {
        gameObject.SetActive(true);
    }
}
