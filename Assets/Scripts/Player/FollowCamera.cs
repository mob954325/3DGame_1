using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public PlayerController player;
    public Vector3 Offset;

    void FixedUpdate()
    {
        transform.position = player.gameObject.transform.position + Offset;
    }
}
