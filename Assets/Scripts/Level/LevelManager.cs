using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public class BlockInstance
    {
        public Block block;
        public GameObject gameObject;
        public Vector2 gridPos;
    }

    [SerializeField]
    private int InitialLevel = 0;
    [SerializeField]
    private List<LevelTemplate> Levels;
    //[SerializeField]
    //private MortisController Player;

    [Required]
    public Block playerBlock;

    private int CurrentLevelIndex;
    private BlockInstance[,] CurrentLevel;
    //[SerializeField]
    //private Vector2 PlayerPosition;
    
    public Stack<Stack<Action>> undoInstructions = new Stack<Stack<Action>>();

    void Start()
    {
        Instance = this;
        undoInstructions.Push(new Stack<Action>());
        LoadLevel(InitialLevel);
    }

    void OnDestroy()
    {
        Instance = null;
    }

   // public void ResetPlayerPosition()
   // {
    //    Player.transform.position = new Vector3(Levels[CurrentLevelIndex].PlayerStartPostion.x, 0.0f, Levels[CurrentLevelIndex].PlayerStartPostion.y);
    //    Player.transform.SetParent(gameObject.transform, false);
    //}

    public bool AttemptMove(BlockInstance someBlock, Vector2 direction)
    {
        Vector2 target = someBlock.gridPos + new Vector2(Mathf.Round(direction.x), Mathf.Round(direction.y));

        if (!(WithinBounds(target, out int x, out int y)))
        {
            return false;
        }

        if (CurrentLevel[x, y] != null &&
            CurrentLevel[x, y].block.Properties.Contains(Block.PROPERTY.Solid))
        {
            return false;
        }
        //all clear
        Action unMove = () => { ForceMove(someBlock, someBlock.gridPos); Debug.Log("didmove"); };
        
        undoInstructions.Peek().Push(unMove);

        someBlock.gridPos = target;        
        return true;
    }

    public void AttemptConsume(BlockInstance someBlock, Vector2 direction, Action<Block> OnAttemptCallback)
    {
        Vector2 target = someBlock.gridPos + new Vector2(Mathf.Round(direction.x), Mathf.Round(direction.y));

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
        //ResetPlayerPosition();
        LevelTemplate level = Levels[levelIndex];
        CurrentLevel = new BlockInstance[level.Height, level.Width];
        LevelTemplate.BlockDefinition pBlock = new LevelTemplate.BlockDefinition
        {
            position = level.PlayerStartPostion,
            block = playerBlock,
        };
        
        foreach (LevelTemplate.BlockDefinition blockDefinition in level.Blocklist)
        {
            LoadBlock(blockDefinition);
            //GameObject instance = Instantiate(blockDefinition.block.Prefab, transform);
            //Vector3 localPosition = blockDefinition.block.Prefab.gameObject.transform.localPosition + new Vector3(blockDefinition.position.x, 0.0f, blockDefinition.position.y);
            //instance.transform.localPosition = localPosition;

            //CurrentLevel[(int)blockDefinition.position.x, (int)blockDefinition.position.y] = new BlockInstance
            //{
             //   block = blockDefinition.block,
             //   gameObject = instance,
            //};
        }
        LoadBlock(pBlock);
    }

    private void LoadBlock(LevelTemplate.BlockDefinition bD)
    {
        GameObject instance = Instantiate(bD.block.Prefab, transform);
        Vector3 localPosition = bD.block.Prefab.gameObject.transform.localPosition + new Vector3(bD.position.x, 0.0f, bD.position.y);
        instance.transform.localPosition = localPosition;
        BlockInstance someBlockInstance = new BlockInstance
        {
            block = bD.block,
            gameObject = instance,
            gridPos = bD.position,
        };

        CurrentLevel[(int)bD.position.x, (int)bD.position.y] = someBlockInstance;
        instance.SendMessage("SetBlockInstance", someBlockInstance, SendMessageOptions.DontRequireReceiver);
    }

    public void Undo()
    {
        Debug.Log("undo called");
        if (undoInstructions.Count > 1)
        {
            Debug.Log("undo stacks > 1");
            undoInstructions.Pop(); //pop empty
            for (int i = 0; i < undoInstructions.Peek().Count ; i++)
            {
                undoInstructions.Peek().Pop().Invoke();
            }
        }
    }

    private void ForceMove(BlockInstance bI, Vector2 pos)
    {
        //move thing to grid pos
        CurrentLevel[(int)bI.gridPos.x, (int)bI.gridPos.y] = null;
        CurrentLevel[(int)pos.x, (int)pos.y] = bI;
        bI.gridPos = pos;

        bI.gameObject.transform.localPosition = new Vector3(pos.x, 0, pos.y);
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
