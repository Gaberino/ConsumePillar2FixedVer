using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "NewBlock", menuName = "ScriptableObjects/Block")]
public class Block : ScriptableObject
{
    public enum PROPERTY { None = 0, Solid, Consumable, Solution, Player }
    
    public List<PROPERTY> Properties;
    public GameObject Prefab;
    [ShowIf("HasConsume")]
    public Block consumedForm;

    bool HasConsume()
    {
        if (Properties != null)
            if (Properties.Contains(PROPERTY.Consumable)) return true;
        return false;
    }
}