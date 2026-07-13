using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace ImmersiveTPSCamera;

class CameraConfig
{
    public double XPosition { get; set; } = 1.0;
    public double YPosition { get; set; } = 0.0;
    public float ZoomDistance { get; set; } = 3.0f;
}

class CameraFunctions
{
    private ICoreClientAPI clientAPI;

    // Trigger for immersing the camera
    static public bool shouldImmerse = false;

    private void LoadConfig()
    {
        CameraConfig config = clientAPI.LoadModConfig<CameraConfig>("ImmersiveTPSCamera.json") ?? new CameraConfig();
        CameraOverwrite.cameraXPosition = config.XPosition;
        CameraOverwrite.cameraYPosition = config.YPosition;
        CameraOverwrite.cameraZoomDistance = config.ZoomDistance;
    }

    private void SaveConfig()
    {
        clientAPI.StoreModConfig(new CameraConfig
        {
            XPosition = CameraOverwrite.cameraXPosition,
            YPosition = CameraOverwrite.cameraYPosition,
            ZoomDistance = CameraOverwrite.cameraZoomDistance
        }, "ImmersiveTPSCamera.json");
    }

    // Class initialization
    public void Initialize(ICoreClientAPI api)
    {
        clientAPI = api;
        shouldImmerse = false;
        CameraOverwrite.zoomLoaded = false;
        CameraOverwrite.cameraInstance = null;
        LoadConfig();
        // Right Input registration
        clientAPI.Input.RegisterHotKey(
            "increasecameraright",
            Lang.Get("immerssivetpscamera:increasecameraright"),
            GlKeys.Right, HotkeyType.GUIOrOtherControls,
            false,
            false,
            false
        );
        clientAPI.Input.SetHotKeyHandler("increasecameraright", (_) => true);
        // Left Input registration
        clientAPI.Input.RegisterHotKey(
            "increasecameraleft",
            Lang.Get("immerssivetpscamera:increasecameraleft"),
            GlKeys.Left, HotkeyType.GUIOrOtherControls,
            false,
            false,
            false
        );
        clientAPI.Input.SetHotKeyHandler("increasecameraleft", (_) => true);
        // Up Input registration
        clientAPI.Input.RegisterHotKey(
            "increasecameraup",
            Lang.Get("immerssivetpscamera:increasecameraup"),
            GlKeys.Up, HotkeyType.GUIOrOtherControls,
            false,
            false,
            false
        );
        clientAPI.Input.SetHotKeyHandler("increasecameraup", (_) => true);
        // Down Input registration
        clientAPI.Input.RegisterHotKey(
            "increasecameradown",
            Lang.Get("immerssivetpscamera:increasecameradown"),
            GlKeys.Down, HotkeyType.GUIOrOtherControls,
            false,
            false,
            false
        );
        clientAPI.Input.SetHotKeyHandler("increasecameradown", (_) => true);
        // Flip Input registration
        clientAPI.Input.RegisterHotKey(
            "flipcamera",
            Lang.Get("immerssivetpscamera:flipcamera"),
            GlKeys.F7, HotkeyType.GUIOrOtherControls,
            false,
            false,
            false
        );
        clientAPI.Input.SetHotKeyHandler("flipcamera", (_) => true);
        clientAPI.Input.AddHotkeyListener(HotKeyListener);
        Debug.Log("Hotkeys registered");
    }

    // Listining for the cycle camera
    private void HotKeyListener(string hotkeycode, KeyCombination keyComb)
    {
        switch (hotkeycode)
        {
            case "cyclecamera": CheckThirdPerson(); return;
            case "increasecameraright": IncreaseCameraRight(); return;
            case "increasecameraleft": IncreaseCameraLeft(); return;
            case "increasecameraup": IncreaseCameraUp(); return;
            case "increasecameradown": IncreaseCameraDown(); return;
            case "flipcamera": FlipCamera(); return;
            case "zoomin":
            case "zoomout": SaveZoom(); return;
        }
    }

    private void IncreaseCameraUp()
    {
        if (CameraOverwrite.cameraYPosition >= 1.5) return;
        CameraOverwrite.cameraYPosition += 0.1;
        SaveConfig();
    }

    private void IncreaseCameraDown()
    {
        if (CameraOverwrite.cameraYPosition <= -1.5) return;
        CameraOverwrite.cameraYPosition -= 0.1;
        SaveConfig();
    }

    private void IncreaseCameraLeft()
    {
        if (CameraOverwrite.cameraXPosition <= -1.5) return;
        CameraOverwrite.cameraXPosition -= 0.1;
        SaveConfig();
    }

    private void SaveZoom()
    {
        if (CameraOverwrite.cameraInstance == null) return;
        CameraOverwrite.cameraZoomDistance = CameraOverwrite.GetTargetZoom(CameraOverwrite.cameraInstance);
        SaveConfig();
    }

    private void FlipCamera()
    {
        CameraOverwrite.cameraXPosition *= -1;
        SaveConfig();
    }

    private void IncreaseCameraRight()
    {
        if (CameraOverwrite.cameraXPosition >= 1.5) return;
        CameraOverwrite.cameraXPosition += 0.1;
        SaveConfig();
    }

    // Check if the camera is on third or overhead person and execute the immersion for the CameraOverwrite
    private void CheckThirdPerson()
    {
        var mode = clientAPI.World.Player.CameraMode;
        shouldImmerse = mode == EnumCameraMode.ThirdPerson || mode == EnumCameraMode.Overhead;
    }
}
