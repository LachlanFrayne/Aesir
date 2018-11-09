using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPlayerTurn : MonoBehaviour {

    public Animator anim;
    public GameObject endTurnButton;

	public bool m_enemyTurnFirst;

	private void Start()
    {
        endTurnButton = GameObject.Find("EndTurnButton");
		anim.GetBehaviour<PlayerTurn>().m_enemyTurnFirst = m_enemyTurnFirst;
    }

    public void ExitPlayerTurn()
    {
        anim.SetBool("EnemyTurn",true);
    }

    public GameObject GetButton()
    {
        return endTurnButton;
    }
}
