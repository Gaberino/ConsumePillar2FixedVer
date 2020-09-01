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
        public Vector3Int gridPos; //z is layer
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
    private BlockInstance[,,] CurrentLevel;
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

    public bool AttemptMove(BlockInstance someBlock, Vector2Int direction)
    {
        //generate new testing grid by setting the public testing grid to current state
        //if (someBlock.linkedBlock != null) 
        Vector2Int target = (Vector2Int)someBlock.gridPos + direction;

        if (!(WithinBounds(target, out int x, out int y)))
        {
            return false;
        }

        if (CurrentLevel[x, y, 1] != null) //solid at that position
        {
            return false;
        }
        //all clear
        Vector3Int storedPos = someBlock.gridPos;
        Action unMove = () => { ForceMove(someBlock, storedPos); };

        undoInstructions.Peek().Enqueue(unMove);

        someBlock.gridPos = new Vector3Int(target.x, target.y, someBlock.gridPos.z); //stay in your lane
        if (CurrentLevel[storedPos.x, storedPos.y, storedPos.z] == someBlock)
            CurrentLevel[storedPos.x, storedPos.y, storedPos.z] = null; //remove self from last pos if something else isn't there
        CurrentLevel[someBlock.gridPos.x, someBlock.gridPos.y, someBlock.gridPos.z] = someBlock; //assign self to new pos

        return true;
    }

    public void AttemptConsume(BlockInstance someBlock, Vector2Int direction, Action<BlockInstance> OnAttemptCallback)
    {
        Vector2Int target = (Vector2Int)someBlock.gridPos + direction;
        if (WithinBounds(target, out int x, out int y)) {
            //Find the first consumable, start from player layer go down
            //can consume self?
            int targLayer = -1;
            for (int i = 2; i > -1; i--)
            {
                if (CurrentLevel[x, y, i] != null)
                {
                    targLayer = i;
                    break;
                }
            }

            if (targLayer > -1)
            {
                OnAttemptCallback(CurrentLevel[x, y, targLayer]);
                //if (CurrentLevel[x, y].block.Properties.Contains(Block.PROPERTY.Consumable))
                //{
                //    RemoveAtUnchecked(x, y);
                //}
            }
        }
        Debug.Log(x + ", " + y);
    }

    public void GoToNextLevel()
    {
        SwitchToLevel(CurrentLevelIndex + 1);
    }

    public void SwitchToLevel(int levelIndex)
    {

        // Do other things like animations.
        UnloadCurrentLevel();
        LoadLevel(levelIndex);
    }

    private void LoadLevel(int levelIndex)
    {
        CurrentLevelIndex = levelIndex;
        //ResetPlayerPosition();
        LevelTemplate level = Levels[levelIndex];
        Board.Instance.SetGrid(level.Height, level.Width);
        CurrentLevel = new BlockInstance[level.Height, level.Width, 3]; //3 layers, passables, solids, player
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

    public void UnloadCurrentLevel()
    {
        UnloadBlocks();
        CurrentLevel = null;
    }

    public void UnloadBlocks()
    {
        foreach (BlockInstance block in CurrentLevel)
        {
            if (block != null)
            {
                Destroy(block.gameObject);
            }
        }
    }

    public void OnCompleteLevel()
    {
        // Do other things like animations.
        GoToNextLevel();
    }

    public void OnAfterMove()
    {
        if (IsLevelInWinState())
        {
            OnCompleteLevel();
        }
    }

    public bool IsLevelInWinState()
    {
        //
        // Placeholder: Currently sets IsInWinState when player is on a solution block.
        //

        HashSet<Block.PROPERTY> propertiesAtPosition = new HashSet<Block.PROPERTY>();

        for (int x = 0; x < Levels[CurrentLevelIndex].Width; x++)
        {
            for (int y = 0; y < Levels[CurrentLevelIndex].Height; y++)
            {
                propertiesAtPosition.Clear();

                for (int z = 0; z < 3; z++)
                {
                    if (CurrentLevel[x, y, z] != null)
                    {
                        foreach (Block.PROPERTY prop in CurrentLevel[x, y, z].block.Properties)
                        {
                            propertiesAtPosition.Add(prop);
                        }
                    }
                }

                if (propertiesAtPosition.Contains(Block.PROPERTY.Player) && propertiesAtPosition.Contains(Block.PROPERTY.Solution))
                {
                    return true;
                }
            }
        }
        return false;
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
            gridPos = Vector3Int.zero, //assign to this post def so we can check props for layer
            linkedBlock = childInstance,
        };
        List<Block.PROPERTY> sbProps = someBlockInstance.block.Properties;
        int layerAssign = 0;

        if (sbProps.Contains(Block.PROPERTY.Player)) layerAssign = 2;
        else if (sbProps.Contains(Block.PROPERTY.Solid)) layerAssign = 1;

        someBlockInstance.gridPos = new Vector3Int(bD.position.x, bD.position.y, layerAssign);

        CurrentLevel[bD.position.x, bD.position.y, layerAssign] = someBlockInstance;
        instance.SendMessage("SetBlockInstance", someBlockInstance, SendMessageOptions.DontRequireReceiver);
        //to undo
        Action unMake = () => { RemoveAtUnchecked(new Vector3Int(bD.position.x, bD.position.y, layerAssign)); };
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

    private void ForceMove(BlockInstance bI, Vector3Int pos) //z is layer
    {
        //move thing to grid pos
        if (CurrentLevel[bI.gridPos.x, bI.gridPos.y, bI.gridPos.z] == bI)
            CurrentLevel[bI.gridPos.x, bI.gridPos.y, bI.gridPos.z] = null;
        CurrentLevel[pos.x, pos.y, pos.z] = bI;
        bI.gridPos = pos;

        bI.gameObject.transform.localPosition = new Vector3(pos.x, 0, pos.y);
    }

    public void RemoveAt(Vector3Int position)
    {
        if (WithinBounds((Vector2Int)position, out int x, out int y))
        {
            if (CurrentLevel[x, y, position.z] != null)
            {
                Destroy(CurrentLevel[x, y, position.z].gameObject);
            }
            CurrentLevel[x, y, position.z] = null;
        }
    }

    public void RemoveAtUnchecked(Vector3Int pos)
    {
        if (CurrentLevel[pos.x, pos.y, pos.z] != null)
        {
            LevelTemplate.BlockDefinition remakeDef = new LevelTemplate.BlockDefinition {
                position = new Vector2Int(pos.x, pos.y),
                block = CurrentLevel[pos.x,pos.y, pos.z].block};
            Action remake = () => { LoadBlock(remakeDef, CurrentLevel[pos.x, pos.y, pos.z]?.linkedBlock); };
            undoInstructions.Peek().Enqueue(remake);
            Destroy(CurrentLevel[pos.x, pos.y, pos.z].gameObject);
        }
        CurrentLevel[pos.x, pos.y, pos.z] = null;

    }

    private bool WithinBounds(Vector2Int target, out int x, out int y)
    {
        x = (int)target.x;
        y = (int)target.y;
        return x >= 0 && y >= 0 && x < Levels[CurrentLevelIndex].Width && y < Levels[CurrentLevelIndex].Height;
    }
}
