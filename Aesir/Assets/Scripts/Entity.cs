using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Entity : MonoBehaviour
{
    public int m_health, m_actionPoints, m_attackRange
                , basicAttack, attackActionPointCost
                , movementActionPointCost;

    [Header("Used for enemies")]
    public int m_meleeDamage;
    public int m_rangeDamage;
}
