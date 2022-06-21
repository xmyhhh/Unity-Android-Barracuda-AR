using MoverioBasicFunctionUnityPlugin;
using UnityEngine;
using UnityEngine.UI;

public class GetGainStepController : MonoBehaviour
{
    private Text gainStepValue;

    void Start()
    {
        var textToDisplayObject = GameObject.Find("GainStepValue");
        gainStepValue = textToDisplayObject.GetComponent<Text>();
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

        gainStepValue.text = MoverioAudio.GetGainStep().ToString();
    }
}
