using MoverioBasicFunctionUnityPlugin;
using MoverioBasicFunctionUnityPlugin.Type;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GetDeviceModeController : MonoBehaviour
{
    private Text audioModeValue;

    void Start()
    {
        var textToDisplayObject = GameObject.Find("DeviceModeValue");
        audioModeValue = textToDisplayObject.GetComponent<Text>();
    }

    public void OnClick()
    {
        if (audioModeValue == null)
        {
            return;
        }

        if (!MoverioAudio.IsActive())
        {
            return;
        }

        try
        {
            if (MoverioAudio.GetDeviceMode() == AudioDeviceMode.DEVICE_MODE_BUILTIN_AUDIO)
            {
                audioModeValue.text = "Builtin";
            }
            else if (MoverioAudio.GetDeviceMode() == AudioDeviceMode.DEVICE_MODE_AUDIO_JACK)
            {
                audioModeValue.text = "Jack";
            }
            else
            {
                audioModeValue.text = "Unknown";
            }
        }
        catch (IOException e)
        {
            Debug.LogError("Getting the audio device mode is failed. Message = " + e.Message);
        }
    }
}
