using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    private MeshRenderer mr;

    private void Awake()
    {
        mr = GetComponentInChildren<MeshRenderer>();
    }

    private void Selected()
    {
        mr.material.color = Color.green;
    }

    private void DeSelected()
    {
        mr.material.color = Color.white;
    }
}
