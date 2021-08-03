//  AgoraRtcEngineExtension.cs
//
//  Created by Yiqing Huang on July 1, 2021.
//  Modified by Yiqing Huang on July 6, 2021.
//
//  Copyright © 2021 Agora. All rights reserved.
//

using System;
using System.Runtime.InteropServices;

namespace agora.rtc
{
    using IrisAudioFrameMixingPtr = IntPtr;

    public static class AgoraRtcEngineExtension
    {
        private static IrisAudioFrameMixingPtr _irisAudioFrameMixingPtr = IntPtr.Zero;

        public static AgoraDisplayInfo[] GetDisplayInfos(this IAgoraRtcEngine agoraRtcEngine)
        {
#if UNITY_EDITOR_WIN || UNITY_EDITOR_OSX || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
            var displayCollectionPtr = AgoraRtcNative.EnumerateDisplays();
            var displayCollection =
                (IrisDisplayCollection) (Marshal.PtrToStructure(displayCollectionPtr, typeof(IrisDisplayCollection)) ??
                                         new IrisDisplayCollection());
            var displayInfos = new AgoraDisplayInfo[displayCollection.length];
            for (var i = 0; i < displayCollection.length; i++)
            {
#if NET_2_0_SUBSET
                var display =
                    (IrisDisplay) (Marshal.PtrToStructure(
                        (IntPtr) ((long) displayCollection.displays + Marshal.SizeOf(typeof(IrisDisplay)) * i),
                        typeof(IrisDisplay)) ?? new IrisDisplay());
                displayInfos[i] = new AgoraDisplayInfo(display.id, display.bounds, display.work_area);
#else
                var display =
                    (IrisDisplay) (Marshal.PtrToStructure(
                        displayCollection.displays + Marshal.SizeOf(typeof(IrisDisplay)) * i,
                        typeof(IrisDisplay)) ?? new IrisDisplay());
                displayInfos[i] = new AgoraDisplayInfo(display.id, display.bounds, display.work_area);
#endif
            }

            AgoraRtcNative.FreeIrisDisplayCollection(displayCollectionPtr);

            return displayInfos;
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static AgoraWindowInfo[] GetWindowInfos(this IAgoraRtcEngine agoraRtcEngine)
        {
#if UNITY_EDITOR_WIN || UNITY_EDITOR_OSX || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
            var windowCollectionPtr = AgoraRtcNative.EnumerateWindows();
            var windowCollection =
                (IrisWindowCollection) (Marshal.PtrToStructure(windowCollectionPtr, typeof(IrisWindowCollection)));
            var windowInfos = new AgoraWindowInfo[windowCollection.length];
            for (var i = 0; i < windowCollection.length; i++)
            {
#if NET_2_0_SUBSET
                var window =
                    (IrisWindow) (Marshal.PtrToStructure(
                        (IntPtr) ((long) windowCollection.windows + Marshal.SizeOf(typeof(IrisWindow)) * i),
                        typeof(IrisWindow)) ?? new IrisWindow());
                windowInfos[i] = new AgoraWindowInfo(window.id, window.name, window.owner_name, window.bounds,
                    window.work_area);
#else
                var window =
                    (IrisWindow) (Marshal.PtrToStructure(
                                      windowCollection.windows + Marshal.SizeOf(typeof(IrisWindow)) * i,
                                      typeof(IrisWindow)) ??
                                  new IrisWindow());
                windowInfos[i] = new AgoraWindowInfo(window.id, window.name, window.owner_name, window.bounds,
                    window.work_area);
#endif
            }

            AgoraRtcNative.FreeIrisWindowCollection(windowCollectionPtr);

            return windowInfos;
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static void CreateIrisAudioFrameMixing()
        {
            _irisAudioFrameMixingPtr = AgoraRtcNative.CreateIrisAudioFrameMixing();
        }

        public static void FreeIrisAudioFrameMixing(IrisAudioFrameMixingPtr mixing_ptr)
        {
            _irisAudioFrameMixingPtr = IntPtr.Zero;
        }

        public static void CacheAudioFrame(AUDIO_FRAME_TYPE type, int samples, int bytesPerSample, int channels,
            int samplesPerSec, IntPtr bufferPtr, uint bufferPtrLength, long renderTimeMs, int avsync_type)
        {
            var audioFrame = new IrisRtcAudioFrame()
            {
                type = type,
                samples = samples,
                bytes_per_sample = bytesPerSample,
                channels = channels,
                samples_per_sec = samplesPerSec,
                buffer = bufferPtr,
                buffer_length = bufferPtrLength,
                render_time_ms = renderTimeMs,
                av_sync_type = avsync_type
            };
            AgoraRtcNative.PushAudioFrame(_irisAudioFrameMixingPtr, ref audioFrame);
        }

        public static void Mixing(AUDIO_FRAME_TYPE type, int samples, int bytesPerSample, int channels,
            int samplesPerSec, IntPtr bufferPtr, uint bufferPtrLength, long renderTimeMs, int avsync_type)
        {
            var audioFrame = new IrisRtcAudioFrame()
            {
                type = type,
                samples = samples,
                bytes_per_sample = bytesPerSample,
                channels = channels,
                samples_per_sec = samplesPerSec,
                buffer = bufferPtr,
                buffer_length = bufferPtrLength,
                render_time_ms = renderTimeMs,
                av_sync_type = avsync_type
            };
            AgoraRtcNative.Mixing(_irisAudioFrameMixingPtr, ref audioFrame);
        }
    }

    public class AgoraWindowInfo
    {
        internal AgoraWindowInfo(ulong id, string name, string ownerName, IrisRect bounds,
            IrisRect workArea)
        {
            WindowId = id;
            WindowName = name;
            AppName = ownerName;
            Bounds = new Rectangle(Convert.ToInt32(bounds.x), Convert.ToInt32(bounds.y),
                Convert.ToInt32(bounds.width), Convert.ToInt32(bounds.height));
            WorkArea = new Rectangle(Convert.ToInt32(workArea.x), Convert.ToInt32(workArea.y),
                Convert.ToInt32(workArea.width), Convert.ToInt32(workArea.height));
        }

        public ulong WindowId { get; private set; }
        public string WindowName { get; private set; }
        public string AppName { get; private set; }
        public Rectangle Bounds { get; private set; }
        public Rectangle WorkArea { get; private set; }
    }

    public class AgoraDisplayInfo
    {
        internal AgoraDisplayInfo(uint id, IrisRect bounds, IrisRect workArea)
        {
            DisplayId = id;
            Bounds = new Rectangle(Convert.ToInt32(bounds.x), Convert.ToInt32(bounds.y),
                Convert.ToInt32(bounds.width), Convert.ToInt32(bounds.height));
            WorkArea = new Rectangle(Convert.ToInt32(workArea.x), Convert.ToInt32(workArea.y),
                Convert.ToInt32(workArea.width), Convert.ToInt32(workArea.height));
        }

        public uint DisplayId { get; private set; }
        public Rectangle Bounds { get; private set; }
        public Rectangle WorkArea { get; private set; }
    }
}