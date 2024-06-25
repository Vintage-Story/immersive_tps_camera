using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

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
    public override void Start(ICoreAPI api)
    {
        base.Start(api);
        Debug.LoadLogger(api.Logger);
        Debug.Log($"Running on Version: {Mod.Info.Version}");
        cameraOverwrite.OverwriteNativeFunctions();
    }
    public override void Dispose() { base.Dispose(); cameraOverwrite.overwriter.UnpatchAll(); }
}

public class Debug
{
    private static readonly OperatingSystem system = Environment.OSVersion;
    static private ILogger loggerForNonTerminalUsers;

    static public void LoadLogger(ILogger logger) => loggerForNonTerminalUsers = logger;
    static public void Log(string message)
    {
        // Check if is linux or other based system and if the terminal is active for the logs to be show
        if ((system.Platform == PlatformID.Unix || system.Platform == PlatformID.Other) && Environment.UserInteractive)
            // Based terminal users
            Console.WriteLine($"{DateTime.Now:d.M.yyyy HH:mm:ss} [ImmersiveTPSCamera] {message}");
        else
            // Unbased non terminal users
            loggerForNonTerminalUsers?.Log(EnumLogType.Notification, $"[ImmersiveTPSCamera] {message}");
    }
}
