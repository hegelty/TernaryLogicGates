using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Math = System.Math;

public class GeneralGates : Gate
{
    public override void CalculateOutput()
    {
        int t;
        switch (gateName)
        {
            case "Buf":
                outputs.Add(inputs[0]);
                break;
            case "Neg":
                outputs.Add(inputs[0] * -1);
                break;
            case "Inc":
                outputs.Add(inputs[0] == 1 ? -1 : inputs[0]+1);
                break;
            case "Dec":
                outputs.Add(inputs[0] == -1 ? 1 : inputs[0]-1);
                break;
            case "Isminus":
                outputs.Add(inputs[0] == -1 ? 1 : -1);
                break;
            case "Iszero":
                outputs.Add(inputs[0] == 0 ? 1 : -1);
                break;
            case "Isplus":
                outputs.Add(inputs[0] == 1 ? 1 : -1);
                break;
            case "Notplus":
                outputs.Add(inputs[0] == 1 ? -1 : 1);
                break;
            case "Clampup":
                outputs.Add(Math.Max(inputs[0], 0));
                break;
            case "Clampdown":
                outputs.Add(Math.Min(inputs[0], 0));
                break;
            case "Min":
                outputs.Add(Math.Min(inputs[0], inputs[1]));
                break;
            case "Max":
                outputs.Add(Math.Max(inputs[0], inputs[1]));
                break;
            case "Antimax":
                outputs.Add(Math.Max(inputs[0], inputs[1]) * -1);
                break;
            case "Antimin":
                outputs.Add(Math.Min(inputs[0], inputs[1]) * -1);
                break;
            case "Xor":
                outputs.Add(Math.Max(Math.Min(inputs[0], -1 * inputs[1]), Math.Min(-1 * inputs[0], inputs[1])));
                break;
            case "Sum":
                t = inputs[0] + inputs[1];
                outputs.Add(t == 2 ? -1 : t == -2 ? 1 : t);
                break;
            case "Cons":
                outputs.Add(inputs[0] == inputs[1] ? inputs[0] : 0);
                break;
            case "Any":
                t = inputs[0] + inputs[1];
                outputs.Add(t > 0 ? 1 : t < 0 ? -1 : 0);
                break;
            case "Equals":
                outputs.Add(inputs[0] == inputs[1] ? 1 : -1);
                break;
            case "Decode":
                outputs.Add(inputs[0] == -1?1:-1);
                outputs.Add(inputs[0] == 0?1:-1);
                outputs.Add(inputs[0] == 1?1:-1);
                break;
        }
    }

    public override void Init()
    {
        gateName = name.Replace("(Clone)", "");
        List<string> monadic = new List<string> { "Buf", "Neg", "Inc", "Dec", "Decode", "Isminus", "Iszero", "Isplus", "Notplus", "Clampup", "Clampdown" };

        input_count = monadic.Contains(gateName) ? 1 : 2;
        output_count = gateName == "Decode"?3:1;
        _isSwitch = false;
        base.Init();
    }
}
