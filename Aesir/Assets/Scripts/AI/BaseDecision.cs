using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDecision : MonoBehaviour
{
    public Enemy m_self;

    protected void Start()
    {
        m_self = GetComponent<Enemy>();
    }

    public abstract void MakeDecision();
}
