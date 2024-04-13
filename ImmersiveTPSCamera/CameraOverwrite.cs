using HarmonyLib;
using Vintagestory.API.MathTools;
using Vintagestory.Client.NoObf;

namespace ImmersiveTPSCamera;

#pragma warning disable IDE0060
[HarmonyPatchCategory("immersivetpscamera_camera")]
class CameraOverwrite
{
    public Harmony overwriter;

    public void OverwriteNativeFunctions()
    {
        if (!Harmony.HasAnyPatches("immersivetpscamera_camera"))
        {
            overwriter = new Harmony("immersivetpscamera_camera");
            overwriter.PatchCategory("immersivetpscamera_camera");
            Debug.Log("Camera overwrited");
        }
        else Debug.Log("ERROR: Camera overwriter has already patched, did some mod already has immersivetpscamera_camera in harmony?");
    }

    // Override Camera Position
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Camera), "GetCameraMatrix")]
    public static void GetCameraMatrix(Camera __instance, Vec3d camEyePosIn, Vec3d worldPos, double yaw, double pitch, AABBIntersectionTest intersectionTester)
    {
        if (CameraFunctions.shouldImmerse)
        {
            // Normalizing Yaw
            if (yaw < 0) { yaw *= -1; yaw = 6.28 - yaw; };
            while (yaw < 0) yaw += 6.28;
            while (yaw >= 6.28) yaw -= 6.28;

            // Get the percentage between the 2 numbers and desired number
            static double GetPercentage(double value, double minValue, double maxValue)
            {
                if (value <= minValue) return 0.0;
                else if (value >= maxValue) return 100.0;
                else return (value - minValue) / (maxValue - minValue) * 100.0;
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
                // Debug.Log($"X: {0.0 + (-1.1 - 0.0) * (percentage / 100.0)}, Z: {-1.1 + (0.0 - -1.1) * (percentage / 100.0)}");
            }
            // South to East
            else
            {
                var percentage = GetPercentage(yaw, 4.75, 6.28);
                camEyePosIn[0] += -1.1 + (0.0 - -1.1) * (percentage / 100.0);
                camEyePosIn[2] += 0.0 + (1.1 - 0.0) * (percentage / 100.0);
                // Debug.Log($"X: {-1.1 + (0.0 - -1.1) * (percentage / 100.0)}, Z: {0.0 + (1.1 - 0.0) * (percentage / 100.0)}");
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