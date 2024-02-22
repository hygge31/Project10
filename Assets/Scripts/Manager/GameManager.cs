using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject player;

    public Transform poolingBox;

    [Header("Map")]
    public GameObject map2;
    public GameObject line;

    [Header("Components")]
    NavMeshSurface navMeshSurface;

    private void Awake()
    {
        instance = this;
        navMeshSurface = GetComponent<NavMeshSurface>();
    }


    public void Bake()
    {
        navMeshSurface.BuildNavMesh();
    }

}
