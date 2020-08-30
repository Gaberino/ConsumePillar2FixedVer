using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public class BlockInstance
    {
        public Block block;
        public GameObject gameObject;
    }

    [SerializeField]
    private int InitialLevel = 0;
    [SerializeField]
    private List<LevelTemplate> Levels;
    [SerializeField]
    private MortisController Player;

    private int CurrentLevelIndex;
    private BlockInstance[,] CurrentLevel;
    [SerializeField]
    private Vector2 PlayerPosition;

    void Start()
    {
        Instance = this;

        LoadLevel(InitialLevel);
    }

    void OnDestroy()
    {
        Instance = null;
    }

    public void ResetPlayerPosition()
    {
        Player.transform.position = new Vector3(Levels[CurrentLevelIndex].PlayerStartPostion.x, 0.0f, Levels[CurrentLevelIndex].PlayerStartPostion.y);
        Player.transform.SetParent(gameObject.transform, false);
    }

    public bool AttemptMove(Vector2 direction)
    {
        Vector2 target = PlayerPosition + new Vector2(Mathf.Round(direction.x), Mathf.Round(direction.y));

        if (!(WithinBounds(target, out int x, out int y)))
        {
            return false;
        }

        if (CurrentLevel[x, y] != null &&
            CurrentLevel[x, y].block.Properties.Contains(Block.PROPERTY.Solid))
        {
            return false;
        }

        PlayerPosition = target;
        return true;
    }

    public void AttemptConsume(Vector2 direction, Action<Block> OnAttemptCallback)
    {
        Vector2 target = PlayerPosition + new Vector2(Mathf.Round(direction.x), Mathf.Round(direction.y));

        if (WithinBounds(target, out int x, out int y) && CurrentLevel[x, y] != null)
        {
            OnAttemptCallback(CurrentLevel[x, y].block);
            if (CurrentLevel[x, y].block.Properties.Contains(Block.PROPERTY.Consumable))
            {
                RemoveAtUnchecked(x, y);
            }
        }
        Debug.Log(x + ", " + y);
    }

    private void LoadLevel(int levelIndex)
    {
        CurrentLevelIndex = levelIndex;
        ResetPlayerPosition();
        LevelTemplate level = Levels[levelIndex];
        CurrentLevel = new BlockInstance[level.Height, level.Width];
        foreach (LevelTemplate.BlockDefinition blockDefinition in level.Blocklist)
        {
            GameObject instance = Instantiate(blockDefinition.block.Prefab, transform);
            Vector3 localPosition = blockDefinition.block.Prefab.gameObject.transform.localPosition + new Vector3(blockDefinition.position.x, 0.0f, blockDefinition.position.y);
            instance.transform.localPosition = localPosition;

            CurrentLevel[(int)blockDefinition.position.x, (int)blockDefinition.position.y] = new BlockInstance
            {
                block = blockDefinition.block,
                gameObject = instance,
            };
        }
    }

    public void RemoveAt(Vector2 position)
    {
        if (WithinBounds(position, out int x, out int y))
        {
            if (CurrentLevel[x, y] != null)
            {
                Destroy(CurrentLevel[x, y].gameObject);
            }
            CurrentLevel[x, y] = null;
        }
    }

    public void RemoveAtUnchecked(int x, int y)
    {
        if (CurrentLevel[x, y] != null)
        {
            Destroy(CurrentLevel[x, y].gameObject);
        }
        CurrentLevel[x, y] = null;
    }

    private bool WithinBounds(Vector2 target, out int x, out int y)
    {
        x = (int)target.x;
        y = (int)target.y;
        return x >= 0 && y >= 0 && x < Levels[CurrentLevelIndex].Width && y < Levels[CurrentLevelIndex].Height;
    }
}
