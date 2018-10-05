using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Turns
{
    playerTurn,
    EnemyTurn,
}

public class TurnSystem : MonoBehaviour {

    public Turns m_turn;

    [Header("used for resetting action points")]
    Thor m_thor;
    Freya m_freya;
    Loki m_loki;

	// Use this for initialization
	void Start ()
    {
        m_turn = Turns.playerTurn;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //need to check for actionpoints for players and turn variable for players ASK MESMAN
        
	}

    void PlayerTurnStart()
    {
        m_turn = Turns.playerTurn;

        m_loki.m_nActionPoints = m_loki.m_nActionPointMax;
        m_freya.m_nActionPoints = m_freya.m_nActionPointMax;
        m_thor.m_nActionPoints = m_thor.m_nActionPointMax;

        m_freya.m_nHealth += m_freya.regen;
    }
}
