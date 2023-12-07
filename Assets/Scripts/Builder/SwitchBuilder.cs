using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class SwitchBuilder : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Mouse Click Button : Left");
            GameObject.Find("Canvas").GetComponent<BuilderManager>().SetCurrentGate("Switch");
        }
    }
}
