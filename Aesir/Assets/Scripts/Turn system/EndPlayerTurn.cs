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
        anim.SetBool("EnemyTurn",true);     //JM:STARTHERE, need to find out how to swap from player to enemy turn
    }

    public GameObject GetButton()
    {
        return endTurnButton;
    }
}
