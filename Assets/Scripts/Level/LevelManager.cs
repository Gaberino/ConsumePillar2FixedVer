using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pixelplacement;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public class BlockInstance
    {
        public Block block;
        public GameObject gameObject;
        public Vector3Int gridPos; //z is layer
        public BlockInstance linkedBlock; //linked block responds to movement attempts from head block
        public BlockInstance ownerBlock; //reverse of linked
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

    public int CurrentLevelIndex;
    private BlockInstance[,,] CurrentLevel;
    private List<Vector3Int> SolutionBlockPositions;
    //[SerializeField]
    //private Vector2 PlayerPosition;
    public Scene FinishedGameScene;

    public RawImage fadeImage;

    public Stack<Queue<Action>> undoInstructions = new Stack<Queue<Action>>();

    void Start()
    {
        Instance = this;
        ResetUndoInstructions();
        LoadLevel(InitialLevel);
        fadeImage.color = Color.black;
        Tween.Color(fadeImage, Color.clear, 0.5f, 0.5f, Tween.EaseInOutStrong);
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
        Vector2Int target = (Vector2Int)someBlock.gridPos + direction;

        if (WithinBounds(target, out int x, out int y) && CurrentLevel[target.x, target.y, 2] != null) return false;

        if (someBlock.linkedBlock != null)
            if (!someBlock.linkedBlock.script.CanLinkedMove(direction, someBlock.gridPos)) return false;

        
        if (!CanMoveTo(someBlock, target, direction)) return false;
        //if (!(WithinBounds(target, out int x, out int y)))
        //{
        //    return false;
        //}

        //if (CurrentLevel[x, y, 1] != null) //solid at that position
        //{
        //    return false;
        //}
        //all clear
        //move self then cascade back
        //do not return control until all is done
        //Vector3Int storedPos = someBlock.gridPos;
        //Action unMove = () => { ForceMove(someBlock, storedPos); };
        
        //undoInstructions.Peek().Enqueue(unMove);

        //someBlock.gridPos = new Vector3Int(target.x, target.y, someBlock.gridPos.z); //stay in your lane
        //if (CurrentLevel[storedPos.x, storedPos.y, storedPos.z] == someBlock)
        //    CurrentLevel[storedPos.x, storedPos.y, storedPos.z] = null; //remove self from last pos if something else isn't there
        //CurrentLevel[someBlock.gridPos.x, someBlock.gridPos.y, someBlock.gridPos.z] = someBlock; //assign self to new pos
        
        return true;
    }

    public void MoveBlock(BlockInstance someBlock, Vector2Int direction) //call only if attempt move is cleared
    {
        //moves a blockinstance by direction to a new grid position and vacates old position
        //runs input 
        Vector2Int target = (Vector2Int)someBlock.gridPos + direction;
        if (CurrentLevel[target.x, target.y, 1] != null) //move the moveable that could be in front of us
            MoveBlock(CurrentLevel[target.x, target.y, 1], direction);

        Vector3Int storedPos = someBlock.gridPos;
        Action unMove = () => { ForceMove(someBlock, storedPos); };

        undoInstructions.Peek().Enqueue(unMove);

        someBlock.gridPos = new Vector3Int(target.x, target.y, someBlock.gridPos.z); //stay in your lane
        if (CurrentLevel[storedPos.x, storedPos.y, storedPos.z] == someBlock)
            CurrentLevel[storedPos.x, storedPos.y, storedPos.z] = null; //remove self from last pos if something else isn't there
        CurrentLevel[someBlock.gridPos.x, someBlock.gridPos.y, someBlock.gridPos.z] = someBlock; //assign self to new pos

        if (someBlock.script != null) someBlock.script.DoVisualMove(direction);
        else someBlock.gameObject.transform.localPosition = new Vector3(target.x, 0, target.y);
    }

    public bool CanMoveTo(BlockInstance movingBlock, Vector2Int pos, Vector2Int move)
    {
        //operate under the assumption that mortis is weak, and therefore
        //if we were to be pushing a solid, and that solid was to use this func
        //to check if it can move, it will be unable if there is another solid in the way
        //this does mean however that a long rigid mortis can push a line of blocks, but not blocks on blocks
        //get what's there
        if (!WithinBounds(pos, out int x, out int y)) return false; //no move ob

        BlockInstance sAtPos = CurrentLevel[pos.x, pos.y, 1];
        if (sAtPos != null)
        {
            if (sAtPos.block.Properties.Contains(Block.PROPERTY.Solid) && movingBlock.block.Properties.Contains(Block.PROPERTY.Player))
            {
                if (!sAtPos.block.Properties.Contains(Block.PROPERTY.Pusheable)) return false;
                else if (!CanMoveTo(sAtPos, new Vector2Int(sAtPos.gridPos.x + move.x, sAtPos.gridPos.y + move.y), move)) return false;
            }
            else return false;
        }
        
        return true;
    }

    public LevelManager.BlockInstance[] GetAdjacentsOnLayer(Vector3Int pos)
    {
        Vector2Int upPos = (Vector2Int)pos + Vector2Int.up;
        Vector2Int downPos = (Vector2Int)pos + Vector2Int.down;
        Vector2Int leftPos = (Vector2Int)pos + Vector2Int.left;
        Vector2Int rightPos = (Vector2Int)pos + Vector2Int.right;
        int x = 0;
        int y = 0;

        LevelManager.BlockInstance[] returnArray = new BlockInstance[4];
        returnArray[0] = (WithinBounds(upPos, out x, out y) &&
            CurrentLevel[x, y, pos.z] != null) ? CurrentLevel[x, y, pos.z] : null;
        returnArray[1] = (WithinBounds(downPos, out x, out y) &&
            CurrentLevel[x, y, pos.z] != null) ? CurrentLevel[x, y, pos.z] : null;
        returnArray[2] = (WithinBounds(leftPos, out x, out y) &&
            CurrentLevel[x, y, pos.z] != null) ? CurrentLevel[x, y, pos.z] : null;
        returnArray[3] = (WithinBounds(rightPos, out x, out y) &&
            CurrentLevel[x, y, pos.z] != null) ? CurrentLevel[x, y, pos.z] : null;

        return returnArray;
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
        //Debug.Log(x + ", " + y);
    }

    public void GoToNextLevel()
    {
        Tween.Color(fadeImage, Color.black, 0.2f, 0f, Tween.EaseInOutStrong, Tween.LoopType.None, null,
            () => { SwitchToLevel(CurrentLevelIndex + 1); });
    }

    public void SwitchToLevel(int levelIndex)
    {
        Tween.Color(fadeImage, Color.clear, 0.5f, 0.5f, Tween.EaseInOutStrong);
        // Do other things like animations.
        UnloadCurrentLevel();
        if (levelIndex < Levels.Count)
            LoadLevel(levelIndex);
        else SceneManager.LoadScene(FinishedGameScene.buildIndex);
    }

    private void LoadLevel(int levelIndex)
    {
        CurrentLevelIndex = levelIndex;
        //ResetPlayerPosition();
        LevelTemplate level = Levels[levelIndex];
        Board.Instance.SetGrid(level.Height, level.Width);
        CurrentLevel = new BlockInstance[level.Height, level.Width, 3]; //3 layers, passables, solids, player

        SolutionBlockPositions = new List<Vector3Int>();

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
        Invoke("LoadPlayerLate", 0.2f);
        //purge undo of level make
        undoInstructions.Pop();
        undoInstructions.Push(new Queue<Action>());
    }

    private void LoadPlayerLate()
    {
        LevelTemplate.BlockDefinition pBlock = new LevelTemplate.BlockDefinition
        {
            position = Levels[CurrentLevelIndex].PlayerStartPostion,
            block = playerBlock,
        };
        LoadBlock(pBlock);

        //purge undo of level make
        undoInstructions.Pop();
        undoInstructions.Push(new Queue<Action>());
    }

    public void UnloadCurrentLevel()
    {
        UnloadBlocks();
        ResetUndoInstructions();
        CurrentLevel = null;
        SolutionBlockPositions = null;
    }

    public void ResetUndoInstructions()
    {
        undoInstructions.Clear();
        undoInstructions.Push(new Queue<Action>());
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
        MortisController.Instance.Win();
        // Do other things like animations.
        //GoToNextLevel();
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
        // Change based on the actual desired win state.
        //return isThereANonSolutionBlockOnEverySolutionBlock();
        return isThereAPlayerBlockOnEverySolutionBlock();
    }

    public bool isThereANonSolutionBlockOnEverySolutionBlock()
    {
        List<HashSet<Block.PROPERTY>> blockPropertiesAtEachSolutionPosition = getBlockPropertiesAtEachSolutionPosition();

        // Return whether there is a non-solution block with any properties at the same position as every solution block.
        blockPropertiesAtEachSolutionPosition.ForEach(properties => properties.Remove(Block.PROPERTY.Solution));
        return blockPropertiesAtEachSolutionPosition.All(properties => properties.Count > 0);
    }

    public bool isThereAPlayerBlockOnEverySolutionBlock()
    {
        List<HashSet<Block.PROPERTY>> blockPropertiesAtEachSolutionPosition = getBlockPropertiesAtEachSolutionPosition();

        // Return whether there is a player block at the same position as every solution block.
        return blockPropertiesAtEachSolutionPosition.All(properties => properties.Contains(Block.PROPERTY.Player)); 
    }

    public List<HashSet<Block.PROPERTY>> getBlockPropertiesAtEachSolutionPosition()
    {
        List<HashSet<Block.PROPERTY>> blockPropertiesAtEachSolutionPosition = new List<HashSet<Block.PROPERTY>>();

        foreach (Vector3Int solutionBlockPosition in SolutionBlockPositions)
        {
            HashSet<Block.PROPERTY> propertiesAtSolutionBlockPosition = new HashSet<Block.PROPERTY>();
            blockPropertiesAtEachSolutionPosition.Add(propertiesAtSolutionBlockPosition);

            // Check layers 1 and 2 for objects
            for (int zLayer = 0; zLayer < 3; zLayer++)
            {
                BlockInstance blockAtSolution = CurrentLevel[solutionBlockPosition.x, solutionBlockPosition.y, zLayer];

                if (blockAtSolution != null)
                {
                    propertiesAtSolutionBlockPosition.UnionWith(blockAtSolution.block.Properties);
                }
            }
        }

        return blockPropertiesAtEachSolutionPosition;
    }



    public BlockInstance LoadBlock(LevelTemplate.BlockDefinition bD)
    {
        return LoadBlock(bD, null, null);
    }

    public BlockInstance LoadBlock(LevelTemplate.BlockDefinition bD, BlockInstance linkedInstance, BlockInstance ownerInstance)
    {
        GameObject instance = Instantiate(bD.block.Prefab, transform);
        Vector3 localPosition = bD.block.Prefab.gameObject.transform.localPosition + new Vector3(bD.position.x, 0.0f, bD.position.y);
        instance.transform.localPosition = localPosition;
        BlockInstance someBlockInstance = new BlockInstance
        {
            block = bD.block,
            gameObject = instance,
            gridPos = Vector3Int.zero, //assign to this post def so we can check props for layer
            linkedBlock = null, //assign in way we can undo
            ownerBlock = null,
        };
        if (ownerInstance != null) SetBlockOwner(someBlockInstance, ownerInstance);
        if (linkedInstance != null) SetBlockLink(someBlockInstance, linkedInstance);
        List<Block.PROPERTY> sbProps = someBlockInstance.block.Properties;
        int layerAssign = 0;

        if (sbProps.Contains(Block.PROPERTY.Player)) layerAssign = 2;
        else if (sbProps.Contains(Block.PROPERTY.Solid)) layerAssign = 1;

        someBlockInstance.gridPos = new Vector3Int(bD.position.x, bD.position.y, layerAssign);
        if (sbProps.Contains(Block.PROPERTY.Solution)) SolutionBlockPositions.Add(someBlockInstance.gridPos);

        CurrentLevel[bD.position.x, bD.position.y, layerAssign] = someBlockInstance;
        instance.SendMessage("SetBlockInstance", someBlockInstance, SendMessageOptions.DontRequireReceiver);
        //to undo
        Action unMake = () => { RemoveAt(new Vector3Int(bD.position.x, bD.position.y, layerAssign)); };
        undoInstructions.Peek().Enqueue(unMake);
        return someBlockInstance;
    }

    public void SetBlockOwner(BlockInstance child, BlockInstance newOwner)
    {
        Debug.Log("set a ownership of " + child.gameObject.name + " to " + newOwner?.gameObject.name);
        BlockInstance oldOwner = child.ownerBlock;
        Action revertOwner = () => { SetBlockOwner(child, oldOwner); };
        undoInstructions.Peek().Enqueue(revertOwner);
        child.ownerBlock = newOwner;
    }

    public void SetBlockLink(BlockInstance owner, BlockInstance newLink)
    {
        Debug.Log("set a link from " + owner.gameObject.name + " to " + newLink?.gameObject.name);
        BlockInstance oldLink = owner.linkedBlock;
        Action revertLink = () => { SetBlockLink(owner, oldLink); };
        undoInstructions.Peek().Enqueue(revertLink);
        owner.linkedBlock = newLink;
    }

    public void Undo()
    {
        //Debug.Log("undo called");
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
            BlockInstance blockAtPos = CurrentLevel[pos.x, pos.y, pos.z];
            //LevelTemplate.BlockDefinition remakeDef = new LevelTemplate.BlockDefinition {
            //    position = new Vector2Int(pos.x, pos.y),
            //    block = CurrentLevel[pos.x,pos.y, pos.z].block};
            //Action remake = () => { LoadBlock(remakeDef, CurrentLevel[pos.x, pos.y, pos.z]?.linkedBlock, CurrentLevel[pos.x, pos.y, pos.z]?.ownerBlock); };
            Action revive = () => { ReviveBlock(blockAtPos); };
            undoInstructions.Peek().Enqueue(revive);
            CurrentLevel[pos.x, pos.y, pos.z].gameObject.SetActive(false);
        }
        if (CurrentLevel[pos.x, pos.y, pos.z].ownerBlock != null) CurrentLevel[pos.x, pos.y, pos.z].ownerBlock.linkedBlock = null; //clear self from parent
        CurrentLevel[pos.x, pos.y, pos.z] = null;

    }

    public void ReviveBlock(BlockInstance dedBlok)
    {
        dedBlok.gameObject.SetActive(true);
        CurrentLevel[dedBlok.gridPos.x, dedBlok.gridPos.y, dedBlok.gridPos.z] = dedBlok;
        if (dedBlok.ownerBlock != null) SetBlockLink(dedBlok.ownerBlock, dedBlok);
    }

    private bool WithinBounds(Vector2Int target, out int x, out int y)
    {
        x = (int)target.x;
        y = (int)target.y;
        return x >= 0 && y >= 0 && x < Levels[CurrentLevelIndex].Height && y < Levels[CurrentLevelIndex].Width;
    }
}
