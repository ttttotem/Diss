using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TabHelper : MonoBehaviour
{

    EventSystem system;
    public GameObject[] tabs;
    public Button enter;
    private int currentTab = 0;

    void Start()
    {
        system = EventSystem.current;// EventSystemManager.currentSystem;

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(tabs == null)
            {
                return;
            }
            currentTab++;
            if (currentTab >= tabs.Length)
            {
                currentTab = 0;
            }
            EventSystem.current.SetSelectedGameObject(tabs[currentTab], null);
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            if(enter != null)
            {
                enter.onClick.Invoke();
            }
        }
    }
}
