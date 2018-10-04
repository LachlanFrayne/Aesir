using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    List<Node> selectableTiles = new List<Node>();
    GameObject[] tiles;
    Stack<Node> path = new Stack<Node>();
    Node currentTile;

    int move = 5;
    int moveSpeed = 2;

    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    float halfHeight = 0;

    protected void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        halfHeight = GetComponent<Collider>().bounds.extents.y;
    }

    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);

    }

    public Node GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Node tile = null;
        if(Physics.Raycast(target.transform.position, -Vector3.up, out hit,1))
        {
            tile = hit.collider.GetComponent<Node>();
        }
        return tile;
    }
}

