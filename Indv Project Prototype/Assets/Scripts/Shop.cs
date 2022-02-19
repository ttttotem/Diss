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

    Money money;

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
        money = FindObjectOfType<Money>();
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
            bool succeed = false;
            if (i == 0)
            {
                //buying turret
                succeed = money.Purchase(100);
            } else if (i == 1)
            {
                //buying capacitor
                succeed = money.Purchase(200);
            } else if (i == 2)
            {
                //buying fuse
                succeed = money.Purchase(1000);
            }
            if (succeed)
            {
                callingNode.BuildTurret(turrets[i]);
            }
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
