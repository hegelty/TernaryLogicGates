using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class GateBuilder : MonoBehaviour, IPointerClickHandler
{
    public string GateName;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            GameObject.Find("Canvas").GetComponent<BuilderManager>().SetCurrentGate(GateName);
        }
    }
}
