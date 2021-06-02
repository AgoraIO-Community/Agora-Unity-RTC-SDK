//  AgoraRtcApiNative.cs
//
//  Created by Yiqing Huang on May 25, 2021.
//  Modified by Yiqing Huang on June 2, 2021.
//
//  Copyright © 2021 Agora. All rights reserved.
//

using System;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace agora_gaming_rtc
{
    using IrisRtcEnginePtr = IntPtr;
    using IrisRtcChannelPtr = IntPtr;
    using IrisRtcDeviceManagerPtr = IntPtr;
    using IrisRtcRawDataPtr = IntPtr;
    using IrisRtcRawDataPluginManagerPtr = IntPtr;
    using IrisRtcRendererPtr = IntPtr;
    using IrisEventHandlerHandle = IntPtr;
    using IrisRtcAudioFrameObserverHandle = IntPtr;
    using IrisRtcVideoFrameObserverHandle = IntPtr;
    using IrisRtcRendererCacheConfigHandle = IntPtr;


    internal static class AgoraRtcNative
    {
        #region DllImport

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        public const string MyLibName = "iris-rtc";
#else
#if UNITY_IPHONE
		public const string MyLibName = "__Internal";
#else
        public const string MyLibName = "iris-rtc";
#endif
#endif
        [DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IrisRtcEnginePtr CreateIrisRtcEngine(EngineType type = EngineType.kEngineTypeNormal,
            string executable_path = null);

        [DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DestroyIrisRtcEngine(IrisRtcEnginePtr engine_ptr);

        [DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IrisEventHandlerHandle SetIrisRtcEngineEventHandler(IrisRtcEnginePtr engine_ptr,
            ref IrisCEventHandler event_handler);

        [DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void UnsetIrisRtcEngineEventHandler(IrisRtcEnginePtr engine_ptr,
            ref IrisEventHandlerHandle handle);

        [DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int CallIrisRtcEngineApi(IrisRtcEnginePtr engine_ptr, ApiTypeEngine api_type,
            byte[] @params, out CharArrayAssistant result);

        [DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int CallIrisRtcEngineApiWithBuffer(IrisRtcEnginePtr engine_ptr, ApiTypeEngine api_type,
            byte[] @params, IntPtr buffer, out CharArrayAssistant result);

        [DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IrisRtcDeviceManagerPtr GetIrisRtcDeviceManager(IrisRtcEnginePtr engine_ptr);

        [DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IrisRtcChannelPtr GetIrisRtcChannel(IrisRtcEnginePtr engine_ptr);

        [DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IrisRtcRawDataPtr GetIrisRtcRawData(IrisRtcEnginePtr engine_ptr);

        [DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int CallIrisRtcAudioDeviceManagerApi(IrisRtcDeviceManagerPtr device_manager_ptr,
            ApiTypeAudioDeviceManager api_type, byte[] @params, out CharArrayAssistant result);

        [DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int CallIrisRtcVideoDeviceManagerApi(IrisRtcDeviceManagerPtr device_manager_ptr,
            ApiTypeVideoDeviceManager api_type, byte[] @params, out CharArrayAssistant result);

        [DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IrisEventHandlerHandle SetIrisRtcChannelEventHandler(IrisRtcChannelPtr channel_ptr,
            ref IrisCEventHandler event_handler);

        [DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void UnsetIrisRtcChannelEventHandler(IrisRtcChannelPtr channel_ptr,
            ref IrisEventHandlerHandle handle);

        [DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IrisEventHandlerHandle RegisterIrisRtcChannelEventHandler(IrisRtcChannelPtr channel_ptr,
            string channel_id, ref IrisCEventHandler event_handler);

        [DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void UnRegisterIrisRtcChannelEventHandler(IrisRtcChannelPtr channel_ptr,
            IrisEventHandlerHandle handle, string channel_id);

        [DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int CallIrisRtcChannelApi(IrisRtcChannelPtr channel_ptr, ApiTypeChannel api_type,
            byte[] @params, out CharArrayAssistant result);

        [DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int CallIrisRtcChannelApiWithBuffer(IrisRtcChannelPtr channel_ptr,
            ApiTypeChannel api_type, byte[] @params, IntPtr buffer, out CharArrayAssistant result);

        [DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IrisRtcAudioFrameObserverHandle RegisterAudioFrameObserver(
            IrisRtcRawDataPtr raw_data_ptr, ref IrisRtcCAudioFrameObserver observer, int order, string identifier);

        [DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void UnRegisterAudioFrameObserver(IrisRtcRawDataPtr raw_data_ptr,
            IrisRtcAudioFrameObserverHandle handle, string identifier);

        [DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IrisRtcVideoFrameObserverHandle RegisterVideoFrameObserver(
            IrisRtcRawDataPtr raw_data_ptr, ref IrisRtcCVideoFrameObserver observer, int order, string identifier);

        [DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void UnRegisterVideoFrameObserver(IrisRtcRawDataPtr raw_data_ptr,
            IrisRtcVideoFrameObserverHandle handle, string identifier);

        [DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IrisRtcRawDataPluginManagerPtr GetIrisRtcRawDataPluginManager(
            IrisRtcRawDataPtr raw_data_ptr);

        [DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IrisRtcRendererPtr GetIrisRtcRenderer(IrisRtcRawDataPtr raw_data_ptr);

        [DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int CallIrisRtcRawDataPluginManagerApi(IrisRtcRawDataPluginManagerPtr plugin_manager_ptr,
            ApiTypeRawDataPluginManager api_type, byte[] @params, out CharArrayAssistant result);

        [DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IrisEventHandlerHandle SetIrisRtcRendererEventHandler(IrisRtcRendererPtr renderer_ptr,
            ref IrisCEventHandler event_handler);

        [DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void UnsetIrisRtcRendererEventHandler(IrisRtcRendererPtr renderer_ptr,
            IrisEventHandlerHandle handle);

        [DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IrisRtcRendererCacheConfigHandle EnableVideoFrameCache(IrisRtcRendererPtr renderer_ptr,
            ref IrisRtcCRendererCacheConfig cache_config, uint uid, string channel_id = "");

        [DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DisableVideoFrameCache(IrisRtcRendererPtr renderer_ptr,
            ref IrisRtcRendererCacheConfigHandle? handle, uint uid = uint.MaxValue, string channel_id = "");

        [DllImport(MyLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool GetVideoFrame(IrisRtcRendererPtr renderer_ptr, ref IrisRtcVideoFrame video_frame,
            out bool is_new_frame, uint uid, string channel_id = "");

        #endregion
    }
}