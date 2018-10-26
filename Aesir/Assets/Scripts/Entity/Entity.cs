using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int m_nHealth;
    public int m_nHealthMax;
    public int m_nActionPoints;
    public int m_nActionPointMax;
    public int m_nBasicAttackDamage;
    public int m_nBasicAttackRange;   
    public int m_nBasicAttackCost;
    
    public int m_nMovementActionPointCostPerTile;


    public Node m_currentNode;
}

