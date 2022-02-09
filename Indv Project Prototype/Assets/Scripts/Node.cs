using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Node : MonoBehaviour
{

    public int col;
    public int row;

    public enum NodeType { Free, Terminal, Blocked, Path, Checking, Turret};
    public NodeType nodeType = NodeType.Free;

    public Color hoverColor;

    private SpriteRenderer rend;
    private Color startColor;

    public Vector3 posOffset;

    private GameObject turret;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        startColor = rend.color;
    }

    public void SetColour(Color c)
    {
        rend.color = c;
    }

    public void ResetColour()
    {
        rend.color = startColor;
    }

    private void OnMouseEnter()
    {
        if (nodeType == NodeType.Free)
        {
            rend.color = hoverColor;
        }
    }

    private void OnMouseExit()
    {
        if (nodeType == NodeType.Free)
        {
            rend.color = startColor;
        }
    }

    private void OnMouseDown()
    {
        if (nodeType == NodeType.Free)
        {
            if (turret != null)
            {
                //Turret exists
                Upgrade.instance.ShowUpgrades(transform.position + posOffset, turret, this);
                return;
            }
            //Is UI (the menu) in the way
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            Shop.instance.ShowShop(transform.position + posOffset,gameObject);
        }
    }

    public void BuildTurret(GameObject _turret)
    {
        if (_turret == null || turret != null)
        {
            return;
        }
        turret = (GameObject)Instantiate(_turret, transform.position + posOffset, transform.rotation);
    }
}
