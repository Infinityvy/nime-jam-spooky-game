using System.Collections;
using System.Collections.Generic;
using Unity.DemoTeam.Hair;
using UnityEngine;

public class SetShaderParentPosition : MonoBehaviour
{
    public Material material;

    void Update()
    {
        material.SetVector("_Parent_Position", transform.position);
    }
}
