using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABDecision : BaseDecision
{
    BaseDecision A;
    BaseDecision B;

    public override void MakeDecision()
    {
        if (A)
        {
            A.MakeDecision();
        }
        else
        {
            B.MakeDecision();
        }
    }
}
