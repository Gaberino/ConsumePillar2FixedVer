using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using System;

public enum Direction { Up, Down, Left, Right}

public class MortisController : Singleton<MortisController>
{
    [Min (0f)]
    public float moveDur = 0.5f;
    [Min(0f)]
    public float turnDur = 0.25f;
    [Min(0f)]
    public float inputBuffer = 100f; //in milliseconds
    private float timer = 0f;

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

    void Do_Move_Forward()
    {
        Vector2 direction = new Vector2(transform.forward.x, transform.forward.z);
        
        if (LevelManager.Instance.AttemptMove(direction))
        {
            canControl = false;
            Tween.Position(transform, transform.position + transform.forward, moveDur, 0f, Tween.EaseSpring, Tween.LoopType.None, null,
            () => { canControl = true; });
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

    private void OnConsume()
    {
        Vector2 direction = new Vector2(transform.forward.x, transform.forward.z);
        LevelManager.Instance.AttemptConsume(direction, (block) =>
        {
            if (block.Properties.Contains(Block.PROPERTY.Consumable))
            {
                Debug.Log("Monch");
                // On eat logic
            }
        });
    }
}
