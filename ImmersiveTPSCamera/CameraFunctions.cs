using System;
using Vintagestory.API.Client;
using Vintagestory.API.Config;

namespace ImmersiveTPSCamera;

class CameraFunctions
{
    private ICoreClientAPI clientAPI;

    // Trigger for immersing the camera
    static public bool shouldImmerse = false;

    // Class initialization
    public void Initialize(ICoreClientAPI api)
    {
        clientAPI = api;
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
        }
    }

    private static void IncreaseCameraUp()
    {
        if (CameraOverwrite.cameraYPosition >= 1.5) return;
        CameraOverwrite.cameraYPosition += 0.1;
    }

    private static void IncreaseCameraDown()
    {
        if (CameraOverwrite.cameraYPosition <= -1.5) return;
        CameraOverwrite.cameraYPosition -= 0.1;
    }

    private static void IncreaseCameraLeft()
    {
        if (CameraOverwrite.cameraXPosition <= -1.5) return;
        CameraOverwrite.cameraXPosition -= 0.1;
    }

    private static void IncreaseCameraRight()
    {
        if (CameraOverwrite.cameraXPosition >= 1.5) return;
        CameraOverwrite.cameraXPosition += 0.1;
    }

    // Check if the camera is on third person and execute the immersion for the CameraOverwrite
    private void CheckThirdPerson()
    {
        // Check if player is on third person
        if (clientAPI.World.Player.CameraMode == EnumCameraMode.ThirdPerson) shouldImmerse = true;
        else shouldImmerse = false;
    }
}
