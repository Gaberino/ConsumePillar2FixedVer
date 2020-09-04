using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Pixelplacement;

public class RigidMortSegment : MonoBehaviour, IDynamicBlock
{
    LevelManager.BlockInstance myBlockInstance;
    public Block decoupledForm;
//hook into who is ahead in chain or maybe ahead knows behind? so when ahead tries to move it tries to move behind
    public void SetBlockInstance(LevelManager.BlockInstance toSet) //important guy ayyyyyy
    {
        myBlockInstance = toSet;
        myBlockInstance.script = this;
    }

    public bool CanLinkedMove(Vector2Int parentMoveDir, Vector3Int parentPos)
    {
        //continue cascading to bottom of chain
        //target direction will be same as parent move for rigid segments
        if (myBlockInstance.linkedBlock != null && myBlockInstance.linkedBlock.script != null)
            if (!myBlockInstance.linkedBlock.script.CanLinkedMove(parentMoveDir, myBlockInstance.gridPos)) return false;

        if (!LevelManager.Instance.CanMoveTo(myBlockInstance, (Vector2Int)myBlockInstance.gridPos + parentMoveDir, parentMoveDir)) return false;
        

        return true;
    }

    public void DoVisualMove(Vector2Int move)
    {
        //rigid segments move in lockstep with their parent segment
        Action completeAction = null;
        if (myBlockInstance.linkedBlock == null)
        {
            completeAction = () => { MortisController.Instance.canControl = true; };
        }
        Tween.Position(transform, new Vector3(transform.position.x + move.x, transform.position.y, transform.position.z + move.y),
            MortisController.Instance.moveDur, 0f, Tween.EaseSpring, Tween.LoopType.None, null, completeAction);
    }

    public void DoLinkedMove(Vector2Int parentMoveDir, Vector3Int parentPos) //process the parent's move to make my own
    {
        //do move in same dir
        Vector3Int storedPos = myBlockInstance.gridPos;
        LevelManager.Instance.MoveBlock(myBlockInstance, parentMoveDir);
        if (myBlockInstance.linkedBlock?.script != null) myBlockInstance.linkedBlock.script.DoLinkedMove(parentMoveDir, storedPos);
    }

    public void Decouple()
    {
        if (myBlockInstance.linkedBlock != null) myBlockInstance.linkedBlock.script.Decouple();
        LevelTemplate.BlockDefinition dcDef = new LevelTemplate.BlockDefinition
        {
            position = (Vector2Int)myBlockInstance.gridPos,
            block = decoupledForm,
        };
        LevelManager.Instance.LoadBlock(dcDef);
        LevelManager.Instance.RemoveAtUnchecked(myBlockInstance.gridPos);
    }
}

public interface IDynamicBlock
{
    bool CanLinkedMove(Vector2Int parentMoveDir, Vector3Int parentPos);
    void SetBlockInstance(LevelManager.BlockInstance toSet);
    void DoVisualMove(Vector2Int move);
    void DoLinkedMove(Vector2Int parentMoveDir, Vector3Int parentPos);
    void Decouple();
}
