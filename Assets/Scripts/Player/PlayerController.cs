using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;




[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        
    }




  


  
}
