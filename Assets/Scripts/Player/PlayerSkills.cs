using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSkills : MonoBehaviour
{
    [Header("Components")]
    public GameObject skillObj;

    public delegate void Skills();

    public List<Skills> skillsList = new List<Skills>();

    public Dash dash;


    private void Awake()
    {
        dash = skillObj.GetComponent<Dash>();
    }




}
