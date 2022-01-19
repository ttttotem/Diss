using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodesManager : MonoBehaviour
{
    public int colSize=8;

    private void Start()
    {
        int i = 0;
        int j = 0;
        Node[] nodes = GetComponentsInChildren<Node>();
        foreach (Node node in nodes)
        {
            node.col = i;
            node.row = j;
            i++;
            if(i >= colSize)
            {
                i = 0;
                j += 1;
            }
        }
    }
}
