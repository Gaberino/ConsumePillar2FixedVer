using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSetter : MonoBehaviour
{
    public Material[] RockMats;
    public Mesh[] RockMeshes;
    // Start is called before the first frame update
    void Start()
    {
        int modulod = (Mathf.Abs(Mathf.RoundToInt(transform.position.x)) + Mathf.Abs(Mathf.RoundToInt(transform.position.z))) % 3;

        MeshRenderer renderer = this.GetComponent<MeshRenderer>();
        renderer.material = RockMats[modulod];

        MeshFilter myMesh = GetComponent<MeshFilter>();
        myMesh.mesh = RockMeshes[modulod];
        transform.localEulerAngles = Vector3.up * (90 * modulod);
    }
}
