using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// 카메라 위치 조정하는 클래스
/// </summary>
public class FollowCamera : MonoBehaviour
{
    public Player player;
    public Vector3 Offset;
    float length; // racast 거리
    float vCameraDistance; // vitualCamera's Distance
    Cinemachine3rdPersonFollow vCamera;

    void Start()
    {
        player = FindAnyObjectByType<Player>();
        vCamera = FindAnyObjectByType<Cinemachine3rdPersonFollow>();
        vCameraDistance = vCamera.CameraDistance;
    }

    void FixedUpdate()
    {
        transform.position = player.gameObject.transform.position + Offset;

        // 카메라 보정
        Vector3 dir = (player.gameObject.transform.position + Offset) - Camera.main.transform.position;
        length = dir.magnitude;

        Ray ray = new Ray(Camera.main.transform.position, dir);

        if(Physics.Raycast(ray, out RaycastHit hitInfo, length))
        {
            if(hitInfo.collider.gameObject.layer != 6) // Ray가 플레이어가 아닌 다른 레이어(오브젝트)에 닿았을 때
            {
                //Debug.DrawRay(Camera.main.transform.position, dir, Color.red);
                //Debug.Log(vCamera.CameraDistance);
                vCamera.CameraDistance = vCamera.CameraDistance - (Camera.main.transform.position - hitInfo.point).magnitude; // 보정될 카메라 위치
            }
            else
            {
                vCamera.CameraDistance = vCameraDistance;
            }
        }
    }
}