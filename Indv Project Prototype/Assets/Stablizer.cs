using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stablizer : MonoBehaviour
{

    //Stop healthbar rotating
    Quaternion rotate;
    void Awake()

    {

        rotate = transform.rotation;

    }
    void LateUpdate()

    {
        transform.rotation = rotate;

    }
}