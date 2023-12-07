using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitManager : MonoBehaviour
{
    public static float delay = 0.1f;
    
    private List<Gate> _gates = new List<Gate>();
    public List<Gate> _startGates = new List<Gate>();

    // Start is called before the first frame update
    void Start()
    {
        FindAndInitAllGates();
        foreach (Gate gate in _startGates)
        {
            gate.Activated();
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
}
