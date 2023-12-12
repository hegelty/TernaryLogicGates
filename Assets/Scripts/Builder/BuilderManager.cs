using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuilderManager : MonoBehaviour
{
    public string CurrentCursor = "";
    public bool IsMouseOnUi = false;

    public void SetCurrentGate(string gate)
    {
        print(gate);
        CurrentCursor = gate;
        Cursor.SetCursor(Resources.Load<Texture2D>(
            CurrentCursor == "Switch" ? "Images/Switch" : "Images/Gates/" + CurrentCursor), Vector2.zero, CursorMode.Auto);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !IsMouseOnUi)
        {
            if (CurrentCursor != "")
            {
                GameObject canvas = GameObject.Find("CircuitCanvas");
                GameObject gate = Instantiate(Resources.Load<GameObject>(
                    CurrentCursor=="Switch"?"Prefabs/Switch":"Prefabs/Gates/" + CurrentCursor), canvas.transform);
                gate.SetActive(true);
                gate.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                gate.transform.position = new Vector3(gate.transform.position.x, gate.transform.position.y, 0);
                gate.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                CurrentCursor = "";
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                canvas.GetComponent<CircuitManager>()._gates.Add(gate.GetComponent<Gate>());
            }
        }
        else if(Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Escape))
        {
            CurrentCursor = "";
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}
