using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelTemplate", menuName = "ScriptableObjects/LevelTemplate")]
public class LevelTemplate : ScriptableObject
{
    [Serializable]
    public class BlockDefinition
    {
        public Vector2 position;
        public Block block;
    }

    [Header("Level Properties")]
    public int Height;
    public int Width;
    public Vector2 PlayerStartPostion;
    [Header("Block List")]
    public List<BlockDefinition> Blocklist;
}