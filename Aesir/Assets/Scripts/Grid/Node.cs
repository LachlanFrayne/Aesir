using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Node[] neighbours = new Node[4];

    public Node prev;
    public int m_gScore;
    public float m_fScore;
    public float m_hScore;
    public GameObject contain;
}
