using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBlock", menuName = "ScriptableObjects/Block")]
public class Block : ScriptableObject
{
    public enum PROPERTY { None = 0, Solid, Consumable, Solution }
    
    public List<PROPERTY> Properties;
    public GameObject Prefab;
}