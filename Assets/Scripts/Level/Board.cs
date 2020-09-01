using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }

    [SerializeField]
    private GameObject TerrainObject;
    [SerializeField]
    private GameObject BoardObject;

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
        Material material = BoardObject.GetComponent<Renderer>().material;
        material.mainTextureScale = new Vector2(blockWidth/2.0f, blockHeight/2.0f);
        BoardObject.transform.localScale = new Vector3(blockWidth, blockHeight, 1.0f);
        BoardObject.transform.localPosition = new Vector3
            ((blockWidth - 1.0f)/2, -0.5f, (blockHeight - 1.0f)/ 2.0f);
        
        float factor = Mathf.Clamp01((blockWidth - 8)/8.0f);

        TerrainObject.transform.localPosition = new Vector3(-8.0f + factor * 3.0f, -14.0f, blockHeight + 2.0f);
        
        transform.localPosition = new Vector3 (blockWidth/-2.0f, -0.5f, blockHeight/-2.0f);
    }
}
