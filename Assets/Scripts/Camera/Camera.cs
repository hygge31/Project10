using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Vector3 offset;
    public Transform player;

    public void Start()
    {
        player = GameManager.instance.player.transform;
    }

    private void LateUpdate()
    {
        transform.position = player.position + offset;
    }
}
