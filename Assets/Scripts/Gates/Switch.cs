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
        input_count = 0;
        output_count = 1;
        _isSwitch = true;
        state = 0;
        base.Init();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Mouse Click Button : Left");
            state = (state + 2) % 3 - 1;
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            Debug.Log("Mouse Click Button : Middle");
            state = 0;
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Mouse Click Button : Right");
            // set state -1 to 1,  0 to -1, 1 to 0
            state = (state + 3) % 3 - 1;
        }
        outputs[0] = state;
        ShowStateText();
    }
}
