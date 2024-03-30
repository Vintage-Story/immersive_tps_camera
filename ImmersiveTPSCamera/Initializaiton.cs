using System;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.Client.NoObf;

namespace ImmersiveTPSCamera;

[HarmonyPatch] // Override Class
public class Init : ModSystem
{
    // Mod functions
    readonly CameraFunctions cameraFunctions = new();
    // API for handling the game state
    ICoreClientAPI clientApi;

    Harmony overrider;

    // Initialization
    public override void StartClientSide(ICoreClientAPI api)
    {
        Debug.Log("Mod Instanciated");
        clientApi = api;
        base.StartClientSide(api);
        cameraFunctions.Initialize(clientApi);

    }

    // Disable load for servers
    public override bool ShouldLoad(EnumAppSide forSide)
    {
        return forSide == EnumAppSide.Client;
    }

    // Override Methods
    public override void Start(ICoreAPI api)
    {
        base.Start(api);
        // Check if patch is already done
        if (!Harmony.HasAnyPatches(Mod.Info.ModID))
        {
            overrider = new Harmony(Mod.Info.ModID);
            // Applies all harmony patches
            overrider.PatchAll();
        }
    }
    public override void Dispose()
    {
        base.Dispose();
        // Unpatch if world exit
        overrider.UnpatchAll(Mod.Info.ModID);
    }

    // Override Camera Position
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Camera), "GetCameraMatrix")]
    public static void GetCameraMatrix(Camera __instance, Vec3d camEyePosIn, Vec3d worldPos, double yaw, double pitch, AABBIntersectionTest intersectionTester)
    {
        if (CameraFunctions.shouldImmerse)
        {
            // Normalizing Yaw
            if (yaw < 0)
            {
                yaw *= -1;
                yaw = 6.28 - yaw;
            };
            while (yaw < 0)
                yaw += 6.28;
            while (yaw >= 6.28)
                yaw -= 6.28;

            // Get the percentage between the 2 numbers and desired number
            static double GetPercentage(double value, double minValue, double maxValue)
            {
                if (value <= minValue)
                    return 0.0;
                else if (value >= maxValue)
                    return 100.0;
                else
                    return (value - minValue) / (maxValue - minValue) * 100.0;
            }

            // CAMERA CALCULATION
            // East to North
            if (yaw < 1.5)
            {
                var percentage = GetPercentage(yaw, 0.0, 1.5);
                camEyePosIn[0] += 0.0 + (1.1 - 0.0) * (percentage / 100.0);
                camEyePosIn[2] += 1.1 + (0.0 - 1.1) * (percentage / 100.0);
                // Debug.Log($"X: {0.0 + (1.1 - 0.0) * (percentage / 100.0)}, Z: {1.1 + (0.0 - 1.1) * (percentage / 100.0)}");
            }
            // North to West
            else if (yaw >= 1.5 && yaw <= 3.15)
            {
                var percentage = GetPercentage(yaw, 1.5, 3.15);
                camEyePosIn[0] += 1.1 + (0.0 - 1.1) * (percentage / 100.0);
                camEyePosIn[2] += 0.0 + (-1.1 - 0.0) * (percentage / 100.0);
                // Debug.Log($"X: {1.1 + (0.0 - 1.1) * (percentage / 100.0)}, Z: {0.0 + (-1.1 - 0.0) * (percentage / 100.0)}");
            }
            // West to South
            else if (yaw > 3.15 && yaw <= 4.75)
            {
                var percentage = GetPercentage(yaw, 3.15, 4.75);
                camEyePosIn[0] += 0.0 + (-1.1 - 0.0) * (percentage / 100.0);
                camEyePosIn[2] += -1.1 + (0.0 - -1.1) * (percentage / 100.0);
                Debug.Log($"X: {0.0 + (-1.1 - 0.0) * (percentage / 100.0)}, Z: {-1.1 + (0.0 - -1.1) * (percentage / 100.0)}");
            }
            // South to East
            else
            {
                var percentage = GetPercentage(yaw, 4.75, 6.28);
                camEyePosIn[0] += -1.1 + (0.0 - -1.1) * (percentage / 100.0);
                camEyePosIn[2] += 0.0 + (1.1 - 0.0) * (percentage / 100.0);
                Debug.Log($"X: {-1.1 + (0.0 - -1.1) * (percentage / 100.0)}, Z: {0.0 + (1.1 - 0.0) * (percentage / 100.0)}");
            }

        }
        //Camera Default Position
        //North Centralized
        // camEyePosIn[0] += 1.1;
        // camEyePosIn[1] += -0.1;
        // camEyePosIn[2] += 0.0;
        //West Centralized
        // camEyePosIn[0] += 0.0;
        // camEyePosIn[1] += -0.1;
        // camEyePosIn[2] += -1.1;
        //East Centralized
        // camEyePosIn[0] += 0.0;
        // camEyePosIn[1] += -0.1;
        // camEyePosIn[2] += 1.1;
        //South Centralized
        // camEyePosIn[0] += -1.1;
        // camEyePosIn[1] += -0.1;
        // camEyePosIn[2] += 0.0;
    }
}

public class Debug
{
    static public void Log(string message)
    {
        Console.WriteLine($"{DateTime.Now:d.M.yyyy HH:mm:ss} [Immersive Camera] {message}");
    }
}
