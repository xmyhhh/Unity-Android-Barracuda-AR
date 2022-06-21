using MoverioBasicFunctionUnityPlugin;
using UnityEngine;
using UnityEngine.UI;

public class GainStepMinController : MonoBehaviour
{
    private Text gainStepMinValue;

    void Start()
    {
        var textToDisplayObject = GameObject.Find("GainStepMinValue");
        gainStepMinValue = textToDisplayObject.GetComponent<Text>();
    }

    public void OnClick()
    {
        if (gainStepMinValue == null)
        {
            return;
        }

        if (!MoverioAudio.IsActive())
        {
            return;
        }

        gainStepMinValue.text = MoverioAudio.GetGainStepMin().ToString();
    }
}
