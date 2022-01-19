using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathCreator : MonoBehaviour
{
    public static PathCreator instance;
    public int maxPathSize = 40;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one upgrade pathcreator in scene");
            return;
        }
        instance = this;
    }


    public bool pressing = false;
    public bool tracing = false;

    private Node[] nodes;
    private int currentPathSize = 0;

    private void Start()
    {
        nodes = new Node[maxPathSize];
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pressing = true;
        } else if (Input.GetMouseButtonUp(0))
        {
            pressing = false;
            CheckValidPath();
        }
    }

    public void CheckValidPath()
    {
        //Check Nodes

        bool valid = true;

        int counter = 0;
        foreach (Node node in nodes)
        {
            if(node == null)
            {
                break;
            }
            Debug.Log(node.nodeType);
            //All checking nodes until terminal
            if(node.nodeType != Node.NodeType.Checking && counter != 0)
            {
                    if(node.nodeType == Node.NodeType.Terminal)
                    {
                        break;
                    }
                    valid = false;
                    break;
            }
            counter++;
        }


        

        if (valid)
        {
            foreach (Node node in nodes)
            {
                if(node != null)
                {
                    node.nodeType = Node.NodeType.Path;
                    node.SetColour(Color.green);
                }
            }
        } else
        {
            RemoveInvalidPath();
        }
    }

    public void AddNode(Node node)
    {
        if (currentPathSize < maxPathSize)
        {
            nodes[currentPathSize] = node;
            currentPathSize++;
        } else
        {
            RemoveInvalidPath();
        }
    }

    public void RemoveInvalidPath()
    {
        currentPathSize = 0;
        foreach (Node n in nodes)
        {
            if (n != null)
            {
                n.ResetColour();
                n.nodeType = Node.NodeType.Free;
            }
        }
        Array.Clear(nodes, 0, nodes.Length);
    }

    public void RemoveNodes()
    {
        foreach(Node node in nodes)
        {
            Debug.Log("destroying");
            Destroy(node);
        }
    }
}
