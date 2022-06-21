using MoverioBasicFunctionUnityPlugin;
using UnityEngine;
using UnityEngine.UI;

public class SetGainStepController : MonoBehaviour
{
    private Dropdown gainStepValue;

    void Start()
    {
        var valueDropdownObject = GameObject.Find("SetGainStepValue");
        gainStepValue = valueDropdownObject.GetComponent<Dropdown>();
    }

    public void OnClick()
    {
        if (gainStepValue == null)
        {
            return;
        }

        if (!MoverioAudio.IsActive())
        {
            return;
        }

        var valueText = gainStepValue.options[gainStepValue.value].text;
        if (!int.TryParse(valueText, out int value))
        {
            return;
        }

        if (!MoverioAudio.SetGainStep(value))
        {
            Debug.LogError("Setting the gain step is failed.");
        }
    }
}
