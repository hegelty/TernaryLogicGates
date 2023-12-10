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
        Debug.Log("SetCurrentGate");
        CurrentCursor = gate;
        Cursor.SetCursor(Resources.Load<Texture2D>("Images/" + CurrentCursor), Vector2.zero, CursorMode.Auto);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !IsMouseOnUi)
        {
            Debug.Log("Mouse Click Button : Left\nCurrent Cursor : " + CurrentCursor);
            if (CurrentCursor != "")
            {
                Debug.Log("Place " + CurrentCursor);
                GameObject canvas = GameObject.Find("Canvas");
                GameObject gate = Instantiate(Resources.Load<GameObject>("Prefabs/" + CurrentCursor), canvas.transform);
                gate.SetActive(true);
                gate.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                gate.transform.position = new Vector3(gate.transform.position.x, gate.transform.position.y, 0);
                gate.transform.localScale = new Vector3(1, 1, 1);
                gate.GetComponent<Gate>().Init();
                CurrentCursor = "";
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
        }
    }

    public static BuilderManager Instance { get; private set; }
}
