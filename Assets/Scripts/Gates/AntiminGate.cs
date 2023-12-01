using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiminGate : Gate
{
    /* Antimin
     *   | - 0 +
     * --+------
     * - | + + +
     * 0 | + 0 0
     * + | + 0 -
     */
    public override void CalculateOutput()
    {
        if (inputs[0] == -1 || inputs[1] == -1) outputs.Add(1);
        else if (inputs[0] == 0 || inputs[1] == 0) outputs.Add(0);
        else outputs.Add(1);
        Debug.Log("Antimin" + inputs[0] + ", " + inputs[1] + " => " + outputs[0]);
    }

    public override void Init()
    {
        input_count = 2;
        output_count = 1;
        _isSwitch = false;
        base.Init();
    }
}
