using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Gate : MonoBehaviour
{
    public bool _isSwitch = false;

    public int input_count;
    public int output_count;

    public List<(Gate, int)> inputGates = new List<(Gate, int)>(); // (gate, output_num)
    public List<List<Gate>> outputGates = new List<List<Gate>>();

    public List<Object> inputPoints = new List<Object>();
    public List<Object> outputPoints = new List<Object>();

    public List<TextMeshProUGUI> inputStateTexts = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> outputStateTexts = new List<TextMeshProUGUI>();


    public List<int> inputs = new List<int>();
    public List<int> outputs = new List<int>();

    public void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        Debug.Log("Init " + gameObject.name); 
        inputGates.Clear();
        outputGates.Clear();
        inputPoints.Clear();
        outputPoints.Clear();
        inputStateTexts.Clear();
        outputStateTexts.Clear();
        inputs.Clear();
        outputs.Clear();
        for(int i = 0; i < input_count; i++)
        {
            inputGates.Add((null, 0));
            inputs.Add(0);
            inputPoints.Add(transform.Find("Input_" + i));
            inputStateTexts.Add(transform.Find("InputText_" + i).GetComponent<TextMeshProUGUI>());
            inputStateTexts[i].text = "0";
        }
        for (int i = 0; i < output_count; i++)
        {
            outputGates.Add(new List<Gate>());
            outputs.Add(0);
            outputPoints.Add(transform.Find("Output_" + i));
            outputStateTexts.Add(transform.Find("OutputText_" + i).GetComponent<TextMeshProUGUI>());
            outputStateTexts[i].text = "0";
        }
    }

    public void SetInputGate(Gate gate, int output_num, int input_num)
    {
        inputGates[input_num] = (gate, output_num);
    }

    public void AddOutputGate(Gate gate, int output_num)
    {
        outputGates[output_num].Add(gate);
    }

    public void RemonveOutputGate(Gate gate, int num)
    {
        outputGates[num].Remove(gate);
    }

    public void Activated()
    {
        if (CheckCondition() && !_isSwitch)
        {
            outputs.Clear();
            GetInputs();
            CalculateOutput();
            ActivateNextGate();
        }
    }

    public abstract void CalculateOutput();

    public void ShowStateText()
    {
        for (int i = 0; i < input_count; i++)
        {
            inputStateTexts[i].text = inputs[i].ToString();
        }
        for (int i = 0; i < output_count; i++)
        {
            outputStateTexts[i].text = outputs[i].ToString();
        }
    }

    public bool CheckCondition()
    {
        foreach ((Gate, int) gate in inputGates)
        {
            if (gate.Item1 == null)
            {
                return false;
            }
        }
        foreach (List<Gate> gates in outputGates)
        {
            foreach (Gate gate in gates)
            {
                if (gate == null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void ActivateNextGate()
    {
        if (CircuitManager.delay > 0)
        {
            StartCoroutine(ActivateNextGateCoroutine());
        }
        else
        {
            ActivateNextGateImmediate();
        }
    }

    private IEnumerator ActivateNextGateCoroutine()
    {
        yield return new WaitForSeconds(CircuitManager.delay);
        ActivateNextGateImmediate();
    }

    private void ActivateNextGateImmediate()
    {
        for(int i = 0;i<output_count;i++)
        {
            foreach (Gate gate in outputGates[i])
            {
                gate.Activated();
            }
        }
    }

    public int GetOutput(int output_num)
    {
        return outputs[output_num];
    }

    public bool IsSwitch()
    {
        return _isSwitch;
    }

    public bool IsStarting()
    {
        foreach ((Gate, int) gate in inputGates)
        {
            if (!gate.Item1.IsSwitch())
            {
                return false;
            }
        }

        return true;
    }

    public void GetInputs()
    {
        inputs.Clear();
        foreach ((Gate, int) gate in inputGates)
        {
            inputs.Add(gate.Item1.GetOutput(gate.Item2));
        }
    }
}
