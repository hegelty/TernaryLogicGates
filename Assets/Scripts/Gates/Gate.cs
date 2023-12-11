using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class Gate : MonoBehaviour, IPointerClickHandler
{
    public string gateName;
    public CircuitManager CircuitManager;

    public bool _isSwitch = false;

    public int input_count; // input 총 개수
    public int output_count; // output 총 개수

    private List<(Gate, int)> inputGates = new List<(Gate, int)>(); // (gate, output_num)
    private List<List<Gate>> outputGates = new List<List<Gate>>();

    public List<GameObject> inputPoints = new List<GameObject>();
    public List<GameObject> outputPoints = new List<GameObject>();

    private List<TextMeshProUGUI> inputStateTexts = new List<TextMeshProUGUI>();
    private List<TextMeshProUGUI> outputStateTexts = new List<TextMeshProUGUI>();

    public List<int> inputs = new List<int>();
    public List<int> outputs = new List<int>();

    public void Start()
    {
        Init();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(CircuitManager._isRemoving)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                CircuitManager.RemoveGate(this);
            }
        }
    }

    public virtual void Init()
    {
        CircuitManager = GameObject.Find("Canvas").GetComponent<CircuitManager>();

        inputGates.Clear(); outputGates.Clear(); 
        inputPoints.Clear(); outputPoints.Clear();
        inputStateTexts.Clear(); outputStateTexts.Clear();
        inputs.Clear(); outputs.Clear();

        for(int i = 0; i < input_count; i++)
        {
            int tmp = i;
            inputGates.Add((null, 0));
            inputs.Add(0);
            inputPoints.Add(transform.Find("Input_" + i).gameObject);
            inputPoints[i].GetComponent<Button>().onClick.AddListener(() => CircuitManager.Connecting(this, tmp, true));
            inputStateTexts.Add(transform.Find("InputText_" + i).GetComponent<TextMeshProUGUI>());
            inputStateTexts[i].text = "0";
        }
        print(inputGates.Count);
        for (int i = 0; i < output_count; i++)
        {
            int tmp = i;
            outputGates.Add(new List<Gate>());
            outputs.Add(0);
            outputPoints.Add(transform.Find("Output_" + i).gameObject);
            outputPoints[i].GetComponent<Button>().onClick.AddListener(() => CircuitManager.Connecting(this, tmp, false));
            outputStateTexts.Add(transform.Find("OutputText_" + i).GetComponent<TextMeshProUGUI>());
            outputStateTexts[i].text = "0";
        }
    }

    // 자기 게이트의 인풋에 아웃풋 연결
    public void SetInputGate(Gate gate, int output_num, int input_num)
    {
        print("SetInput" + gate.name + " " + output_num + " " + input_num + "(" + inputGates.Count);
        inputGates[input_num] = (gate, output_num);
        Activated();
    }

    // 자기 게이트의 아웃풋에 인풋 연결
    public void AddOutputGate(Gate gate, int output_num)
    {
        print("AddOutput" + gate.name + " " + output_num + "(" + outputGates.Count);
        outputGates[output_num].Add(gate);
    }

    public void RemoveOutputGate(Gate gate, int num)
    {
        outputGates[num].Remove(gate);
    }

    public void RemoveInputGate(Gate gate, int output_num, int input_num)
    {
        inputGates[input_num] = (null, 0);
    }

    // 게이트가 활성화되었을 때 호출되는 함수
    public void Activated()
    {
        if (CheckCondition() && !_isSwitch)
        {
            outputs.Clear();
            GetInputs();
            CalculateOutput();
            ShowStateText();
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
        else ActivateNextGateImmediate();
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
            Debug.Log(gate.Item1.name);
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
