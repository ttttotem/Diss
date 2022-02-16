using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputFieldClick : MonoBehaviour, IPointerClickHandler
{
    public InputField text;
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("this");
        if(text.interactable == true)
        {
            text.text = "";
        }
    }

}
