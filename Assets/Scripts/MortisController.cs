using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using System;
using UnityEngine.SceneManagement;

public enum Direction { Up, Down, Left, Right}

public class MortisController : Singleton<MortisController>, IDynamicBlock
{
    [Min (0f)]
    public float moveDur = 0.5f;
    [Min(0f)]
    public float turnDur = 0.25f;
    [Min(0f)]
    public float inputBuffer = 100f; //in milliseconds
    private float timer = 0f;

    private LevelManager.BlockInstance myBlockInstance;
    private Action bufferedMethod = () => { };

    bool canControl = true;
    public Direction facing = Direction.Down;
    // Start is called before the first frame update
    void Start()
    {
        bufferedMethod = MortisLazy;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < inputBuffer / 1000f)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0f;
            bufferedMethod = MortisLazy;
        }

        if (bufferedMethod != MortisLazy)
        {
            bufferedMethod();
        }
    }

    void MortisLazy()
    {
        Debug.Log("mortis does not desire to move peasant");
    }

    void OnMoveUp()
    {
        if (canControl)
        {
            if (transform.forward == Vector3.forward) Do_Move_Forward();
            else Do_Rotate(Direction.Up);
        }
        else if (bufferedMethod != OnMoveUp)
        {
            bufferedMethod = OnMoveUp;
            timer = 0f;
        }
    }

    void OnMoveDown()
    {
        if (canControl)
        {
            if (transform.forward == Vector3.back) Do_Move_Forward();
            else Do_Rotate(Direction.Down);
        }
        else if (bufferedMethod != OnMoveDown)
        {
            bufferedMethod = OnMoveDown;
            timer = 0f;
        }
    }

    void OnMoveLeft()
    {
        if (canControl)
        {
            if (transform.forward == Vector3.left) Do_Move_Forward();
            else Do_Rotate(Direction.Left);
        }
        else if (bufferedMethod != OnMoveLeft)
        {
            bufferedMethod = OnMoveLeft;
            timer = 0f;
        }
    }

    void OnMoveRight()
    {
        if (canControl)
        {
            if (transform.forward == Vector3.right) Do_Move_Forward();
            else Do_Rotate(Direction.Right);
        }
        else if (bufferedMethod != OnMoveRight)
        {
            bufferedMethod = OnMoveRight;
            timer = 0f;
        }
    }

    void OnUndo()
    {
        if (canControl)
        {
            LevelManager.Instance.Undo();
        }
    }

    void OnReset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Do_Move_Forward()
    {
        Vector2Int direction = new Vector2Int(Mathf.RoundToInt(transform.forward.x), Mathf.RoundToInt(transform.forward.z));
        
        if (LevelManager.Instance.AttemptMove(myBlockInstance, direction))
        {
            Direction tempfacing = facing;
            Action undoRot = () => { MortisController.Instance.Set_Rotate(tempfacing);};
            LevelManager.Instance.undoInstructions.Peek().Enqueue(undoRot);
            //start new undo stack
            LevelManager.Instance.undoInstructions.Push(new Queue<Action>());
            canControl = false;
            Tween.Position(transform, transform.position + transform.forward, moveDur, 0f, Tween.EaseSpring, Tween.LoopType.None, null,
            () => { canControl = true; });

            LevelManager.Instance.OnAfterMove();
        }
    }

    void Do_Rotate(Direction dir)
    {
        
        float turnDeg = 0f;
        switch (dir)
        {
            case Direction.Up:
                switch (facing)
                {
                    case Direction.Down:
                        turnDeg = -180f;
                        break;
                    case Direction.Left:
                        turnDeg = 90f;
                        break;
                    case Direction.Right:
                        turnDeg = -90f;
                        break;
                    default:
                        break;
                }
                break;
            case Direction.Down:
                switch (facing)
                {
                    case Direction.Up:
                        turnDeg = 180f;
                        break;
                    case Direction.Left:
                        turnDeg = -90f;
                        break;
                    case Direction.Right:
                        turnDeg = 90f;
                        break;
                    default:
                        break;
                }
                break;
            case Direction.Left:
                switch (facing)
                {
                    case Direction.Up:
                        turnDeg = -90f;
                        break;
                    case Direction.Down:
                        turnDeg = 90f;
                        break;
                    case Direction.Right:
                        turnDeg = 180f;
                        break;
                    default:
                        break;
                }
                break;
            case Direction.Right:
                switch (facing)
                {
                    case Direction.Up:
                        turnDeg = 90f;
                        break;
                    case Direction.Down:
                        turnDeg = -90f;
                        break;
                    case Direction.Left:
                        turnDeg = -180f;
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
        canControl = false;
        Tween.Rotate(transform, Vector3.up * turnDeg, Space.World, turnDur, 0f, Tween.EaseSpring, Tween.LoopType.None, null,
            () => { facing = dir; canControl = true; });
    }

    public void Set_Rotate(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                transform.localEulerAngles = Vector3.zero;
                facing = Direction.Up;
                break;
            case Direction.Down:
                transform.localEulerAngles = Vector3.up * 180f;
                facing = Direction.Down;
                break;
            case Direction.Left:
                transform.localEulerAngles = Vector3.up * -90f;
                facing = Direction.Left;
                break;
            case Direction.Right:
                transform.localEulerAngles = Vector3.up * 90f;
                facing = Direction.Right;
                break;
            default:
                break;
        }
    }

    private void OnConsume()
    {
        Vector2Int direction = new Vector2Int(Mathf.RoundToInt(transform.forward.x), Mathf.RoundToInt(transform.forward.z));
        LevelManager.Instance.AttemptConsume(myBlockInstance, direction, (blockToConsume) =>
        {
            if (blockToConsume.block.Properties.Contains(Block.PROPERTY.Consumable))
            {
                LevelTemplate.BlockDefinition def = new LevelTemplate.BlockDefinition
                {
                    position = (Vector2Int)myBlockInstance.gridPos,
                    block = blockToConsume.block.consumedForm,
                };
                LevelManager.Instance.RemoveAtUnchecked(blockToConsume.gridPos);

                Debug.Log("Monch");
                Tween.LocalScale(transform, transform.localScale, moveDur, 0f, Tween.EaseWobble);
                LevelManager.BlockInstance tempInstance = myBlockInstance.linkedBlock;
                myBlockInstance.linkedBlock = LevelManager.Instance.LoadBlock(def, tempInstance);

                Do_Move_Forward();
                
                // On eat logic
            }
        });
    }

    public void SetBlockInstance(LevelManager.BlockInstance toSet) //important guy ayyyyyy
    {
        myBlockInstance = toSet;
        myBlockInstance.script = this;
    }

    public bool CanLinkedMove(Direction parentMoveDir, Vector3Int parentPos)
    {
        return false;
    }
}