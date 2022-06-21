using MoverioBasicFunctionUnityPlugin;
using MoverioBasicFunctionUnityPlugin.Type;
using UnityEngine;

public class SetDeviceModeController : MonoBehaviour
{
    public bool isDeviceModeBuiltin;

    public void OnClick()
    {
        if (!MoverioAudio.IsActive())
        {
            return;
        }

        if (isDeviceModeBuiltin)
        {
            if (!MoverioAudio.SetDeviceMode(AudioDeviceMode.DEVICE_MODE_BUILTIN_AUDIO))
            {
                Debug.LogError("Setting the audio device mode is failed.");
            }
        }
        else
        {
            if (!MoverioAudio.SetDeviceMode(AudioDeviceMode.DEVICE_MODE_AUDIO_JACK))
            {
                Debug.LogError("Setting the audio device mode is failed.");
            }
        }
    }
}
