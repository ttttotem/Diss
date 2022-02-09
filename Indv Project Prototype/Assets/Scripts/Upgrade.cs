using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public static Upgrade instance;
    public Button[] btns;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one upgrade shop in scene");
            return;
        }
        instance = this;
    }

    public Canvas canvas;
    private Turret callingTurret;
    private BoxCollider2D col;
    private Tower callingTower;
    private Capacitor callingCapacitor;
    private Node callingNode;

    public void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }

    public void ShowUpgrades(Vector3 pos, GameObject tower, Node node)
    {
        transform.position = pos + new Vector3(0, 0, -1);
        callingTower = tower.GetComponent<Tower>();
        callingNode = node;

        int counter = 0;
        foreach (Upgrades upgrade in callingTower.getUpgrades())
        {
            if (counter < btns.Length)
            {
                btns[counter].GetComponentInChildren<Text>().text = upgrade.name;
                counter++;
            }
        }


        col.enabled = true;
        canvas.enabled = true;
    }

    public void HideUpgrades()
    {
        col.enabled = false;
        canvas.enabled = false;
    }

    public void PickSelection(int i)
    {
        if (i >= 0 && i < callingTower.getUpgrades().Length)
        {
            callingTower.Upgrade(i);
            HideUpgrades();
        }
    }

    public void Sell()
    {
        Destroy(callingTower.gameObject);
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        HideUpgrades();

    }
}
