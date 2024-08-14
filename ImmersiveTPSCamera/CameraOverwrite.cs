using System;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.Client.NoObf;

namespace ImmersiveTPSCamera;

#pragma warning disable IDE0060
[HarmonyPatchCategory("immersivetpscamera_camera")]
class CameraOverwrite
{
    public Harmony overwriter;
    static public double cameraXPosition = 0.5;
    static public double cameraYPosition = 0.00;

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

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Camera), "GetCameraMatrix")]
    public static void GetCameraMatrixStart(Camera __instance, Vec3d camEyePosIn, Vec3d worldPos, double yaw, double pitch, AABBIntersectionTest intersectionTester)
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

            // East to North
            if (yaw < 1.5)
            {
                var percentage = GetPercentage(yaw, 0.0, 1.5);
                camEyePosIn[0] += 0.0 + (cameraXPosition - 0.0) * (percentage / 100.0);
                camEyePosIn[1] += cameraYPosition;
                camEyePosIn[2] += cameraXPosition + (0.0 - cameraXPosition) * (percentage / 100.0);
                // Debug.Log($"X: {0.0 + (cameraXPosition - 0.0) * (percentage / 100.0)}, Z: {cameraXPosition + (0.0 - cameraXPosition) * (percentage / 100.0)}");
            }
            // North to West
            else if (yaw >= 1.5 && yaw <= 3.15)
            {
                var percentage = GetPercentage(yaw, 1.5, 3.15);
                camEyePosIn[0] += cameraXPosition + (0.0 - cameraXPosition) * (percentage / 100.0);
                camEyePosIn[1] += cameraYPosition;
                camEyePosIn[2] += 0.0 + (-cameraXPosition - 0.0) * (percentage / 100.0);
                // Debug.Log($"X: {cameraXPosition + (0.0 - cameraXPosition) * (percentage / 100.0)}, Z: {0.0 + (-cameraXPosition - 0.0) * (percentage / 100.0)}");
            }
            // West to South
            else if (yaw > 3.15 && yaw <= 4.75)
            {
                var percentage = GetPercentage(yaw, 3.15, 4.75);
                camEyePosIn[0] += 0.0 + (-cameraXPosition - 0.0) * (percentage / 100.0);
                camEyePosIn[1] += cameraYPosition;
                camEyePosIn[2] += -cameraXPosition + (0.0 - -cameraXPosition) * (percentage / 100.0);
                // Debug.Log($"X: {0.0 + (-cameraXPosition - 0.0) * (percentage / 100.0)}, Z: {-cameraXPosition + (0.0 - -cameraXPosition) * (percentage / 100.0)}");
            }
            // South to East
            else
            {
                var percentage = GetPercentage(yaw, 4.75, 6.28);
                camEyePosIn[0] += -cameraXPosition + (0.0 - -cameraXPosition) * (percentage / 100.0);
                camEyePosIn[1] += cameraYPosition;
                camEyePosIn[2] += 0.0 + (cameraXPosition - 0.0) * (percentage / 100.0);
                // Debug.Log($"X: {-cameraXPosition + (0.0 - -cameraXPosition) * (percentage / 100.0)}, Z: {0.0 + (cameraXPosition - 0.0) * (percentage / 100.0)}");
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

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Camera), "GetCameraMatrix")]
    public static double[] GetCameraMatrixFinish(double[] __result, Camera __instance, Vec3d camEyePosIn, Vec3d worldPos, double yaw, double pitch, AABBIntersectionTest intersectionTester)
    {
        #region native
        int TppCameraDistanceMax = 10;
        int Tppcameradistance = 1;
        Vec3d camEyePosOutTmp = __instance.CameraEyePos.Clone();
        Vec3d camTargetTmp = new();
        #endregion

        // Recreating the internal functions from Camera
        bool LimitThirdPersonCameraToWalls(AABBIntersectionTest intersectionTester, double yaw, Vec3d eye, Vec3d target, FloatRef curtppcameradistance)
        {
            float GetIntersectionDistance(AABBIntersectionTest intersectionTester, Vec3d eye, Vec3d target)
            {
                float Length(float x, float y, float z)
                {
                    return GameMath.Sqrt(x * x + y * y + z * z);
                }

                Line3D pick = new();
                double raydirX = eye.X - target.X;
                double raydirY = eye.Y - target.Y;
                double raydirZ = eye.Z - target.Z;
                float raydirLength1 = (float)Math.Sqrt(raydirX * raydirX + raydirY * raydirY + raydirZ * raydirZ);
                raydirX /= (double)raydirLength1;
                raydirY /= (double)raydirLength1;
                raydirZ /= (double)raydirLength1;
                raydirX *= (double)(Tppcameradistance + 1f);
                raydirY *= (double)(Tppcameradistance + 1f);
                raydirZ *= (double)(Tppcameradistance + 1f);
                pick.Start = target.ToDoubleArray();
                pick.End = new double[3];
                pick.End[0] = target.X + raydirX;
                pick.End[1] = target.Y + raydirY;
                pick.End[2] = target.Z + raydirZ;
                intersectionTester.LoadRayAndPos(pick);
                BlockSelection selection = intersectionTester.GetSelectedBlock(TppCameraDistanceMax, (BlockPos pos, Block block) => block.CollisionBoxes != null && block.CollisionBoxes.Length != 0 && block.RenderPass != EnumChunkRenderPass.Transparent && block.RenderPass != EnumChunkRenderPass.Meta);
                if (selection != null)
                {
                    float pickX = (float)((double)selection.Position.X + selection.HitPosition.X - target.X);
                    float pickY = (float)((double)selection.Position.Y + selection.HitPosition.Y - target.Y);
                    float pickZ = (float)((double)selection.Position.Z + selection.HitPosition.Z - target.Z);
                    float pickdistance = Length(pickX, pickY, pickZ);
                    return GameMath.Max(0.3f, pickdistance - 1f);
                }
                return 999f;
            }

            float centerDistance = GetIntersectionDistance(intersectionTester, eye, target);
            float leftDistance = GetIntersectionDistance(intersectionTester, eye.AheadCopy(0.15000000596046448, 0.0, yaw + 1.5707963705062866), target.AheadCopy(0.15000000596046448, 0.0, yaw + 1.5707963705062866));
            float rightDistance = GetIntersectionDistance(intersectionTester, eye.AheadCopy(-0.15000000596046448, 0.0, yaw + 1.5707963705062866), target.AheadCopy(-0.15000000596046448, 0.0, yaw + 1.5707963705062866));
            float distance = GameMath.Min(centerDistance, leftDistance, rightDistance);
            if ((double)distance < 0.35) return false;
            else return true;
        }

        if (CameraFunctions.shouldImmerse)
        {
            IClientWorldAccessor cworld = intersectionTester.blockSelectionTester as IClientWorldAccessor;
            EntityPlayer plr = cworld.Player.Entity;

            // Specific pixel xray treatment
            double xDiff = Math.Abs(plr.Pos.X - camEyePosOutTmp.X);
            double yDiff = Math.Abs(plr.Pos.Y - camEyePosOutTmp.Y);
            double zDiff = Math.Abs(plr.Pos.Z - camEyePosOutTmp.Z);
            // Debug.Log($"PLAYER: {plr.Pos.X} {plr.Pos.Y} {plr.Pos.Z}, CAMERA: {camEyePosOutTmp.X} {camEyePosOutTmp.Y} {camEyePosOutTmp.Z}");
            // Debug.Log($"DIFFERENCE BETWEEN: {xDiff} {yDiff} {zDiff}");
            if (xDiff < 0.3 && zDiff < 0.4)
                (cworld.Player as ClientPlayer).OverrideCameraMode = EnumCameraMode.FirstPerson;

            double yawBrute = yaw;
            // Normalizing Yaw
            if (yaw < 0) { yaw *= -1; yaw = 6.28 - yaw; };
            while (yaw < 0) yaw += 6.28;
            while (yaw >= 6.28) yaw -= 6.28;

            // Readding the private member
            double[] lookAt(Vec3d from, Vec3d to)
            {
                #region native
                double[] array = new double[16];
                Mat4d.LookAt(array, from.ToDoubleArray(), to.ToDoubleArray(), Vec3Utilsd.FromValues(0.0, 1.0, 0.0));
                return array;
                #endregion
            }

            // Readding the private member
            double[] lookatFp(EntityPlayer plr, Vec3d camEyePosIn)
            {
                #region native
                camEyePosOutTmp.X = camEyePosIn.X + plr.LocalEyePos.X;
                camEyePosOutTmp.Y = camEyePosIn.Y + plr.LocalEyePos.Y;
                camEyePosOutTmp.Z = camEyePosIn.Z + plr.LocalEyePos.Z;
                #endregion

                // Get the percentage between the 2 numbers and desired number
                static double GetPercentage(double value, double minValue, double maxValue)
                {
                    if (value <= minValue) return 0.0;
                    else if (value >= maxValue) return 100.0;
                    else return (value - minValue) / (maxValue - minValue) * 100.0;
                }


                // Recalculating the camera position

                // East to North
                if (yaw < 1.5)
                {
                    var percentage = GetPercentage(yaw, 0.0, 1.5);
                    camEyePosOutTmp.X -= 0.0 + (cameraXPosition - 0.0) * (percentage / 100.0);
                    camEyePosOutTmp.Z -= cameraXPosition + (0.0 - cameraXPosition) * (percentage / 100.0);
                    // Debug.Log($"East to North, yaw: {yaw}");
                }
                // North to West
                else if (yaw >= 1.5 && yaw <= 3.15)
                {
                    var percentage = GetPercentage(yaw, 1.5, 3.15);
                    camEyePosOutTmp.X -= cameraXPosition + (0.0 - cameraXPosition) * (percentage / 100.0);
                    camEyePosOutTmp.Z -= 0.0 + (-cameraXPosition - 0.0) * (percentage / 100.0);
                    // Debug.Log($"North to West, yaw: {yaw}");
                }
                // West to South
                else if (yaw > 3.15 && yaw <= 4.75)
                {
                    var percentage = GetPercentage(yaw, 3.15, 4.75);
                    camEyePosOutTmp.X -= 0.0 + (-cameraXPosition - 0.0) * (percentage / 100.0);
                    camEyePosOutTmp.Z -= -cameraXPosition + (0.0 - -cameraXPosition) * (percentage / 100.0);
                    // Debug.Log($"West to South, yaw: {yaw}");
                }
                // South to East
                else
                {
                    var percentage = GetPercentage(yaw, 4.75, 6.28);
                    camEyePosOutTmp.X -= -cameraXPosition + (0.0 - -cameraXPosition) * (percentage / 100.0);
                    camEyePosOutTmp.Z -= 0.0 + (cameraXPosition - 0.0) * (percentage / 100.0);
                    // Debug.Log($"South to East, yaw: {yaw}");
                }

                #region native
                camTargetTmp.X = camEyePosOutTmp.X + __instance.forwardVec.X;
                camTargetTmp.Y = camEyePosOutTmp.Y + __instance.forwardVec.Y;
                camTargetTmp.Z = camEyePosOutTmp.Z + __instance.forwardVec.Z;
                return lookAt(camEyePosOutTmp, camTargetTmp);
                #endregion
            }

            camTargetTmp.X = worldPos.X + plr.LocalEyePos.X;
            camTargetTmp.Y = worldPos.Y + plr.LocalEyePos.Y;
            camTargetTmp.Z = worldPos.Z + plr.LocalEyePos.Z;
            camEyePosOutTmp.X = camTargetTmp.X + __instance.forwardVec.X * (double)(0f - Tppcameradistance);
            camEyePosOutTmp.Y = camTargetTmp.Y + __instance.forwardVec.Y * (double)(0f - Tppcameradistance);
            camEyePosOutTmp.Z = camTargetTmp.Z + __instance.forwardVec.Z * (double)(0f - Tppcameradistance);
            if ((cworld.Player as ClientPlayer).OverrideCameraMode == EnumCameraMode.FirstPerson || !LimitThirdPersonCameraToWalls(intersectionTester, yawBrute, camEyePosOutTmp, camTargetTmp, FloatRef.Create(Tppcameradistance)))
                return lookatFp(plr, camEyePosIn);
        }

        return __result;
    }
}