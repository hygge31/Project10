using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{

    PlayerController playerController;
    public LayerMask clickableLayerMask;
    public int clickableLayerMaskCode;

    private void Awake()
    {
        clickableLayerMaskCode = LayerMask.GetMask("Ground");
    }


    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }



    public void ClickToMove()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clickableLayerMask))
        {
            if(clickableLayerMask == clickableLayerMaskCode)
            {
                playerController.navMeshAgent.destination = hit.point;
                RotateForward(hit.point);
            }
           
        }
    }


    void RotateForward(Vector3 point)
    {
        Vector3 dir = (point - transform.position).normalized;
        transform.forward = dir;
    }
}
