using System;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.Client.NoObf;

namespace ImmersiveTPSCamera;

public class Init : ModSystem
{
    ICoreClientAPI clientApi;

    readonly CameraFunctions cameraFunctions = new();
    readonly CameraOverwrite cameraOverwrite = new();

    public override void StartClientSide(ICoreClientAPI api)
    {
        clientApi = api;
        base.StartClientSide(api);
        cameraFunctions.Initialize(clientApi);
    }

    public override bool ShouldLoad(EnumAppSide forSide) => forSide == EnumAppSide.Client;
    public override void Start(ICoreAPI api) { base.Start(api); cameraOverwrite.OverwriteNativeFunctions(); }
    public override void Dispose() { base.Dispose(); cameraOverwrite.overwriter.UnpatchAll(); }
}

public class Debug
{
    static public void Log(string message)
    {
        Console.WriteLine($"{DateTime.Now:d.M.yyyy HH:mm:ss} [ImmersiveTPSCamera] {message}");
    }
}
