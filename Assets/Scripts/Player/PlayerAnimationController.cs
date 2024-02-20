using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("Components")]
    [HideInInspector]public Animator animator;

    [HideInInspector] public bool isRunning;


    public event Action<float> OnMoveEvent;
    public event Action<float> OnAnimationEvent;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        OnMoveEvent += MoveAnimation;
        OnAnimationEvent += SetAnimationSpeed;
    }

    #region Call Function --------------------------------------------------------------------------------------------------------------------------

    public void CallOnMoveEvent(float distance)
    {
        OnMoveEvent?.Invoke(distance);
    }

    public void CallOnRunAnimationEvent(float speed)
    {
        OnAnimationEvent?.Invoke(speed);
    }
    #endregion

    void MoveAnimation(float distance)
    {
        if(distance < 0.1f)
        {
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isWalking", true);
        }
        
    }


    void SetAnimationSpeed(float speed)
    {
        animator.SetFloat("addSpeed", speed);
    }


}
