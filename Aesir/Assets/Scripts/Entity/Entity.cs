using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int m_nHealth;
    public int m_nHealthMax;
    public int m_nActionPoints;
    public int m_nActionPointMax;
    public int m_nBasicAttack;
    public int m_nBasicAttackRange;   
    public int m_nBasicAttackCost;
    public int m_nAbility1Attack;
    public int m_nAbility1AttackRange;
    public int m_nAbility1AttackCost;
    public int m_nAbility2Attack;
    public int m_nAbility2AttackRange;
    public int m_nAbility2AttackCost;
    public int m_nMovementActionPointCostPerTile;


    public Node m_currentNode;
}

