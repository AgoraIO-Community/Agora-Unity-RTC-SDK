//  AgoraRtcApiNative.cs
//
//  Created by YuGuo Chen on September 26, 2021.
//
//  Copyright © 2021 Agora. All rights reserved.
//

using System;
using System.Runtime.InteropServices;

namespace agora.rtc
{
    using IrisRtcEnginePtr = IntPtr;
    using IrisRtcDeviceManagerPtr = IntPtr;
    using IrisRtcRawDataPtr = IntPtr;
    using IrisRtcRawDataPluginManagerPtr = IntPtr;
    using IrisRtcRendererPtr = IntPtr;
    using IrisEventHandlerHandle = IntPtr;
    using IrisRtcAudioFrameObserverHandle = IntPtr;
    using IrisRtcVideoFrameObserverHandle = IntPtr;
    using IrisRtcRendererCacheConfigHandle = IntPtr;
    using IrisVideoFrameBufferManagerPtr = IntPtr;
    using IrisVideoFrameBufferDelegateHandle = IntPtr;

    internal static class AgoraRtcNative
    {
        #region DllImport

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        private const string AgoraRtcLibName = "AgoraRtcWrapper";
#elif UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        private const string AgoraRtcLibName = "AgoraRtcWrapperUnity";
#elif UNITY_IPHONE
		private const string AgoraRtcLibName = "__Internal";
#else
        private const string AgoraRtcLibName = "AgoraRtcWrapper";
#endif

// IrisRtcEngine
        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IrisRtcEnginePtr CreateIrisRtcEngine();

        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DestroyIrisRtcEngine(IrisRtcEnginePtr engine_ptr);

        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IrisEventHandlerHandle SetIrisRtcEngineEventHandler(IrisRtcEnginePtr engine_ptr,
            IntPtr event_handler);

        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void UnsetIrisRtcEngineEventHandler(IrisRtcEnginePtr engine_ptr,
            IrisEventHandlerHandle handle);

        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int CallIrisRtcEngineApi(IrisRtcEnginePtr engine_ptr, ApiTypeEngine api_type,
            string @params, out CharAssistant result);

        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int CallIrisRtcEngineApiWithBuffer(IrisRtcEnginePtr engine_ptr, ApiTypeEngine api_type,
            string @params, byte[] buffer, out CharAssistant result);

        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IrisRtcDeviceManagerPtr GetIrisRtcDeviceManager(IrisRtcEnginePtr engine_ptr);

        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IrisRtcRawDataPtr GetIrisRtcRawData(IrisRtcEnginePtr engine_ptr);

// IrisRtcDeviceManager
        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int CallIrisRtcAudioDeviceManagerApi(IrisRtcDeviceManagerPtr device_manager_ptr,
            ApiTypeAudioDeviceManager api_type, string @params, out CharAssistant result);

        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int CallIrisRtcVideoDeviceManagerApi(IrisRtcDeviceManagerPtr device_manager_ptr,
            ApiTypeVideoDeviceManager api_type, string @params, out CharAssistant result);


// IrisRtcRawData
        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IrisRtcAudioFrameObserverHandle RegisterAudioFrameObserver(
            IrisRtcRawDataPtr raw_data_ptr, IntPtr observerNative, int order, string identifier);

        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void UnRegisterAudioFrameObserver(IrisRtcRawDataPtr raw_data_ptr,
            IrisRtcAudioFrameObserverHandle handle, string identifier);

        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IrisRtcVideoFrameObserverHandle RegisterVideoFrameObserver(
            IrisRtcRawDataPtr raw_data_ptr, IntPtr observerNative, int order, string identifier);

        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void UnRegisterVideoFrameObserver(IrisRtcRawDataPtr raw_data_ptr,
            IrisRtcVideoFrameObserverHandle handle, string identifier);


        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IrisRtcRawDataPluginManagerPtr GetIrisRtcRawDataPluginManager(
            IrisRtcRawDataPtr raw_data_ptr);

        // [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        // internal static extern IrisVideoFrameBufferManagerPtr GetVideoFrameBufferManagerPtr(IrisRtcRawDataPtr raw_data_ptr);


        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Attach(IrisRtcRawDataPtr raw_data_ptr, IrisVideoFrameBufferManagerPtr manager_ptr);

        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Detach(IrisRtcRawDataPtr raw_data_ptr, IrisVideoFrameBufferManagerPtr manager_ptr);

// IrisRtcRawDataPluginManager
        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int CallIrisRtcRawDataPluginManagerApi(IrisRtcRawDataPluginManagerPtr plugin_manager_ptr,
            ApiTypeRawDataPluginManager api_type, string @params, out CharAssistant result);

// IrisVideoFrameBufferManager
        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IrisVideoFrameBufferManagerPtr CreateIrisVideoFrameBufferManager();

        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void
        FreeIrisVideoFrameBufferManager(IrisVideoFrameBufferManagerPtr manager_ptr);

        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IrisEventHandlerHandle SetIrisVideoFrameBufferManagerEventHandler(
            IrisVideoFrameBufferManagerPtr manager_ptr,
            IrisCEventHandler event_handler);

        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void UnsetIrisVideoFrameBufferManagerEventHandler(
            IrisVideoFrameBufferManagerPtr manager_ptr, IrisEventHandlerHandle handle);

        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IrisVideoFrameBufferDelegateHandle EnableVideoFrameBuffer(
            IrisVideoFrameBufferManagerPtr manager_ptr, ref IrisCVideoFrameBufferNative buffer,
            uint uid = 0, string channel_id = "");

        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DisableVideoFrameBufferByDelegate(
            IrisVideoFrameBufferManagerPtr manager_ptr,
            IrisVideoFrameBufferDelegateHandle handle);

        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void
        DisableVideoFrameBufferByUid(IrisVideoFrameBufferManagerPtr manager_ptr,
                                    uint uid = 0, string channel_id = "");

        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void
        DisableAllVideoFrameBuffer(IrisVideoFrameBufferManagerPtr manager_ptr);

        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool GetVideoFrame(IrisVideoFrameBufferManagerPtr manager_ptr,
                                    ref IrisVideoFrame video_frame, out bool is_new_frame,
                                    uint uid, string channel_id = "");


// IrisRtcRenderer
        // [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        // internal static extern IrisRtcRendererCacheConfigHandle EnableVideoFrameCache(IrisRtcRendererPtr renderer_ptr,
        //     ref IrisRtcCRendererCacheConfigNative cacheConfigNative, uint uid = 0, string channel_id = "");

        // [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        // internal static extern void DisableVideoFrameCacheByUid(IrisRtcRendererPtr renderer_ptr, uint uid = 0,
        //     string channel_id = "");

        // [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        // internal static extern bool GetVideoFrame(IrisRtcRendererPtr renderer_ptr, ref IrisVideoFrame video_frame,
        //     out bool is_new_frame, uint uid, string channel_id = "");


// Iris Media Base
        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IrisVideoFrame ConvertVideoFrame(ref IrisVideoFrame src, VIDEO_FRAME_TYPE format);

        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ClearVideoFrame(ref IrisVideoFrame video_frame);



        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr EnumerateDisplays();

        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void FreeIrisDisplayCollection(IntPtr collection);

        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr EnumerateWindows();

        [DllImport(AgoraRtcLibName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void FreeIrisWindowCollection(IntPtr collection);


        #endregion

    }
}