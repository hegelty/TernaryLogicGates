using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Remover : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            GameObject.Find("Canvas").GetComponent<CircuitManager>()._isRemoving = true;
            Cursor.SetCursor(Resources.Load<Texture2D>("Images/trash"), Vector2.zero, CursorMode.Auto);
        }
    }
}
