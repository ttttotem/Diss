using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shop : MonoBehaviour
{
    public static Shop instance;
    public Canvas canvas;
    public GameObject[] turrets;
    private Node callingNode;
    private BoxCollider2D col;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one shop in scene");
            return;
        }
        instance = this;
    }

    public void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }

    public void ShowShop(Vector3 pos, GameObject node)
    {
        col.enabled = true;
        transform.position = pos + new Vector3(0,0,-1);
        callingNode = node.GetComponent<Node>();
        canvas.enabled = true;
    }

    public void HideShop()
    {
        col.enabled = false;
        canvas.enabled = false;
    }

    public void PickSelection(int i)
    {
        if(i >= 0 && i< turrets.Length)
        {
            callingNode.BuildTurret(turrets[i]);
            HideShop();
        }
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        HideShop();
    }
}
