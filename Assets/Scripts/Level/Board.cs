using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }

    [SerializeField]
    private GameObject Terrain;
    
    void Start()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void SetGrid(int blockWidth, int blockHeight)
    {
        Material material = GetComponent<Renderer>().material;
        material.mainTextureScale = new Vector2(blockWidth/2.0f, blockHeight/2.0f);
        transform.localScale = new Vector3(blockWidth, blockHeight, 1.0f);
        transform.localPosition = new Vector3
            ((blockWidth - 1.0f)/2, -0.5f, (blockHeight - 1.0f)/ 2);

        Terrain.transform.localPosition = new Vector3(-2.0f, 0.0f, blockHeight + 1.0f);
    }
}
