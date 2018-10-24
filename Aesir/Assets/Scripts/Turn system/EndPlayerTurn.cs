using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPlayerTurn : MonoBehaviour {

    public Animator anim;
    public GameObject endTurnButton;

    private void Start()
    {
        endTurnButton = GameObject.Find("EndTurnButton");
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
