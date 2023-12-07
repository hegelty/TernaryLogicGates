using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuilderManager : MonoBehaviour
{
    public string _currentCursor = null;

    public void SetCurrentGate(string gate)
    {
        Debug.Log("SetCurrentGate");
        _currentCursor = gate;
        Cursor.SetCursor(Resources.Load<Texture2D>("Images/" + _currentCursor), Vector2.zero, CursorMode.Auto);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Mouse Click Button : Left\nCurrnent Cursor : " + _currentCursor);
            if (_currentCursor != null)
            {
                Debug.Log("Place " + _currentCursor);
                GameObject canvas = GameObject.Find("Canvas");
                GameObject gate = Instantiate(Resources.Load<GameObject>("Prefabs/" + _currentCursor));
                gate.SetActive(true);
                gate.transform.SetParent(canvas.transform);
                gate.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                gate.transform.localScale = new Vector3(1, 1, 1);
                gate.GetComponent<Gate>().Init();
                _currentCursor = null;
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
        }
    }

    public static BuilderManager Instance { get; private set; }
}
