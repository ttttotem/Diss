using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public enum Type {Turret, Capacitor, Fuse}
    public Type type;
    public int level;
    public int resellValue = 50;

    public Upgrades[] upgrades;

    public void Upgrade(int i)
    {
        if(i<0 || i >= upgrades.Length)
        {
            return;
        }   
        if(type == Type.Turret)
        {
            Turret type = GetComponent<Turret>();
            type.Upgrade(upgrades[i]);
        } else if (type == Type.Capacitor)
        {
            Capacitor type = GetComponent<Capacitor>();
            type.Upgrade(upgrades[i]);
        } else if (type == Type.Fuse)
        {
            Fuze type = GetComponent<Fuze>();
            type.Upgrade(upgrades[i]);
        }
    }

    public Upgrades[] getUpgrades()
    {
        return upgrades;
    }
}
