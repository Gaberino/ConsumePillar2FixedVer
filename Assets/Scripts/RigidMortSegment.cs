using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidMortSegment : MonoBehaviour, IDynamicBlock
{
    LevelManager.BlockInstance myBlockInstance;
//hook into who is ahead in chain or maybe ahead knows behind? so when ahead tries to move it tries to move behind
    public void SetBlockInstance(LevelManager.BlockInstance toSet) //important guy ayyyyyy
    {
        myBlockInstance = toSet;
        myBlockInstance.script = this;
    }

    public bool CanLinkedMove(Direction parentMoveDir, Vector3Int parentPos)
    {
        //target direction will be same as parent move
        if (myBlockInstance.linkedBlock != null && myBlockInstance.linkedBlock.script != null)
            if (!myBlockInstance.linkedBlock.script.CanLinkedMove(parentMoveDir, myBlockInstance.gridPos)) return false;
        //NEED TO MAKE A SECOND, TEMPORARY GRID THAT I CAN MOVE STUFF ON AS IT CASCADES FROM TAIL, DOES NOT CHANGE ANYTHING, JUST USED TO PERFORM
        //TESTING AND THEN MERCILESSLY SLAUGHTERED
        //no linked block
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}

public interface IDynamicBlock
{
    bool CanLinkedMove(Direction parentMoveDir, Vector3Int parentPos);
    void SetBlockInstance(LevelManager.BlockInstance toSet);
}
