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

    void LateUpdate()
    {
        transform.position = player.gameObject.transform.position + Offset;

        // 카메라 보정
        Vector3 dir = (player.gameObject.transform.position + Offset) - Camera.main.transform.position;
        length = dir.magnitude;

        Ray ray = new Ray(Camera.main.transform.position, dir);

        if(Physics.Raycast(ray, out RaycastHit hitInfo, length))
        {
            if(hitInfo.collider != null && hitInfo.collider.gameObject.layer != 6)
            {
                vCamera.CameraDistance = Mathf.Min(vCameraDistance, vCamera.CameraDistance - (Camera.main.transform.position - hitInfo.point).magnitude + 0.1f); // 보정될 카메라 위치
            }
            else if(hitInfo.collider.gameObject.layer == 6)
            {
                vCamera.CameraDistance = vCameraDistance;
            }
        }
    }
}