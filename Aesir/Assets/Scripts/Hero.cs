using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Entity      
{
    public bool move;
    public bool attack;

    public void SetMoveTrue()
    {
        move = true;
    }
    public void SetMoveFalse()
    {
        move = false;
    }

    public void SetAttackTrue()
    {
        attack = true;
    }
    public void SetAttackFalse()
    {
        attack = false;
    }


}
