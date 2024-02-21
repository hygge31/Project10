using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayTile : MonoBehaviour
{

    private void Start()
    {
        HideTile();
    }

    public void ShowTile()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 1);
    }
    public void HideTile()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 0);
    }
}
