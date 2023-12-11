using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Switch : Gate, IPointerClickHandler
{
    public int state;
    public override void CalculateOutput()
    {
        outputs.Add(state);
    }

    public override void Init()
    {
        gateName = name.Replace("(Clone)", "");
        input_count = 0;
        output_count = 1;
        _isSwitch = true;
        state = 0;
        base.Init();
    }

    public new void OnPointerClick(PointerEventData eventData)
    {
        if (CircuitManager._isRemoving)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                CircuitManager.RemoveGate(this);
            }
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            state = (state + 2) % 3 - 1;
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            state = 0;
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            state = (state + 3) % 3 - 1;
        }
        outputs[0] = state;
        ShowStateText();
        ActivateNextGate();
    }
}
