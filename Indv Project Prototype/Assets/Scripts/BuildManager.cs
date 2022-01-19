using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{

    public static BuildManager instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than one build manager in scene");
            return;
        }
        instance = this;
    }

    public GameObject standardTurretPrefab;
    private GameObject turretToBuild;

    private void Start()
    {
        turretToBuild = standardTurretPrefab;
    }

    public GameObject GetTurretToBuild()
    {
        return turretToBuild;
    }

    public GameObject SetTurretToBuild()
    {
        return null;
    }

}
