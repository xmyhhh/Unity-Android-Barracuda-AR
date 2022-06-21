using MoverioBasicFunctionUnityPlugin;
using MoverioBasicFunctionUnityPlugin.Type;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SetPropertyController : MonoBehaviour
{
    public GameObject propertyPanel;
    public GameObject cameraPreview;

    public void OnClick()
    {
        if (!MoverioCamera.IsActive())
        {
            cameraPreview.SetActive(true);
            propertyPanel.SetActive(false);
            return;
        }

        var cameraProperty = MoverioCamera.GetProperty();
        if (cameraProperty == null)
        {
            cameraPreview.SetActive(true);
            propertyPanel.SetActive(false);
            return;
        }

        cameraProperty.Brightness = GetBrightness();
        cameraProperty.WhiteBalanceMode = GetWhiteBalanceMode();
        cameraProperty.Gain = GetGain();
        cameraProperty.FocusDistance = GetFocusDistance();
        cameraProperty.ExposureMode = GetExposureMode();
        cameraProperty.ExposureStep = GetExposureStep();
        cameraProperty.FocusMode = GetFocusMode();
        cameraProperty.CaptureSize = GetCaptureSize();
        cameraProperty.PowerLineFrequencyControlMode = GetPowerLineFrequencyControlMode();
        cameraProperty.CaptureFps = GetCaptureFps();
        cameraProperty.IndicatorMode = GetIndicatorMode();

        MoverioCamera.SetProperty(cameraProperty);

        cameraPreview.SetActive(true);
        propertyPanel.SetActive(false);
    }

    public void OnSetPropertyCompleted(bool result)
    {
        if (!result)
        {
            Debug.Log("Set property is failed.");
        }
        else
        {
            Debug.Log("Set property is success.");
        }
    }

    private int GetBrightness()
    {
        var valueDropdownObject = GameObject.Find("BrightnessValue");
        var brightnessValue = valueDropdownObject.GetComponent<Dropdown>();
        var valueText = brightnessValue.options[brightnessValue.value].text;

        return int.TryParse(valueText, out int value) ? value : 0;
    }

    private WhiteBalanceMode GetWhiteBalanceMode()
    {
        var valueDropdownObject = GameObject.Find("WhiteBalanceModeValue");
        var whiteBalanceModeValue = valueDropdownObject.GetComponent<Dropdown>();
        var valueText = whiteBalanceModeValue.options[whiteBalanceModeValue.value].text;

        if (!(Enum.TryParse(valueText, out WhiteBalanceMode mode) && Enum.IsDefined(typeof(WhiteBalanceMode), mode)))
        {
            return WhiteBalanceMode.WHITE_BALANCE_MODE_AUTO;
        }

        return mode;
    }

    private int GetGain()
    {
        var valueDropdownObject = GameObject.Find("GainValue");
        var gainValue = valueDropdownObject.GetComponent<Dropdown>();
        var valueText = gainValue.options[gainValue.value].text;

        return int.TryParse(valueText, out int value) ? value : 0;
    }

    private int GetFocusDistance()
    {
        var valueDropdownObject = GameObject.Find("FocusDistanceValue");
        var focusDistanceValue = valueDropdownObject.GetComponent<Dropdown>();
        var valueText = focusDistanceValue.options[focusDistanceValue.value].text;

        return int.TryParse(valueText, out int value) ? value : 0;
    }

    private ExposureMode GetExposureMode()
    {
        var valueDropdownObject = GameObject.Find("ExposureModeValue");
        var exposureModeValue = valueDropdownObject.GetComponent<Dropdown>();
        var valueText = exposureModeValue.options[exposureModeValue.value].text;

        if (!(Enum.TryParse(valueText, out ExposureMode mode) && Enum.IsDefined(typeof(ExposureMode), mode)))
        {
            return ExposureMode.EXPOSURE_MODE_AUTO;
        }

        return mode;
    }

    private int GetExposureStep()
    {
        var valueDropdownObject = GameObject.Find("ExposureStepValue");
        var exposureStepValue = valueDropdownObject.GetComponent<Dropdown>();
        var valueText = exposureStepValue.options[exposureStepValue.value].text;

        return int.TryParse(valueText, out int value) ? value : 0;
    }

    private FocusMode GetFocusMode()
    {
        var valueDropdownObject = GameObject.Find("FocusModeValue");
        var focusModeValue = valueDropdownObject.GetComponent<Dropdown>();
        var valueText = focusModeValue.options[focusModeValue.value].text;

        if (!(Enum.TryParse(valueText, out FocusMode mode) && Enum.IsDefined(typeof(FocusMode), mode)))
        {
            return FocusMode.FOCUS_MODE_AUTO;
        }

        return mode;
    }

    private CaptureSize GetCaptureSize()
    {
        var valueDropdownObject = GameObject.Find("CaptureSizeValue");
        var captureSizeValue = valueDropdownObject.GetComponent<Dropdown>();
        var valueText = captureSizeValue.options[captureSizeValue.value].text;

        if (!(Enum.TryParse(valueText, out CaptureSize size) && Enum.IsDefined(typeof(CaptureSize), size)))
        {
            return CaptureSize.CAPTURE_SIZE_640x480;
        }

        return size;
    }

    private PowerLineFrequencyControlMode GetPowerLineFrequencyControlMode()
    {
        var valueDropdownObject = GameObject.Find("PowerLineFrequencyControlModeValue");
        var powerLineFrequencyControlModeValue = valueDropdownObject.GetComponent<Dropdown>();
        var valueText = powerLineFrequencyControlModeValue.options[powerLineFrequencyControlModeValue.value].text;

        if (!(Enum.TryParse(valueText, out PowerLineFrequencyControlMode mode) && Enum.IsDefined(typeof(PowerLineFrequencyControlMode), mode)))
        {
            return PowerLineFrequencyControlMode.POWER_LINE_FREQUENCY_CONTROL_MODE_50HZ;
        }

        return mode;
    }

    private CaptureFps GetCaptureFps()
    {
        var valueDropdownObject = GameObject.Find("CaptureFpsValue");
        var captureFpsValue = valueDropdownObject.GetComponent<Dropdown>();
        var valueText = captureFpsValue.options[captureFpsValue.value].text;

        if (!(Enum.TryParse(valueText, out CaptureFps fps) && Enum.IsDefined(typeof(CaptureFps), fps)))
        {
            return CaptureFps.CAPTURE_FPS_15;
        }

        return fps;
    }

    private IndicatorMode GetIndicatorMode()
    {
        var valueDropdownObject = GameObject.Find("IndicatorModeValue");
        var indicatorModeValue = valueDropdownObject.GetComponent<Dropdown>();
        var valueText = indicatorModeValue.options[indicatorModeValue.value].text;

        if (!(Enum.TryParse(valueText, out IndicatorMode mode) && Enum.IsDefined(typeof(IndicatorMode), mode)))
        {
            return IndicatorMode.INDICATOR_MODE_AUTO;
        }

        return mode;
    }
}
