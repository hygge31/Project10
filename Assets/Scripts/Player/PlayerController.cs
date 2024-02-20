using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAnimationController))]
//[RequireComponent(typeof(PlayerSkills))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Player State")]
    public float playerSpeed;
    
    public Rigidbody _rigidbody;

    private void Awake()
    {
        
        _rigidbody = GetComponent<Rigidbody>();
        
    }




  


  
}
