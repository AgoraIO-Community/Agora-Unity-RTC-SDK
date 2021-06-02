//  AgoraCallback.cs
//
//  Created by Yiqing Huang on May 25, 2021.
//  Modified by Yiqing Huang on May 25, 2021.
//
//  Copyright © 2021 Agora. All rights reserved.
//

using System;
using System.Runtime.InteropServices;

namespace agora_gaming_rtc
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate bool Func_Bool();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate uint Func_Uint32_t();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate void Func_Event(string @event, string data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate void Func_EventWithBuffer(string @event, string data, IntPtr buffer, uint length);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate bool Func_AudioFrameLocal(ref IrisRtcAudioFrame audio_frame);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate bool Func_AudioFrameRemote(uint uid, ref IrisRtcAudioFrame audio_frame);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate bool Func_AudioFrameEx(string channel_id, uint uid, ref IrisRtcAudioFrame audio_frame);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate bool Func_VideoFrameLocal(ref IrisRtcVideoFrame video_frame);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate bool Func_VideoFrameRemote(uint uid, ref IrisRtcVideoFrame video_frame);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate bool Func_VideoFrameEx(string channel_id, uint uid, ref IrisRtcVideoFrame video_frame);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate VideoFrameType Func_VideoFrameType();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate void Func_VideoFrame(ref IrisRtcVideoFrame video_frame, bool resize);

    [StructLayout(LayoutKind.Sequential)]
    internal struct IrisCEventHandler
    {
        internal Func_Event onEvent;
        internal Func_EventWithBuffer onEventWithBuffer;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct IrisRtcCAudioFrameObserver
    {
        internal Func_AudioFrameLocal OnRecordAudioFrame;
        internal Func_AudioFrameLocal OnPlaybackAudioFrame;
        internal Func_AudioFrameLocal OnMixedAudioFrame;
        internal Func_AudioFrameRemote OnPlaybackAudioFrameBeforeMixing;
        internal Func_Bool IsMultipleChannelFrameWanted;
        internal Func_AudioFrameEx OnPlaybackAudioFrameBeforeMixingEx;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct IrisRtcCVideoFrameObserver
    {
        internal Func_VideoFrameLocal OnCaptureVideoFrame;
        internal Func_VideoFrameLocal OnPreEncodeVideoFrame;
        internal Func_VideoFrameRemote OnRenderVideoFrame;
        internal Func_VideoFrameType GetVideoFormatPreference;
        internal Func_Uint32_t GetObservedFramePosition;
        internal Func_Bool IsMultipleChannelFrameWanted;
        internal Func_VideoFrameEx OnRenderVideoFrameEx;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct IrisRtcCRendererCacheConfig
    {
        internal VideoFrameType type;
        internal Func_VideoFrame OnVideoFrameReceived;
        internal int resize_width;
        internal int resize_height;
    }

    // TODO: Switch EventHandler Case.
}