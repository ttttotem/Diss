using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public enum Type {Turret, Capacitor}
    public Type type;
    public int level;

    private Turret turret;
    private Capacitor capacitor;
    public Upgrades[] upgrades;

    private void Start()
    {
        if(type == Type.Turret)
        {
            turret = GetComponent<Turret>();   
        } else if(type == Type.Capacitor)
        {
            capacitor = GetComponent<Capacitor>();
        }
    }

    public void Upgrade(int i)
    {
        if(type == Type.Turret)
        {
            turret.Upgrade(i);
        } else if (type == Type.Capacitor)
        {
            capacitor.Upgrade(i);
        }
    }

    public Upgrades[] getUpgrades()
    {
        if (type == Tower.Type.Turret)
        {
            return turret.upgrades;
        }
        else if (type == Tower.Type.Capacitor)
        {
            return capacitor.upgrades;
        }
        return null;
    }
}
