using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Entity : MonoBehaviour
{
    public int m_health, m_actionPoints, m_attackRange
                , m_basicAttack, m_attackActionPointCost
                , m_movementActionPointCost;

    public bool move;
    public bool attack;

    [Header("Used for enemies")]
    public int m_meleeDamage;
    public int m_rangeDamage;


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

