using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitManager : MonoBehaviour
{
    public static float delay = 0.1f;
    
    private List<Gate> _gates = new List<Gate>();
    private List<Gate> _startGates = new List<Gate>();

    // Start is called before the first frame update
    void Start()
    {
        FindAllGates();
        foreach (Gate gate in _gates)
        {
            gate.Init();
        }
        foreach (Gate gate in _startGates)
        {
            gate.Activated();
        }
    }

    private void FindAllGates()
    {
        Gate[] gates = FindObjectsOfType<Gate>();
        foreach (Gate gate in gates)
        {
            _gates.Add(gate);
            if (gate.IsStarting())
            {
                _startGates.Add(gate);
            }
        }
    }
}
