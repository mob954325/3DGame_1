using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Player player;

    /// <summary>
    /// ������ �����ߴ��� Ȯ���ϴ� ���� (true : ������, false : ����)
    /// </summary>
    public bool isBattle = false;

    /// <summary>
    /// ���ӽð�
    /// </summary>
    public float timer = 0.0f;

    void Update()
    {
        timer += Time.deltaTime;
    }
}