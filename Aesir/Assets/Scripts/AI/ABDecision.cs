using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABDecision : BaseDecision
{
    public BaseDecision A;
    public BaseDecision B;

	protected new void Start()
	{
		base.Start();
	}
}
