using System.Runtime.InteropServices;
using UnityEngine;

public enum DeviceType
{
    Desktop,
    Mobile
}

public static class DeviceDetector
{
    [DllImport("__Internal")]
    private static extern int IsMobileDevice();

    [DllImport("__Internal")]
    private static extern float GetDevicePixelRatio();

    [DllImport("__Internal")]
    private static extern int GetScreenWidth();

    [DllImport("__Internal")]
    private static extern int GetScreenHeight();

    private static DeviceType? _cachedDeviceType = null;

    public static DeviceType GetDeviceType()
    {
        if (_cachedDeviceType.HasValue)
            return _cachedDeviceType.Value;

#if UNITY_WEBGL && !UNITY_EDITOR
        try
        {
            int isMobile = IsMobileDevice();
            _cachedDeviceType = (isMobile == 1) ? DeviceType.Mobile : DeviceType.Desktop;
        }
        catch
        {
            _cachedDeviceType = DeviceType.Desktop;
        }
#else
        _cachedDeviceType = (SystemInfo.deviceType == UnityEngine.DeviceType.Handheld) 
            ? DeviceType.Mobile 
            : DeviceType.Desktop;
#endif
        return _cachedDeviceType.Value;
    }

    public static bool IsMobile()
    {
        return GetDeviceType() == DeviceType.Mobile;
    }

    public static bool IsDesktop()
    {
        return GetDeviceType() == DeviceType.Desktop;
    }

    public static float GetPixelRatio()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        try { return GetDevicePixelRatio(); } catch { return 1f; }
#else
        return Screen.dpi > 0 ? Screen.dpi / 96f : 1f;
#endif
    }

    public static Vector2Int GetScreenResolution()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        try { return new Vector2Int(GetScreenWidth(), GetScreenHeight()); } 
        catch { return new Vector2Int(Screen.width, Screen.height); }
#else
        return new Vector2Int(Screen.width, Screen.height);
#endif
    }
}
