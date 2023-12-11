using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionInfo
{
    public Gate InputGate, OutputGate;
    public int InputNum, OutputNum;
    public GameObject LineObject;
    public LineRenderer ConnectingLine;
    public ConnectionInfo(Gate gate)
    {
        LineObject = new GameObject();

        LineObject.name = "ConnectingLine";
        LineObject.transform.parent = gate.transform;
        ConnectingLine = LineObject.AddComponent<LineRenderer>();

        ConnectingLine.material = new Material(Shader.Find("Sprites/Default"));
        ConnectingLine.startColor = Color.black;
        ConnectingLine.endColor = Color.black;
        ConnectingLine.startWidth = 0.1f;
        ConnectingLine.endWidth = 0.1f;
        ConnectingLine.positionCount = 2;
    }
}

public class CircuitManager : MonoBehaviour
{
    public static float delay = 0.1f;

    public Slider slider;
    
    public List<Gate> _gates = new List<Gate>();
    public List<Gate> _startGates = new List<Gate>();

    public bool _isRemoving = false;

    private bool _isConnecting = false;
    private bool _connectingPointType;
    private Gate _connectingGate;
    private ConnectionInfo _connectionInfo;
    private LineRenderer _connectingLine;

    private List<ConnectionInfo> _connectingLines = new List<ConnectionInfo>();

    // Start is called before the first frame update
    void Start()
    {
        FindAndInitAllGates();
        foreach (Gate gate in _startGates)
        {
            gate.Activated();
        }
        slider.onValueChanged.AddListener(delegate { delay = slider.value; });
    }

    void Update()
    {
        if (_isConnecting)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse1))
            {
                _isConnecting = false;
                Destroy(_connectionInfo.LineObject);
            }
            else
            {
                Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _connectingLine.SetPosition(1, new Vector3(mousePoint.x, mousePoint.y, 0));
            }
        }
    }

    private void FindAndInitAllGates()
    {
        Gate[] gates = FindObjectsOfType<Gate>();
        foreach (Gate gate in gates)
        {
            gate.Init();
            _gates.Add(gate);
            if (gate.IsStarting())
            {
                _startGates.Add(gate);
            }
        }
    }

    public void Connecting(Gate gate, int num, bool pointType) //pointType: true->input, false->output
    {
        if(!_isConnecting)
        {
            Debug.Log("Connecting_f_" + num);
            _connectingPointType = pointType;
            
            _connectionInfo = new ConnectionInfo(gate);
            _connectingLine = _connectionInfo.ConnectingLine;
            _connectingGate = gate;

            if (pointType)
            {
                _connectionInfo.InputGate = gate;
                _connectionInfo.InputNum = num;
                _connectingLine.SetPosition(0, gate.inputPoints[num].transform.position);
            }
            else
            {
                _connectionInfo.OutputGate = gate;
                _connectionInfo.OutputNum = num;
                _connectingLine.SetPosition(0, gate.outputPoints[num].transform.position);
            }

            _isConnecting = true;
        }
        else
        {
            if(pointType!=_connectingPointType && _connectingGate != gate)
            {
                Debug.Log("Connecting_t_" + num);
                if(_connectingPointType)
                {
                    _connectionInfo.OutputGate = gate;
                    _connectionInfo.OutputNum = num;
                }
                else
                {
                    _connectionInfo.InputGate = gate;
                    _connectionInfo.InputNum = num;
                }
                Connect();
            }
        }
    }

    private void Connect()
    {
        RemoveConnection();
        Gate inputGate = _connectionInfo.InputGate;
        Gate outputGate = _connectionInfo.OutputGate;
        int inputNum = _connectionInfo.InputNum;
        int outputNum = _connectionInfo.OutputNum;

        outputGate.AddOutputGate(inputGate, outputNum);
        inputGate.SetInputGate(outputGate, outputNum, inputNum);

        if(_connectingPointType) _connectingLine.SetPosition(1, outputGate.outputPoints[outputNum].transform.position);
        else _connectingLine.SetPosition(1, inputGate.inputPoints[inputNum].transform.position);
        _isConnecting = false;
        _connectingLines.Add(_connectionInfo);
    }

    public void RemoveConnection()
    {
        Gate inputGate = _connectionInfo.InputGate;
        Gate outputGate = _connectionInfo.OutputGate;
        int inputNum = _connectionInfo.InputNum;
        int outputNum = _connectionInfo.OutputNum;

        foreach(ConnectionInfo connectionInfo in _connectingLines)
        {
            if(connectionInfo.InputGate == inputGate && connectionInfo.InputNum == inputNum)
            {
                connectionInfo.OutputGate.RemoveOutputGate(inputGate, outputNum);
                inputGate.RemoveInputGate(outputGate, outputNum, inputNum);
                Destroy(connectionInfo.LineObject);
                _connectingLines.Remove(connectionInfo);
                break;
            }
        }   
    }

    public void RemoveGate(Gate gate)
    {
        foreach (ConnectionInfo connectionInfo in _connectingLines)
        {
            if (connectionInfo.InputGate == gate || connectionInfo.OutputGate == gate)
            {
                connectionInfo.OutputGate.RemoveOutputGate(gate, connectionInfo.OutputNum);
                gate.RemoveInputGate(connectionInfo.OutputGate, connectionInfo.OutputNum, connectionInfo.InputNum);
                Destroy(connectionInfo.LineObject);
                _connectingLines.Remove(connectionInfo);
                break;
            }
        }
        GameObject.Destroy(gate.gameObject);
        _gates.Remove(gate);
    }

    public static CircuitManager Instance { get; private set; }
}
