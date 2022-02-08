using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollScript : MonoBehaviour
{

    public Scrollbar scroll;

    public void Start()
    {
        scroll.value = 1;
    }

    public void ResetScroll()
    {
        scroll.value = 1;
    }
}
