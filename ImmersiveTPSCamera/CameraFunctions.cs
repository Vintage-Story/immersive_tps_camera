using Vintagestory.API.Client;

namespace ImmersiveTPSCamera;

class CameraFunctions
{
    private ICoreClientAPI clientAPI;

    // Trigger for immersing the camera
    static public bool shouldImmerse = false;

    // Class initialization
    public void Initialize(ICoreClientAPI api)
    {
        Debug.Log("Initializing camera system");
        clientAPI = api;
        clientAPI.Input.AddHotkeyListener(HotKeyListener);
        Debug.Log("Hotkeys registered");
    }

    // Listining for the cycle camera
    private void HotKeyListener(string hotkeycode, KeyCombination keyComb)
    {
        switch (hotkeycode)
        {
            case "cyclecamera": CheckThirdPerson(); return;
        }
    }

    // Check if the camera is on third person and execute the immersion for the core
    private void CheckThirdPerson()
    {
        // Check if player is on third person
        if (clientAPI.World.Player.CameraMode == EnumCameraMode.ThirdPerson) shouldImmerse = true;
        else shouldImmerse = false;
    }
}
