using MoverioBasicFunctionUnityPlugin;
using UnityEngine;
using UnityEngine.UI;

public class GainStepMaxController : MonoBehaviour
{
    private Text gainStepMaxValue;

    void Start()
    {
        var textToDisplayObject = GameObject.Find("GainStepMaxValue");
        gainStepMaxValue = textToDisplayObject.GetComponent<Text>();
    }

    public void OnClick()
    {
        if (gainStepMaxValue == null)
        {
            return;
        }

        if (!MoverioAudio.IsActive())
        {
            return;
        }

        gainStepMaxValue.text = MoverioAudio.GetGainStepMax().ToString();
    }
}
