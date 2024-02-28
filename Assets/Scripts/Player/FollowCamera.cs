using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 카메라 위치 조정하는 클래스
/// </summary>
public class FollowCamera : MonoBehaviour
{
    public Player player;
    public Vector3 Offset;
    public float length; // racast 거리

    void Start()
    {
        player = FindAnyObjectByType<Player>();
    }

    void FixedUpdate()
    {
        transform.position = player.gameObject.transform.position + Offset;

        Vector3 dir = (player.gameObject.transform.position + Offset) - Camera.main.transform.position;

        length = dir.magnitude;

        Ray ray = new Ray(Camera.main.transform.position, dir);

        Debug.DrawRay(Camera.main.transform.position, dir, Color.red);

        if(Physics.Raycast(ray, out RaycastHit hitInfo, length))
        {
            if(hitInfo.collider.gameObject.layer != 6) // 6 : Player
                transform.position = hitInfo.point;
        }
    }
}