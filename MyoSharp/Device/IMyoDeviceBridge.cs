using System;
using System.Collections.Generic;

using System.Text;

using MyoSharp.Poses;
using MyoSharp.Communication;

namespace MyoSharp.Device
{
    public interface IMyoDeviceBridge
    {
        #region Methods
        MyoResult Vibrate32(IntPtr myo, VibrationType type, out IntPtr error);

        MyoResult Vibrate64(IntPtr myo, VibrationType type, out IntPtr error);

        MyoResult RequestRssi32(IntPtr myo, out IntPtr error);

        MyoResult RequestRssi64(IntPtr myo, out IntPtr error);

        uint EventGetFirmwareVersion32(IntPtr evt, VersionComponent component);

        uint EventGetFirmwareVersion64(IntPtr evt, VersionComponent component);

        Arm EventGetArm32(IntPtr evt);

        Arm EventGetArm64(IntPtr evt);

        XDirection EventGetXDirection32(IntPtr evt);

        XDirection EventGetXDirection64(IntPtr evt);

        float EventGetOrientation32(IntPtr evt, OrientationIndex index);

        float EventGetOrientation64(IntPtr evt, OrientationIndex index);

        float EventGetAccelerometer32(IntPtr evt, uint index);

        float EventGetAccelerometer64(IntPtr evt, uint index);

        float EventGetGyroscope32(IntPtr evt, uint index);

        float EventGetGyroscope64(IntPtr evt, uint index);

        Pose EventGetPose32(IntPtr evt);

        Pose EventGetPose64(IntPtr evt);

        sbyte EventGetRssi32(IntPtr evt);

        sbyte EventGetRssi64(IntPtr evt);

        MyoResult Unlock32(IntPtr myo, UnlockType type, out IntPtr error);

        MyoResult Unlock64(IntPtr myo, UnlockType type, out IntPtr error);

        MyoResult Lock32(IntPtr myo, out IntPtr error);

        MyoResult Lock64(IntPtr myo, out IntPtr error);

        MyoResult StreamEmg32(IntPtr myo, StreamEmgType streamEmgType, out IntPtr error);

        MyoResult StreamEmg64(IntPtr myo, StreamEmgType streamEmgType, out IntPtr error);

        sbyte EventGetEmg32(IntPtr evt, int sensor);

        sbyte EventGetEmg64(IntPtr evt, int sensor);
        #endregion
    }
}
