using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class PFXManager : Singleton<PFXManager>
{
    public enum PFX { POPIN, TPCHARGE, TPBURST}
    public GameObject[] PFXPrefabs;
    private Dictionary<PFX, GameObject> EffectDict = new Dictionary<PFX, GameObject>();
    //prefab references to self destroying particle effects

    private void Start()
    {
        for (int i = 0; i < PFXPrefabs.Length; i++)
        {
            EffectDict.Add((PFX)i, PFXPrefabs[i]);
        }
    }

    public void SpawnParticle(PFX effect, Vector3 position, Vector3 rotation)
    {
        GameObject newParticleObj = (GameObject)Instantiate(EffectDict[effect], position, Quaternion.Euler(rotation));
    }

    public void SpawnParticle(PFX effect, Transform parent)
    {
        GameObject newParticleObj = (GameObject)Instantiate(EffectDict[effect], parent);
    }

}
