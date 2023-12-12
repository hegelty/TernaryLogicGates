using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class GateBuilder : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            GameObject.Find("CircuitCanvas").GetComponent<BuilderManager>().SetCurrentGate(this.name.Replace("Builder_", ""));
        }
    }
}
