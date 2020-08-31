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
        public Vector2Int gridPos;
        public BlockInstance linkedBlock; //linked block responds to movement attempts from head block
        public IDynamicBlock script;
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
    
    public Stack<Queue<Action>> undoInstructions = new Stack<Queue<Action>>();

    void Start()
    {
        Instance = this;
        undoInstructions.Push(new Queue<Action>());
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
        //generate new testing grid by setting the public testing grid to current state
        //if (someBlock.linkedBlock != null) 
        Vector2Int target = someBlock.gridPos + Vector2Int.RoundToInt(direction);

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
        Vector2Int storedPos = someBlock.gridPos;
        Action unMove = () => { ForceMove(someBlock, storedPos); Debug.Log("didmove"); };
        
        undoInstructions.Peek().Enqueue(unMove);
        someBlock.gridPos = target;        
        return true;
    }

    public void AttemptConsume(BlockInstance someBlock, Vector2 direction, Action<BlockInstance> OnAttemptCallback)
    {
        Vector2Int target = someBlock.gridPos + Vector2Int.RoundToInt(direction);

        if (WithinBounds(target, out int x, out int y) && CurrentLevel[x, y] != null)
        {
            OnAttemptCallback(CurrentLevel[x, y]);
            //if (CurrentLevel[x, y].block.Properties.Contains(Block.PROPERTY.Consumable))
            //{
            //    RemoveAtUnchecked(x, y);
            //}
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
        //purge undo of level make
        undoInstructions.Pop();
        undoInstructions.Push(new Queue<Action>());
    }

    public BlockInstance LoadBlock(LevelTemplate.BlockDefinition bD)
    {
        return LoadBlock(bD, null);
    }

    public BlockInstance LoadBlock(LevelTemplate.BlockDefinition bD, BlockInstance childInstance)
    {
        GameObject instance = Instantiate(bD.block.Prefab, transform);
        Vector3 localPosition = bD.block.Prefab.gameObject.transform.localPosition + new Vector3(bD.position.x, 0.0f, bD.position.y);
        instance.transform.localPosition = localPosition;
        BlockInstance someBlockInstance = new BlockInstance
        {
            block = bD.block,
            gameObject = instance,
            gridPos = bD.position,
            linkedBlock = childInstance,
        };

        CurrentLevel[(int)bD.position.x, (int)bD.position.y] = someBlockInstance;

        instance.SendMessage("SetBlockInstance", someBlockInstance, SendMessageOptions.DontRequireReceiver);
        //to undo
        Action unMake = () => { RemoveAtUnchecked((int)bD.position.x, (int)bD.position.y); };
        undoInstructions.Peek().Enqueue(unMake);
        return someBlockInstance;
    }

    public void Undo()
    {
        Debug.Log("undo called");
        if (undoInstructions.Count > 1)
        {
            undoInstructions.Pop(); //pop empty
            int numOfOps = undoInstructions.Peek().Count;
            for (int i = 0; i < numOfOps ; i++)
            {
                undoInstructions.Peek().Dequeue().Invoke();
            }
            //cleanse the junk
            undoInstructions.Pop();
            undoInstructions.Push(new Queue<Action>());
        }
    }

    private void ForceMove(BlockInstance bI, Vector2Int pos)
    {
        //move thing to grid pos
        if (CurrentLevel[(int)bI.gridPos.x, (int)bI.gridPos.y] == bI)
            CurrentLevel[(int)bI.gridPos.x, (int)bI.gridPos.y] = null;
        CurrentLevel[(int)pos.x, (int)pos.y] = bI;
        bI.gridPos = pos;

        bI.gameObject.transform.localPosition = new Vector3(pos.x, 0, pos.y);
    }

    public void RemoveAt(Vector2Int position)
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
            LevelTemplate.BlockDefinition remakeDef = new LevelTemplate.BlockDefinition {
                position = new Vector2Int(x, y),
                block = CurrentLevel[x,y].block};
            Action remake = () => { LoadBlock(remakeDef, CurrentLevel[x, y].linkedBlock); };
            undoInstructions.Peek().Enqueue(remake);
            Destroy(CurrentLevel[x, y].gameObject);
        }
        CurrentLevel[x, y] = null;

    }

    private bool WithinBounds(Vector2Int target, out int x, out int y)
    {
        x = (int)target.x;
        y = (int)target.y;
        return x >= 0 && y >= 0 && x < Levels[CurrentLevelIndex].Width && y < Levels[CurrentLevelIndex].Height;
    }
}
