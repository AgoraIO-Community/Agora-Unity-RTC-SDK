//  AgoraRtcEngine.cs
//
//  Created by Yiqing Huang on June 2, 2021.
//  Modified by Yiqing Huang on June 24, 2021.
//
//  Copyright © 2021 Agora. All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LitJson;

namespace agora_gaming_rtc
{
    using view_t = UInt64;
    using IrisRtcEnginePtr = IntPtr;
    using IrisEventHandlerHandleNative = IntPtr;
    using IrisCEventHandlerNativeMarshal = IntPtr;
    using IrisRtcDeviceManagerPtr = IntPtr;
    using IrisRtcVideoFrameObserverHandleNative = IntPtr;
    using IrisRtcCVideoFrameObserverNativeMarshal = IntPtr;
    using IrisRtcAudioFrameObserverHandleNative = IntPtr;
    using IrisRtcRendererPtr = IntPtr;
    using IrisRtcCAudioFrameObserverNativeMarshal = IntPtr;

    internal sealed class RtcEngineEventHandlerNative
    {
        private IAgoraRtcEngineEventHandler _engineEventHandler;

        private AgoraCallbackObject _callbackObject;

        private IrisRtcEnginePtr _irisRtcEnginePtr;
        private IrisEventHandlerHandleNative _irisEngineEventHandlerHandleNative;
        private IrisCEventHandler _irisCEventHandler;
        private IrisEventHandlerHandleNative _irisCEngineEventHandlerNative;

        internal RtcEngineEventHandlerNative(IrisRtcEnginePtr irisRtcEnginePtr)
        {
            _irisRtcEnginePtr = irisRtcEnginePtr;
            var name = "Agora" + GetHashCode().ToString();

            _irisCEventHandler = new IrisCEventHandler()
            {
                OnEvent = OnEvent,
                OnEventWithBuffer = OnEventWithBuffer
            };

            var cEventHandlerNativeLocal = new IrisCEventHandlerNative
            {
                onEvent = Marshal.GetFunctionPointerForDelegate(_irisCEventHandler.OnEvent),
                onEventWithBuffer =
                    Marshal.GetFunctionPointerForDelegate(_irisCEventHandler.OnEventWithBuffer)
            };

            _irisCEngineEventHandlerNative = Marshal.AllocHGlobal(Marshal.SizeOf(cEventHandlerNativeLocal));
            Marshal.StructureToPtr(cEventHandlerNativeLocal, _irisCEngineEventHandlerNative, true);
            _irisEngineEventHandlerHandleNative =
                AgoraRtcNative.SetIrisRtcEngineEventHandler(_irisRtcEnginePtr, _irisCEngineEventHandlerNative);

            _callbackObject = new AgoraCallbackObject(name);
        }

        internal void Dispose()
        {
            _engineEventHandler = null;
            if (_callbackObject != null) _callbackObject.Release();
            _callbackObject = null;
            AgoraRtcNative.UnsetIrisRtcEngineEventHandler(_irisRtcEnginePtr, _irisEngineEventHandlerHandleNative);
            Marshal.FreeHGlobal(_irisCEngineEventHandlerNative);
            _irisEngineEventHandlerHandleNative = IntPtr.Zero;
        }

        internal void SetEventHandler(IAgoraRtcEngineEventHandler engineEventHandler)
        {
            _engineEventHandler = engineEventHandler;
        }

        internal void OnEvent(string @event, string data)
        {
            if (_callbackObject == null || _callbackObject._CallbackQueue == null) return;
            switch (@event)
            {
                case "onWarning":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnWarning((int) AgoraJson.GetData<int>(data, "warn"),
                                (string) AgoraJson.GetData<string>(data, "msg"));
                        }
                    });
                    break;
                case "onError":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnError((int) AgoraJson.GetData<int>(data, "err"),
                                (string) AgoraJson.GetData<string>(data, "msg"));
                        }
                    });
                    break;
                case "onJoinChannelSuccess":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnJoinChannelSuccess(
                                (string) AgoraJson.GetData<string>(data, "channel"),
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (int) AgoraJson.GetData<int>(data, "elapsed"));
                        }
                    });
                    break;
                case "onRejoinChannelSuccess":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnRejoinChannelSuccess(
                                (string) AgoraJson.GetData<string>(data, "channel"),
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (int) AgoraJson.GetData<int>(data, "elapsed"));
                        }
                    });
                    break;
                case "onLeaveChannel":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnLeaveChannel(
                                AgoraJson.JsonToStruct<RtcStats>(data, "stats"));
                        }
                    });
                    break;
                case "onClientRoleChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnClientRoleChanged(
                                (CLIENT_ROLE_TYPE) AgoraJson.GetData<int>(data, "oldRole"),
                                (CLIENT_ROLE_TYPE) AgoraJson.GetData<int>(data, "newRole"));
                        }
                    });
                    break;
                case "onUserJoined":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnUserJoined((uint) AgoraJson.GetData<uint>(data, "uid"),
                                (int) AgoraJson.GetData<int>(data, "elapsed"));
                        }
                    });
                    break;
                case "onUserOffline":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnUserOffline((uint) AgoraJson.GetData<uint>(data, "uid"),
                                (USER_OFFLINE_REASON_TYPE) AgoraJson.GetData<int>(data, "reason"));
                        }
                    });
                    break;
                case "onLastmileQuality":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnLastmileQuality(
                                (int) AgoraJson.GetData<int>(data, "quality"));
                        }
                    });
                    break;
                case "onLastmileProbeResult":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnLastmileProbeResult(
                                AgoraJson.JsonToStruct<LastmileProbeResult>(data, "result"));
                        }
                    });
                    break;
                case "onConnectionInterrupted":
                    _callbackObject._CallbackQueue.EnQueue(
                        () =>
                        {
                            if (_engineEventHandler != null)
                            {
                                _engineEventHandler.OnConnectionInterrupted();
                            }
                        });
                    break;
                case "onConnectionLost":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnConnectionLost();
                        }
                    });
                    break;
                case "onConnectionBanned":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnConnectionBanned();
                        }
                    });
                    break;
                case "onApiCallExecuted":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnApiCallExecuted(
                                (int) AgoraJson.GetData<int>(data, "err"),
                                (string) AgoraJson.GetData<string>(data, "api"),
                                (string) AgoraJson.GetData<string>(data, "result"));
                        }
                    });
                    break;
                case "onRequestToken":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnRequestToken();
                        }
                    });
                    break;
                case "onTokenPrivilegeWillExpire":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnTokenPrivilegeWillExpire(
                                (string) AgoraJson.GetData<string>(data, "token"));
                        }
                    });
                    break;
                case "onAudioQuality":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnAudioQuality((uint) AgoraJson.GetData<uint>(data, "uid"),
                                (int) AgoraJson.GetData<int>(data, "quality"),
                                (ushort) AgoraJson.GetData<ushort>(data, "delay"),
                                (ushort) AgoraJson.GetData<ushort>(data, "lost"));
                        }
                    });
                    break;
                case "onRtcStats":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnRtcStats(
                                AgoraJson.JsonToStruct<RtcStats>(data, "stats"));
                        }
                    });
                    break;
                case "onNetworkQuality":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnNetworkQuality((uint) AgoraJson.GetData<uint>(data, "uid"),
                                (int) AgoraJson.GetData<int>(data, "txQuality"),
                                (int) AgoraJson.GetData<int>(data, "rxQuality"));
                        }
                    });
                    break;
                case "onLocalVideoStats":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnLocalVideoStats(
                                AgoraJson.JsonToStruct<LocalVideoStats>(data, "stats"));
                        }
                    });
                    break;
                case "onRemoteVideoStats":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnRemoteVideoStats(
                                AgoraJson.JsonToStruct<RemoteVideoStats>(data, "stats"));
                        }
                    });
                    break;
                case "onLocalAudioStats":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnLocalAudioStats(
                                AgoraJson.JsonToStruct<LocalAudioStats>(data, "stats"));
                        }
                    });
                    break;
                case "onRemoteAudioStats":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnRemoteAudioStats(
                                AgoraJson.JsonToStruct<RemoteAudioStats>(data, "stats"));
                        }
                    });
                    break;
                case "onLocalAudioStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnLocalAudioStateChanged(
                                (LOCAL_AUDIO_STREAM_STATE) AgoraJson.GetData<int>(data, "state"),
                                (LOCAL_AUDIO_STREAM_ERROR) AgoraJson.GetData<int>(data, "error"));
                        }
                    });
                    break;
                case "onRemoteAudioStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnRemoteAudioStateChanged(
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (REMOTE_AUDIO_STATE) AgoraJson.GetData<int>(data, "state"),
                                (REMOTE_AUDIO_STATE_REASON) AgoraJson.GetData<int>(data, "reason"),
                                (int) AgoraJson.GetData<int>(data, "elapsed"));
                        }
                    });
                    break;
                case "onAudioPublishStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnAudioPublishStateChanged(
                                (string) AgoraJson.GetData<string>(data, "channel"),
                                (STREAM_PUBLISH_STATE) AgoraJson.GetData<int>(data, "oldState"),
                                (STREAM_PUBLISH_STATE) AgoraJson.GetData<int>(data, "newState"),
                                (int) AgoraJson.GetData<int>(data, "elapseSinceLastState"));
                        }
                    });
                    break;
                case "onVideoPublishStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnVideoPublishStateChanged(
                                (string) AgoraJson.GetData<string>(data, "channel"),
                                (STREAM_PUBLISH_STATE) AgoraJson.GetData<int>(data, "oldState"),
                                (STREAM_PUBLISH_STATE) AgoraJson.GetData<int>(data, "newState"),
                                (int) AgoraJson.GetData<int>(data, "elapseSinceLastState"));
                        }
                    });
                    break;
                case "onAudioSubscribeStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnAudioSubscribeStateChanged(
                                (string) AgoraJson.GetData<string>(data, "channel"),
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (STREAM_SUBSCRIBE_STATE) AgoraJson.GetData<int>(data, "oldState"),
                                (STREAM_SUBSCRIBE_STATE) AgoraJson.GetData<int>(data, "newState"),
                                (int) AgoraJson.GetData<int>(data, "elapseSinceLastState"));
                        }
                    });
                    break;
                case "onVideoSubscribeStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnVideoSubscribeStateChanged(
                                (string) AgoraJson.GetData<string>(data, "channel"),
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (STREAM_SUBSCRIBE_STATE) AgoraJson.GetData<int>(data, "oldState"),
                                (STREAM_SUBSCRIBE_STATE) AgoraJson.GetData<int>(data, "newState"),
                                (int) AgoraJson.GetData<int>(data, "elapseSinceLastState"));
                        }
                    });
                    break;
                case "onAudioVolumeIndication":
                    var speakerNumber = (uint) AgoraJson.GetData<uint>(data, "speakerNumber");
                    var speakers = AgoraJson.JsonToStructArray<AudioVolumeInfo>(data, "speakers", speakerNumber);
                    var totalVolume = (int) AgoraJson.GetData<int>(data, "totalVolume");
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnAudioVolumeIndication(speakers, speakerNumber, totalVolume);
                        }
                    });
                    break;
                case "onActiveSpeaker":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnActiveSpeaker((uint) AgoraJson.GetData<uint>(data, "uid"));
                        }
                    });
                    break;
                case "onVideoStopped":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnVideoStopped();
                        }
                    });
                    break;
                case "onFirstLocalVideoFrame":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnFirstLocalVideoFrame(
                                (int) AgoraJson.GetData<int>(data, "width"),
                                (int) AgoraJson.GetData<int>(data, "height"),
                                (int) AgoraJson.GetData<int>(data, "elapsed"));
                        }
                    });
                    break;
                case "onFirstLocalVideoFramePublished":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnFirstLocalVideoFramePublished(
                                (int) AgoraJson.GetData<int>(data, "elapsed"));
                        }
                    });
                    break;
                case "onFirstRemoteVideoDecoded":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnFirstRemoteVideoDecoded(
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (int) AgoraJson.GetData<int>(data, "width"),
                                (int) AgoraJson.GetData<int>(data, "height"),
                                (int) AgoraJson.GetData<int>(data, "elapsed"));
                        }
                    });
                    break;
                case "onFirstRemoteVideoFrame":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnFirstRemoteVideoFrame(
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (int) AgoraJson.GetData<int>(data, "width"),
                                (int) AgoraJson.GetData<int>(data, "height"),
                                (int) AgoraJson.GetData<int>(data, "elapsed"));
                        }
                    });
                    break;
                case "onUserMuteAudio":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnUserMuteAudio((uint) AgoraJson.GetData<uint>(data, "uid"),
                                (bool) AgoraJson.GetData<bool>(data, "muted"));
                        }
                    });
                    break;
                case "onUserMuteVideo":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnUserMuteVideo((uint) AgoraJson.GetData<uint>(data, "uid"),
                                (bool) AgoraJson.GetData<bool>(data, "muted"));
                        }
                    });
                    break;
                case "onUserEnableVideo":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnUserEnableVideo((uint) AgoraJson.GetData<uint>(data, "uid"),
                                (bool) AgoraJson.GetData<bool>(data, "enabled"));
                        }
                    });
                    break;
                case "onAudioDeviceStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnAudioDeviceStateChanged(
                                (string) AgoraJson.GetData<string>(data, "deviceId"),
                                (int) AgoraJson.GetData<int>(data, "deviceType"),
                                (int) AgoraJson.GetData<int>(data, "deviceState"));
                        }
                    });
                    break;
                case "onAudioDeviceVolumeChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnAudioDeviceVolumeChanged(
                                (MEDIA_DEVICE_TYPE) AgoraJson.GetData<int>(data, "deviceType"),
                                (int) AgoraJson.GetData<int>(data, "volume"),
                                (bool) AgoraJson.GetData<bool>(data, "muted"));
                        }
                    });
                    break;
                case "onCameraReady":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnCameraReady();
                        }
                    });
                    break;
                case "onCameraFocusAreaChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnCameraFocusAreaChanged(
                                (int) AgoraJson.GetData<int>(data, "x"),
                                (int) AgoraJson.GetData<int>(data, "y"), (int) AgoraJson.GetData<int>(data, "width"),
                                (int) AgoraJson.GetData<int>(data, "height"));
                        }
                    });
                    break;
                case "onFacePositionChanged":
                    var numFaces = (int) AgoraJson.GetData<int>(data, "numFaces");
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnFacePositionChanged(
                                (int) AgoraJson.GetData<int>(data, "imageWidth"),
                                (int) AgoraJson.GetData<int>(data, "imageHeight"),
                                AgoraJson.JsonToStruct<Rectangle>(
                                    (string) AgoraJson.GetData<string>(data, "vecRectangle")),
                                AgoraJson.JsonToStructArray<int>(data, "vecDistance", (uint) numFaces), numFaces);
                        }
                    });
                    break;
                case "onCameraExposureAreaChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnCameraExposureAreaChanged(
                                (int) AgoraJson.GetData<int>(data, "x"),
                                (int) AgoraJson.GetData<int>(data, "y"), (int) AgoraJson.GetData<int>(data, "width"),
                                (int) AgoraJson.GetData<int>(data, "height"));
                        }
                    });
                    break;
                case "onAudioMixingFinished":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnAudioMixingFinished();
                        }
                    });
                    break;
                case "onAudioMixingStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnAudioMixingStateChanged(
                                (AUDIO_MIXING_STATE_TYPE) AgoraJson.GetData<int>(data, "state"),
                                (AUDIO_MIXING_ERROR_TYPE) AgoraJson.GetData<int>(data, "errorCode"));
                        }
                    });
                    break;
                case "onRemoteAudioMixingBegin":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnRemoteAudioMixingBegin();
                        }
                    });
                    break;
                case "onRemoteAudioMixingEnd":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnRemoteAudioMixingEnd();
                        }
                    });
                    break;
                case "onAudioEffectFinished":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnAudioEffectFinished(
                                (int) AgoraJson.GetData<int>(data, "soundId"));
                        }
                    });
                    break;
                case "onFirstRemoteAudioDecoded":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnFirstRemoteAudioDecoded(
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (int) AgoraJson.GetData<int>(data, "elapsed"));
                        }
                    });
                    break;
                case "onVideoDeviceStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnVideoDeviceStateChanged(
                                (string) AgoraJson.GetData<string>(data, "deviceId"),
                                (int) AgoraJson.GetData<int>(data, "deviceType"),
                                (int) AgoraJson.GetData<int>(data, "deviceState"));
                        }
                    });
                    break;
                case "onLocalVideoStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnLocalVideoStateChanged(
                                (LOCAL_VIDEO_STREAM_STATE) AgoraJson.GetData<int>(data, "localVideoState"),
                                (LOCAL_VIDEO_STREAM_ERROR) AgoraJson.GetData<int>(data, "error"));
                        }
                    });
                    break;
                case "onVideoSizeChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnVideoSizeChanged((uint) AgoraJson.GetData<uint>(data, "uid"),
                                (int) AgoraJson.GetData<int>(data, "width"),
                                (int) AgoraJson.GetData<int>(data, "height"),
                                (int) AgoraJson.GetData<int>(data, "rotation"));
                        }
                    });
                    break;
                case "onRemoteVideoStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnRemoteVideoStateChanged(
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (REMOTE_VIDEO_STATE) AgoraJson.GetData<int>(data, "state"),
                                (REMOTE_VIDEO_STATE_REASON) AgoraJson.GetData<int>(data, "reason"),
                                (int) AgoraJson.GetData<int>(data, "elapsed"));
                        }
                    });
                    break;
                case "onUserEnableLocalVideo":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnUserEnableLocalVideo(
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (bool) AgoraJson.GetData<bool>(data, "enabled"));
                        }
                    });
                    break;
                case "onStreamMessageError":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnStreamMessageError(
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (int) AgoraJson.GetData<int>(data, "streamId"),
                                (int) AgoraJson.GetData<int>(data, "code"),
                                (int) AgoraJson.GetData<int>(data, "missed"),
                                (int) AgoraJson.GetData<int>(data, "cached"));
                        }
                    });
                    break;
                case "onMediaEngineLoadSuccess":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnMediaEngineLoadSuccess();
                        }
                    });
                    break;
                case "onMediaEngineStartCallSuccess":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnMediaEngineStartCallSuccess();
                        }
                    });
                    break;
                case "onUserSuperResolutionEnabled":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnUserSuperResolutionEnabled(
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (bool) AgoraJson.GetData<bool>(data, "enabled"),
                                (SUPER_RESOLUTION_STATE_REASON) AgoraJson.GetData<int>(data, "reason"));
                        }
                    });
                    break;
                case "onChannelMediaRelayStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnChannelMediaRelayStateChanged(
                                (CHANNEL_MEDIA_RELAY_STATE) AgoraJson.GetData<int>(data, "state"),
                                (CHANNEL_MEDIA_RELAY_ERROR) AgoraJson.GetData<int>(data, "code"));
                        }
                    });
                    break;
                case "onChannelMediaRelayEvent":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnChannelMediaRelayEvent(
                                (CHANNEL_MEDIA_RELAY_EVENT) AgoraJson.GetData<int>(data, "code"));
                        }
                    });
                    break;
                case "onFirstLocalAudioFrame":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnFirstLocalAudioFrame(
                                (int) AgoraJson.GetData<int>(data, "elapsed"));
                        }
                    });
                    break;
                case "onFirstLocalAudioFramePublished":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnFirstLocalAudioFramePublished(
                                (int) AgoraJson.GetData<int>(data, "elapsed"));
                        }
                    });
                    break;
                case "onFirstRemoteAudioFrame":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnFirstRemoteAudioFrame(
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (int) AgoraJson.GetData<int>(data, "elapsed"));
                        }
                    });
                    break;
                case "onRtmpStreamingStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnRtmpStreamingStateChanged(
                                (string) AgoraJson.GetData<string>(data, "url"),
                                (RTMP_STREAM_PUBLISH_STATE) AgoraJson.GetData<int>(data, "state"),
                                (RTMP_STREAM_PUBLISH_ERROR) AgoraJson.GetData<int>(data, "errCode"));
                        }
                    });
                    break;
                case "onRtmpStreamingEvent":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnRtmpStreamingEvent(
                                (string) AgoraJson.GetData<string>(data, "url"),
                                (RTMP_STREAMING_EVENT) AgoraJson.GetData<int>(data, "eventCode"));
                        }
                    });
                    break;
                case "onStreamPublished":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnStreamPublished(
                                (string) AgoraJson.GetData<string>(data, "url"),
                                (int) AgoraJson.GetData<int>(data, "error"));
                        }
                    });
                    break;
                case "onStreamUnpublished":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnStreamUnpublished(
                                (string) AgoraJson.GetData<string>(data, "url"));
                        }
                    });
                    break;
                case "onTranscodingUpdated":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnTranscodingUpdated();
                        }
                    });
                    break;
                case "onStreamInjectedStatus":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnStreamInjectedStatus(
                                (string) AgoraJson.GetData<string>(data, "url"),
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (int) AgoraJson.GetData<int>(data, "status"));
                        }
                    });
                    break;
                case "onAudioRouteChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnAudioRouteChanged(
                                (AUDIO_ROUTE_TYPE) AgoraJson.GetData<int>(data, "routing"));
                        }
                    });
                    break;
                case "onLocalPublishFallbackToAudioOnly":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnLocalPublishFallbackToAudioOnly(
                                (bool) AgoraJson.GetData<bool>(data, "isFallbackOrRecover"));
                        }
                    });
                    break;
                case "onRemoteSubscribeFallbackToAudioOnly":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnRemoteSubscribeFallbackToAudioOnly(
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (bool) AgoraJson.GetData<bool>(data, "isFallbackOrRecover"));
                        }
                    });
                    break;
                case "onRemoteAudioTransportStats":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnRemoteAudioTransportStats(
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (ushort) AgoraJson.GetData<ushort>(data, "delay"),
                                (ushort) AgoraJson.GetData<ushort>(data, "lost"),
                                (ushort) AgoraJson.GetData<ushort>(data, "rxKBitRate"));
                        }
                    });
                    break;
                case "onRemoteVideoTransportStats":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnRemoteVideoTransportStats(
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (ushort) AgoraJson.GetData<ushort>(data, "delay"),
                                (ushort) AgoraJson.GetData<ushort>(data, "lost"),
                                (ushort) AgoraJson.GetData<ushort>(data, "rxKBitRate"));
                        }
                    });
                    break;
                case "onMicrophoneEnabled":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnMicrophoneEnabled(
                                (bool) AgoraJson.GetData<bool>(data, "enabled"));
                        }
                    });
                    break;
                case "onConnectionStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnConnectionStateChanged(
                                (CONNECTION_STATE_TYPE) AgoraJson.GetData<int>(data, "state"),
                                (CONNECTION_CHANGED_REASON_TYPE) AgoraJson.GetData<int>(data, "reason"));
                        }
                    });
                    break;
                case "onNetworkTypeChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnNetworkTypeChanged(
                                (NETWORK_TYPE) AgoraJson.GetData<int>(data, "type"));
                        }
                    });
                    break;
                case "onLocalUserRegistered":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnLocalUserRegistered(
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (string) AgoraJson.GetData<string>(data, "userAccount"));
                        }
                    });
                    break;
                case "onUserInfoUpdated":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnUserInfoUpdated((uint) AgoraJson.GetData<uint>(data, "uid"),
                                AgoraJson.JsonToStruct<UserInfo>(data, "info"));
                        }
                    });
                    break;
                case "onUploadLogResult":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnUploadLogResult(
                                (string) AgoraJson.GetData<string>(data, "requestId"),
                                (bool) AgoraJson.GetData<bool>(data, "success"),
                                (UPLOAD_ERROR_REASON) AgoraJson.GetData<int>(data, "reason"));
                        }
                    });
                    break;
            }
        }

        internal void OnEventWithBuffer(string @event, string data, IntPtr buffer, uint length)
        {
            var byteData = new byte[length];
            if (buffer != IntPtr.Zero) Marshal.Copy(buffer, byteData, 0, (int) length);
            if (_callbackObject == null || _callbackObject._CallbackQueue == null) return;
            switch (@event)
            {
                case "onStreamMessage":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler != null)
                        {
                            _engineEventHandler.OnStreamMessage((uint) AgoraJson.GetData<uint>(data, "uid"),
                                (int) AgoraJson.GetData<int>(data, "streamId"), byteData, length);
                        }
                    });
                    break;
                case "onReadyToSendMetadata":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler == null) return;
                        var metadata = new Metadata((uint) AgoraJson.GetData<uint>(data, "uid"),
                            (uint) AgoraJson.GetData<uint>(data, "size"), byteData,
                            (long) AgoraJson.GetData<long>(data, "timeStampMs"));
                        _engineEventHandler.OnReadyToSendMetadata(metadata);
                    });
                    break;
                case "onMetadataReceived":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_engineEventHandler == null) return;
                        var metadata = new Metadata((uint) AgoraJson.GetData<uint>(data, "uid"),
                            (uint) AgoraJson.GetData<uint>(data, "size"), byteData,
                            (long) AgoraJson.GetData<long>(data, "timeStampMs"));
                        _engineEventHandler.OnMetadataReceived(metadata);
                    });
                    break;
            }
        }
    }

    public sealed class AgoraRtcEngine : IAgoraRtcEngine
    {
        private bool _disposed;

        private static readonly AgoraRtcEngine[] engineInstance = {null, null};

        private static readonly string[] identifier = {"UnityRtcMainProcess", "UnityRtcSubProcess"};

        private IrisRtcEnginePtr _irisRtcEngine;

        private RtcEngineEventHandlerNative _rtcEngineEventHandlerNative;

        private readonly Dictionary<string, AgoraRtcChannel> _channelInstance;

        private IrisRtcDeviceManagerPtr _irisRtcDeviceManager;
        private AgoraRtcVideoDeviceManager _videoDeviceManagerInstance;
        private VideoDeviceManager _deprecatedVideoDeviceManagerInstance;
        private AgoraRtcAudioPlaybackDeviceManager _audioPlaybackDeviceManagerInstance;
        private AudioPlaybackDeviceManager _deprecatedAudioPlaybackDeviceManagerInstance;
        private AgoraRtcAudioRecordingDeviceManager _audioRecordingDeviceManagerInstance;
        private AudioRecordingDeviceManager _deprecatedAudioRecordingDeviceManagerInstance;
        private AudioEffectManager _deprecatedAudioEffectManagerInstance;

        private RtcAudioFrameObserverNative _rtcAudioFrameObserverNative;
        private IrisRtcCAudioFrameObserverNativeMarshal _irisRtcCAudioFrameObserverNative;
        private IrisRtcCAudioFrameObserver _irisRtcCAudioFrameObserver;
        private IrisRtcAudioFrameObserverHandleNative _irisRtcAudioFrameObserverHandleNative;

        private RtcVideoFrameObserverNative _rtcVideoFrameObserverNative;
        private IrisRtcCVideoFrameObserverNativeMarshal _irisRtcCVideoFrameObserverNative;
        private IrisRtcCVideoFrameObserver _irisRtcCVideoFrameObserver;
        private IrisRtcVideoFrameObserverHandleNative _irisRtcVideoFrameObserverHandleNative;

        private CharArrayAssistant _result;

        private AgoraRtcEngine(EngineType type = EngineType.kEngineTypeNormal)
        {
            _result = new CharArrayAssistant();
            _channelInstance = new Dictionary<string, AgoraRtcChannel>();
            _irisRtcEngine = type == EngineType.kEngineTypeNormal
                ? AgoraRtcNative.CreateIrisRtcEngine()
                : AgoraRtcNative.CreateIrisRtcEngine(EngineType.kEngineTypeSubProcess);

            _rtcEngineEventHandlerNative = new RtcEngineEventHandlerNative(_irisRtcEngine);

            _irisRtcDeviceManager = AgoraRtcNative.GetIrisRtcDeviceManager(_irisRtcEngine);

            _videoDeviceManagerInstance = new AgoraRtcVideoDeviceManager(_irisRtcDeviceManager);
            _deprecatedVideoDeviceManagerInstance = new VideoDeviceManager(_videoDeviceManagerInstance);

            _audioPlaybackDeviceManagerInstance = new AgoraRtcAudioPlaybackDeviceManager(_irisRtcDeviceManager);
            _deprecatedAudioPlaybackDeviceManagerInstance =
                new AudioPlaybackDeviceManager(_audioPlaybackDeviceManagerInstance);

            _audioRecordingDeviceManagerInstance = new AgoraRtcAudioRecordingDeviceManager(_irisRtcDeviceManager);
            _deprecatedAudioRecordingDeviceManagerInstance =
                new AudioRecordingDeviceManager(_audioRecordingDeviceManagerInstance);

            _deprecatedAudioEffectManagerInstance =
                new AudioEffectManager(type == EngineType.kEngineTypeNormal ? engineInstance[0] : engineInstance[1]);
        }

        private void Dispose(bool disposing, bool sync)
        {
            if (_disposed) return;

            if (disposing)
            {
                if (_rtcEngineEventHandlerNative != null) _rtcEngineEventHandlerNative.Dispose();
                _rtcEngineEventHandlerNative = null;
                // TODO: Unmanaged resources.
                UnSetIrisAudioFrameObserver();
                UnSetIrisVideoFrameObserver();

                foreach (var channelInstance in _channelInstance.Values)
                {
                    channelInstance.Dispose();
                }

                _deprecatedVideoDeviceManagerInstance.Dispose();
                _deprecatedVideoDeviceManagerInstance = null;
                _videoDeviceManagerInstance.Dispose();
                _videoDeviceManagerInstance = null;

                _deprecatedAudioPlaybackDeviceManagerInstance.Dispose();
                _deprecatedAudioPlaybackDeviceManagerInstance = null;
                _audioPlaybackDeviceManagerInstance.Dispose();
                _audioPlaybackDeviceManagerInstance = null;

                _deprecatedAudioRecordingDeviceManagerInstance.Dispose();
                _deprecatedAudioRecordingDeviceManagerInstance = null;
                _audioRecordingDeviceManagerInstance.Dispose();
                _audioRecordingDeviceManagerInstance = null;

                _deprecatedAudioEffectManagerInstance.Dispose();
                _deprecatedAudioEffectManagerInstance = null;

                _irisRtcDeviceManager = IntPtr.Zero;
            }

            Release(sync);

            _disposed = true;
        }

        private void Release(bool sync = false)
        {
            var param = new
            {
                sync
            };

            AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineRelease,
                JsonMapper.ToJson(param), out _result);
            AgoraRtcNative.DestroyIrisRtcEngine(_irisRtcEngine);
            _irisRtcEngine = IntPtr.Zero;
            _result = new CharArrayAssistant();
            for (var i = 0; i < engineInstance.Length; i++)
            {
                if (engineInstance[i] == this) engineInstance[i] = null;
            }
        }

        internal IrisRtcEnginePtr GetNativeHandler()
        {
            return _irisRtcEngine;
        }

        public static IAgoraRtcEngine CreateAgoraRtcEngine(AgoraEngineType engineType = AgoraEngineType.MainProcess)
        {
            switch (engineType)
            {
                case AgoraEngineType.MainProcess:
                    return engineInstance[0] ?? (engineInstance[0] = new AgoraRtcEngine());
                case AgoraEngineType.SubProcess:
                    return engineInstance[1] ??
                           (engineInstance[1] = new AgoraRtcEngine(EngineType.kEngineTypeSubProcess));
                default:
                    throw new ArgumentOutOfRangeException("", engineType, null);
            }
        }

        [Obsolete(
            "This method is deprecated. Please call CreateAgoraRtcEngine and Initialize instead",
            false)]
        public static IRtcEngine GetEngine(string appId)
        {
            var agoraRtcEngine = CreateAgoraRtcEngine();
            agoraRtcEngine.Initialize(new RtcEngineContext(appId));
            return agoraRtcEngine;
        }

        [Obsolete(
            "This method is deprecated. Please call CreateAgoraRtcEngine and Initialize instead",
            false)]
        public static IRtcEngine GetEngine(RtcEngineConfig engineConfig)
        {
            var agoraRtcEngine = CreateAgoraRtcEngine();
            agoraRtcEngine.Initialize(new RtcEngineContext(engineConfig.appId, engineConfig.areaCode,
                engineConfig.logConfig));
            return agoraRtcEngine;
        }

        [Obsolete("This method is deprecated. Please call CreateAgoraRtcEngine instead.", false)]
        public static IRtcEngine QueryEngine()
        {
            return engineInstance[0];
        }

        public static IAgoraRtcEngine Get(AgoraEngineType engineType = AgoraEngineType.MainProcess)
        {
            switch (engineType)
            {
                case AgoraEngineType.MainProcess:
                    return engineInstance[0];
                case AgoraEngineType.SubProcess:
                    return engineInstance[1];
                default:
                    throw new ArgumentOutOfRangeException("", engineType, null);
            }
        }

        private int SetAppType(AppType appType)
        {
            var param = new
            {
                appType
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetAppType,
                JsonMapper.ToJson(param), out _result);
        }

        public override int Initialize(RtcEngineContext context)
        {
            var param = new
            {
                context
            };
            var ret = AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineInitialize,
                JsonMapper.ToJson(param), out _result);
            if (ret == 0) SetAppType(AppType.APP_TYPE_UNITY);
            return ret;
        }

        public override void InitEventHandler(IAgoraRtcEngineEventHandler engineEventHandler)
        {
            if (_rtcEngineEventHandlerNative != null) _rtcEngineEventHandlerNative.SetEventHandler(engineEventHandler);
        }

        public override void RegisterAudioFrameObserver(IAgoraRtcAudioFrameObserver audioFrameObserver)
        {
            SetIrisAudioFrameObserver();
            _rtcAudioFrameObserverNative.SetAudioFrameObserver(audioFrameObserver);
        }

        private void SetIrisAudioFrameObserver()
        {
            if (_rtcAudioFrameObserverNative != null) return;

            _rtcAudioFrameObserverNative = new RtcAudioFrameObserverNative();

            _irisRtcCAudioFrameObserver = new IrisRtcCAudioFrameObserver
            {
                OnRecordAudioFrame = _rtcAudioFrameObserverNative.OnRecordAudioFrame,
                OnPlaybackAudioFrame = _rtcAudioFrameObserverNative.OnPlaybackAudioFrame,
                OnMixedAudioFrame = _rtcAudioFrameObserverNative.OnMixedAudioFrame,
                OnPlaybackAudioFrameBeforeMixing = _rtcAudioFrameObserverNative.OnPlaybackAudioFrameBeforeMixing,
                IsMultipleChannelFrameWanted = _rtcAudioFrameObserverNative.IsMultipleChannelFrameWanted,
                OnPlaybackAudioFrameBeforeMixingEx = _rtcAudioFrameObserverNative.OnPlaybackAudioFrameBeforeMixingEx
            };

            var irisRtcCAudioFrameObserverNativeLocal = new IrisRtcCAudioFrameObserverNative
            {
                OnRecordAudioFrame =
                    Marshal.GetFunctionPointerForDelegate(_irisRtcCAudioFrameObserver.OnRecordAudioFrame),
                OnPlaybackAudioFrame =
                    Marshal.GetFunctionPointerForDelegate(_irisRtcCAudioFrameObserver.OnPlaybackAudioFrame),
                OnMixedAudioFrame =
                    Marshal.GetFunctionPointerForDelegate(_irisRtcCAudioFrameObserver.OnMixedAudioFrame),
                OnPlaybackAudioFrameBeforeMixing =
                    Marshal.GetFunctionPointerForDelegate(_irisRtcCAudioFrameObserver.OnPlaybackAudioFrameBeforeMixing),
                IsMultipleChannelFrameWanted =
                    Marshal.GetFunctionPointerForDelegate(_irisRtcCAudioFrameObserver.IsMultipleChannelFrameWanted),
                OnPlaybackAudioFrameBeforeMixingEx =
                    Marshal.GetFunctionPointerForDelegate(
                        _irisRtcCAudioFrameObserver.OnPlaybackAudioFrameBeforeMixingEx)
            };

            _irisRtcCAudioFrameObserverNative =
                Marshal.AllocHGlobal(Marshal.SizeOf(irisRtcCAudioFrameObserverNativeLocal));

            _irisRtcAudioFrameObserverHandleNative = AgoraRtcNative.RegisterAudioFrameObserver(
                AgoraRtcNative.GetIrisRtcRawData(_irisRtcEngine), _irisRtcCAudioFrameObserverNative, 0,
                this == engineInstance[0] ? identifier[0] : identifier[1]);
        }

        private void UnSetIrisAudioFrameObserver()
        {
            AgoraRtcNative.UnRegisterAudioFrameObserver(AgoraRtcNative.GetIrisRtcRawData(_irisRtcEngine),
                _irisRtcAudioFrameObserverHandleNative, this == engineInstance[0] ? identifier[0] : identifier[1]);
            _irisRtcAudioFrameObserverHandleNative = IntPtr.Zero;
            if (_rtcAudioFrameObserverNative != null) _rtcAudioFrameObserverNative.Dispose();
            _rtcAudioFrameObserverNative = null;
            _irisRtcCAudioFrameObserver = new IrisRtcCAudioFrameObserver();
            Marshal.FreeHGlobal(_irisRtcCAudioFrameObserverNative);
        }

        public override void RegisterVideoFrameObserver(IAgoraRtcVideoFrameObserver videoFrameObserver)
        {
            SetIrisVideoFrameObserver();
            _rtcVideoFrameObserverNative.SetVideoFrameObserver(videoFrameObserver);
        }

        private void SetIrisVideoFrameObserver()
        {
            if (_rtcVideoFrameObserverNative != null) return;

            _rtcVideoFrameObserverNative = new RtcVideoFrameObserverNative();

            _irisRtcCVideoFrameObserver = new IrisRtcCVideoFrameObserver
            {
                OnCaptureVideoFrame = _rtcVideoFrameObserverNative.OnCaptureVideoFrame,
                OnPreEncodeVideoFrame = _rtcVideoFrameObserverNative.OnPreEncodeVideoFrame,
                OnRenderVideoFrame = _rtcVideoFrameObserverNative.OnRenderVideoFrame,
                GetObservedFramePosition = _rtcVideoFrameObserverNative.GetObservedFramePosition,
                IsMultipleChannelFrameWanted = _rtcVideoFrameObserverNative.IsMultipleChannelFrameWanted,
                OnRenderVideoFrameEx = _rtcVideoFrameObserverNative.OnRenderVideoFrameEx
            };

            var irisRtcCVideoFrameObserverNativeLocal = new IrisRtcCVideoFrameObserverNative
            {
                OnCaptureVideoFrame =
                    Marshal.GetFunctionPointerForDelegate(_irisRtcCVideoFrameObserver.OnCaptureVideoFrame),
                OnPreEncodeVideoFrame =
                    Marshal.GetFunctionPointerForDelegate(_irisRtcCVideoFrameObserver.OnPreEncodeVideoFrame),
                OnRenderVideoFrame =
                    Marshal.GetFunctionPointerForDelegate(_irisRtcCVideoFrameObserver.OnRenderVideoFrame),
                GetObservedFramePosition =
                    Marshal.GetFunctionPointerForDelegate(_irisRtcCVideoFrameObserver.GetObservedFramePosition),
                IsMultipleChannelFrameWanted =
                    Marshal.GetFunctionPointerForDelegate(_irisRtcCVideoFrameObserver.IsMultipleChannelFrameWanted),
                OnRenderVideoFrameEx =
                    Marshal.GetFunctionPointerForDelegate(
                        _irisRtcCVideoFrameObserver.OnRenderVideoFrameEx)
            };

            _irisRtcCVideoFrameObserverNative =
                Marshal.AllocHGlobal(Marshal.SizeOf(irisRtcCVideoFrameObserverNativeLocal));

            _irisRtcVideoFrameObserverHandleNative = AgoraRtcNative.RegisterVideoFrameObserver(
                AgoraRtcNative.GetIrisRtcRawData(_irisRtcEngine), _irisRtcCVideoFrameObserverNative, 0,
                this == engineInstance[0] ? identifier[0] : identifier[1]);
        }

        private void UnSetIrisVideoFrameObserver()
        {
            AgoraRtcNative.UnRegisterVideoFrameObserver(AgoraRtcNative.GetIrisRtcRawData(_irisRtcEngine),
                _irisRtcVideoFrameObserverHandleNative, this == engineInstance[0] ? identifier[0] : identifier[1]);
            _irisRtcVideoFrameObserverHandleNative = IntPtr.Zero;
            if (_rtcVideoFrameObserverNative != null) _rtcVideoFrameObserverNative.Dispose();
            _rtcVideoFrameObserverNative = null;
            _irisRtcCVideoFrameObserver = new IrisRtcCVideoFrameObserver();
            Marshal.FreeHGlobal(_irisRtcCVideoFrameObserverNative);
        }

        public override void Dispose(bool sync = false)
        {
            Dispose(true, sync);
            GC.SuppressFinalize(this);
        }

        [Obsolete(ObsoleteMethodWarning.DestroyWarning, true)]
        public static void Destroy(AgoraRtcEngine rtcEngine = null)
        {
            if (rtcEngine == null)
            {
                if (engineInstance[0] != null) engineInstance[0].Dispose();
            }
            else
            {
                rtcEngine.Dispose();
            }
        }

        [Obsolete(
            "This method is deprecated. IAudioEffectManagerWarning is deprecated. All the methods can be called directly in AgoraRtcEngine.",
            false)]
        public override IAudioEffectManager GetAudioEffectManager()
        {
            return _deprecatedAudioEffectManagerInstance;
        }

        [Obsolete("This method is deprecated. Please call GetAgoraRtcAudioRecordingDeviceManager instead.", false)]
        public override IAudioRecordingDeviceManager GetAudioRecordingDeviceManager()
        {
            return _deprecatedAudioRecordingDeviceManagerInstance;
        }

        public override IAgoraRtcAudioRecordingDeviceManager GetAgoraRtcAudioRecordingDeviceManager()
        {
            return _audioRecordingDeviceManagerInstance;
        }

        [Obsolete("This method is deprecated. Please call GetAgoraRtcAudioPlaybackDeviceManager instead.", false)]
        public override IAudioPlaybackDeviceManager GetAudioPlaybackDeviceManager()
        {
            return _deprecatedAudioPlaybackDeviceManagerInstance;
        }

        public override IAgoraRtcAudioPlaybackDeviceManager GetAgoraRtcAudioPlaybackDeviceManager()
        {
            return _audioPlaybackDeviceManagerInstance;
        }

        [Obsolete("This method is deprecated. Please call GetAgoraRtcVideoDeviceManager instead.", false)]
        public override IVideoDeviceManager GetVideoDeviceManager()
        {
            return _deprecatedVideoDeviceManagerInstance;
        }

        public override IAgoraRtcVideoDeviceManager GetAgoraRtcVideoDeviceManager()
        {
            return _videoDeviceManagerInstance;
        }

        public override IAudioRawDataManager GetAudioRawDataManager()
        {
            throw new NotImplementedException();
        }

        public override IVideoRawDataManager GetVideoRawDataManager()
        {
            throw new NotImplementedException();
        }

        public override IAgoraRtcChannel CreateChannel(string channelId)
        {
            if (_channelInstance.ContainsKey(channelId))
            {
                return _channelInstance[channelId];
            }

            var ret = new AgoraRtcChannel(this, channelId);
            _channelInstance.Add(channelId, ret);
            return ret;
        }

        internal IVideoStreamManager GetVideoStreamManager()
        {
            return new VideoStreamManager(this);
        }

        internal void ReleaseChannel(string channelId)
        {
            _channelInstance.Remove(channelId);
        }

        public override int SetChannelProfile(CHANNEL_PROFILE_TYPE profile)
        {
            var param = new
            {
                profile
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetChannelProfile,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetClientRole(CLIENT_ROLE_TYPE role)
        {
            var param = new
            {
                role
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetClientRole,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetClientRole(CLIENT_ROLE_TYPE role, ClientRoleOptions options)
        {
            var param = new
            {
                role,
                options
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetClientRole,
                JsonMapper.ToJson(param), out _result);
        }

        public override int JoinChannel(string token, string channelId, string info = "", uint uid = 0)
        {
            var param = new
            {
                token,
                channelId,
                info,
                uid
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineJoinChannel,
                JsonMapper.ToJson(param), out _result);
        }

        public override int JoinChannel(string token, string channelId, string info, uint uid,
            ChannelMediaOptions options)
        {
            var param = new
            {
                token,
                channelId,
                info,
                uid,
                options
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineJoinChannel,
                JsonMapper.ToJson(param), out _result);
        }

        [Obsolete(ObsoleteMethodWarning.GeneralWarning, false)]
        public override int JoinChannel(string channelId, string info = "", uint uid = 0)
        {
            return JoinChannel(null, channelId, info, uid);
        }

        [Obsolete(ObsoleteMethodWarning.JoinChannelByKeyWarning, false)]
        public override int JoinChannelByKey(string token, string channelId, string info = "", uint uid = 0)
        {
            return JoinChannel(token, channelId, info, uid);
        }

        public override int SwitchChannel(string token, string channelId)
        {
            var param = new
            {
                token,
                channelId
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSwitchChannel,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SwitchChannel(string token, string channelId, ChannelMediaOptions options)
        {
            var param = new
            {
                token,
                channelId,
                options
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSwitchChannel,
                JsonMapper.ToJson(param), out _result);
        }

        public override int LeaveChannel()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineLeaveChannel,
                JsonMapper.ToJson(param), out _result);
        }

        public override int RenewToken(string token)
        {
            var param = new
            {
                token
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineRenewToken,
                JsonMapper.ToJson(param), out _result);
        }

        public override int RegisterLocalUserAccount(string appId, string userAccount)
        {
            var param = new
            {
                appId,
                userAccount
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineRegisterLocalUserAccount,
                JsonMapper.ToJson(param), out _result);
        }

        public override int JoinChannelWithUserAccount(string token, string channelId, string userAccount)
        {
            var param = new
            {
                token,
                channelId,
                userAccount
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineJoinChannelWithUserAccount,
                JsonMapper.ToJson(param), out _result);
        }

        public override int JoinChannelWithUserAccount(string token, string channelId, string userAccount,
            ChannelMediaOptions options)
        {
            var param = new
            {
                token,
                channelId,
                userAccount,
                options
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineJoinChannelWithUserAccount,
                JsonMapper.ToJson(param), out _result);
        }

        public override int GetUserInfoByUserAccount(string userAccount, out UserInfo userInfo)
        {
            var param = new
            {
                userAccount
            };
            var ret = AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineGetUserInfoByUserAccount,
                JsonMapper.ToJson(param), out _result);
            userInfo = _result.Result.Length == 0 ? new UserInfo() : AgoraJson.JsonToStruct<UserInfo>(_result.Result);
            return ret;
        }

        [Obsolete(ObsoleteMethodWarning.GeneralWarning, false)]
        public override UserInfo GetUserInfoByUserAccount(string userAccount)
        {
            var param = new
            {
                userAccount
            };
            AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineGetUserInfoByUserAccount,
                JsonMapper.ToJson(param), out _result);
            return _result.Result.Length == 0 ? new UserInfo() : AgoraJson.JsonToStruct<UserInfo>(_result.Result);
        }

        public override int GetUserInfoByUid(uint uid, out UserInfo userInfo)
        {
            var param = new
            {
                uid
            };
            var ret = AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineGetUserInfoByUid,
                JsonMapper.ToJson(param), out _result);
            userInfo = _result.Result.Length == 0 ? new UserInfo() : AgoraJson.JsonToStruct<UserInfo>(_result.Result);
            return ret;
        }

        [Obsolete(ObsoleteMethodWarning.GeneralWarning, false)]
        public override UserInfo GetUserInfoByUid(uint uid)
        {
            var param = new
            {
                uid
            };
            AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineGetUserInfoByUid,
                JsonMapper.ToJson(param), out _result);
            return _result.Result.Length == 0 ? new UserInfo() : AgoraJson.JsonToStruct<UserInfo>(_result.Result);
        }

        [Obsolete(ObsoleteMethodWarning.GeneralWarning, false)]
        public override int StartEchoTest()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineStartEchoTest,
                JsonMapper.ToJson(param), out _result);
        }

        public override int StartEchoTest(int intervalInSeconds)
        {
            var param = new
            {
                intervalInSeconds
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineStartEchoTest,
                JsonMapper.ToJson(param), out _result);
        }

        public override int StopEchoTest()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineStopEchoTest,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetCloudProxy(CLOUD_PROXY_TYPE proxyType)
        {
            var param = new
            {
                proxyType
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetCloudProxy,
                JsonMapper.ToJson(param), out _result);
        }

        public override int EnableVideo()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineEnableVideo,
                JsonMapper.ToJson(param), out _result);
        }

        public override int DisableVideo()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineDisableVideo,
                JsonMapper.ToJson(param), out _result);
        }

        public override int EnableVideoObserver()
        {
            throw new NotImplementedException();
        }

        public override int DisableVideoObserver()
        {
            throw new NotImplementedException();
        }

        [Obsolete("This method is deprecated. Please call SetVideoEncoderConfiguration instead.", false)]
        public override int SetVideoProfile(VIDEO_PROFILE_TYPE profile, bool swapWidthAndHeight = false)
        {
            var param = new
            {
                profile,
                swapWidthAndHeight
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetVideoProfile,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetVideoEncoderConfiguration(VideoEncoderConfiguration config)
        {
            var param = new
            {
                config
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetVideoEncoderConfiguration, JsonMapper.ToJson(param),
                out _result);
        }

        public override int SetCameraCapturerConfiguration(CameraCapturerConfiguration config)
        {
            var param = new
            {
                config
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetCameraCapturerConfiguration, JsonMapper.ToJson(param),
                out _result);
        }

        public override int SetupLocalVideo(VideoCanvas canvas)
        {
            var param = new
            {
                canvas = new
                {
                    view = (ulong) canvas.view,
                    canvas.renderMode,
                    canvas.channelId,
                    canvas.uid,
                    canvas.mirrorMode
                }
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetupLocalVideo,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetupRemoteVideo(VideoCanvas canvas)
        {
            var param = new
            {
                canvas = new
                {
                    view = (ulong) canvas.view,
                    canvas.renderMode,
                    canvas.channelId,
                    canvas.uid,
                    canvas.mirrorMode
                }
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetupRemoteVideo,
                JsonMapper.ToJson(param), out _result);
        }

        public override int StartPreview()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineStartPreview,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetRemoteUserPriority(uint uid, PRIORITY_TYPE userPriority)
        {
            var param = new
            {
                uid,
                userPriority
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetRemoteUserPriority,
                JsonMapper.ToJson(param), out _result);
        }

        public override int StopPreview()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineStopPreview,
                JsonMapper.ToJson(param), out _result);
        }

        public override int EnableAudio()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineEnableAudio,
                JsonMapper.ToJson(param), out _result);
        }

        public override int EnableLocalAudio(bool enabled)
        {
            var param = new
            {
                enabled
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineEnableLocalAudio,
                JsonMapper.ToJson(param), out _result);
        }

        public override int DisableAudio()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineDisableAudio,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetAudioProfile(AUDIO_PROFILE_TYPE profile, AUDIO_SCENARIO_TYPE scenario)
        {
            var param = new
            {
                profile,
                scenario
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetAudioProfile,
                JsonMapper.ToJson(param), out _result);
        }

        public override int MuteLocalAudioStream(bool mute)
        {
            var param = new
            {
                mute
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineMuteLocalAudioStream,
                JsonMapper.ToJson(param), out _result);
        }

        public override int MuteAllRemoteAudioStreams(bool mute)
        {
            var param = new
            {
                mute
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineMuteAllRemoteAudioStreams,
                JsonMapper.ToJson(param), out _result);
        }

        [Obsolete(ObsoleteMethodWarning.GeneralWarning, false)]
        public override int SetDefaultMuteAllRemoteAudioStreams(bool mute)
        {
            var param = new
            {
                mute
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetDefaultMuteAllRemoteAudioStreams,
                JsonMapper.ToJson(param), out _result);
        }

        public override int AdjustUserPlaybackSignalVolume(uint uid, int volume)
        {
            var param = new
            {
                uid,
                volume
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineAdjustUserPlaybackSignalVolume, JsonMapper.ToJson(param),
                out _result);
        }

        public override int MuteRemoteAudioStream(uint userId, bool mute)
        {
            var param = new
            {
                userId,
                mute
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineMuteRemoteAudioStream,
                JsonMapper.ToJson(param), out _result);
        }

        public override int MuteLocalVideoStream(bool mute)
        {
            var param = new
            {
                mute
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineMuteLocalVideoStream,
                JsonMapper.ToJson(param), out _result);
        }

        public override int EnableLocalVideo(bool enabled)
        {
            var param = new
            {
                enabled
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineEnableLocalVideo,
                JsonMapper.ToJson(param), out _result);
        }

        public override int MuteAllRemoteVideoStreams(bool mute)
        {
            var param = new
            {
                mute
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineMuteAllRemoteVideoStreams,
                JsonMapper.ToJson(param), out _result);
        }

        [Obsolete(ObsoleteMethodWarning.GeneralWarning, false)]
        public override int SetDefaultMuteAllRemoteVideoStreams(bool mute)
        {
            var param = new
            {
                mute
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetDefaultMuteAllRemoteVideoStreams,
                JsonMapper.ToJson(param), out _result);
        }

        public override int MuteRemoteVideoStream(uint userId, bool mute)
        {
            var param = new
            {
                userId,
                mute
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineMuteRemoteVideoStream,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetRemoteVideoStreamType(uint userId, REMOTE_VIDEO_STREAM_TYPE streamType)
        {
            var param = new
            {
                userId,
                streamType
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetRemoteVideoStreamType,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetRemoteDefaultVideoStreamType(REMOTE_VIDEO_STREAM_TYPE streamType)
        {
            var param = new
            {
                streamType
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetRemoteDefaultVideoStreamType, JsonMapper.ToJson(param),
                out _result);
        }

        public override int EnableAudioVolumeIndication(int interval, int smooth, bool reportVad = false)
        {
            var param = new
            {
                interval,
                smooth,
                reportVad
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineEnableAudioVolumeIndication,
                JsonMapper.ToJson(param), out _result);
        }

        [Obsolete(ObsoleteMethodWarning.GeneralWarning, false)]
        public override int StartAudioRecording(string filePath, AUDIO_RECORDING_QUALITY_TYPE quality)
        {
            var param = new
            {
                filePath,
                quality
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineStartAudioRecording,
                JsonMapper.ToJson(param), out _result);
        }

        public override int StartAudioRecording(string filePath, int sampleRate, AUDIO_RECORDING_QUALITY_TYPE quality)
        {
            var param = new
            {
                filePath,
                sampleRate,
                quality
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineStartAudioRecording,
                JsonMapper.ToJson(param), out _result);
        }

        public override int StopAudioRecording()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineStopAudioRecording,
                JsonMapper.ToJson(param), out _result);
        }

        public override int StartAudioMixing(string filePath, bool loopback, bool replace, int cycle)
        {
            var param = new
            {
                filePath,
                loopback,
                replace,
                cycle
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineStartAudioMixing,
                JsonMapper.ToJson(param), out _result);
        }

        public override int StopAudioMixing()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineStopAudioMixing,
                JsonMapper.ToJson(param), out _result);
        }

        public override int PauseAudioMixing()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEnginePauseAudioMixing,
                JsonMapper.ToJson(param), out _result);
        }

        public override int ResumeAudioMixing()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineResumeAudioMixing,
                JsonMapper.ToJson(param), out _result);
        }

        [Obsolete(ObsoleteMethodWarning.GeneralWarning, false)]
        public override int SetHighQualityAudioParameters(bool fullband, bool stereo, bool fullBitrate)
        {
            var param = new
            {
                fullband,
                stereo,
                fullBitrate
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetHighQualityAudioParameters, JsonMapper.ToJson(param),
                out _result);
        }

        public override int AdjustAudioMixingVolume(int volume)
        {
            var param = new
            {
                volume
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineAdjustAudioMixingVolume,
                JsonMapper.ToJson(param), out _result);
        }

        public override int AdjustAudioMixingPlayoutVolume(int volume)
        {
            var param = new
            {
                volume
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineAdjustAudioMixingPlayoutVolume, JsonMapper.ToJson(param),
                out _result);
        }

        public override int GetAudioMixingPlayoutVolume()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineGetAudioMixingPlayoutVolume,
                JsonMapper.ToJson(param), out _result);
        }

        public override int AdjustAudioMixingPublishVolume(int volume)
        {
            var param = new
            {
                volume
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineAdjustAudioMixingPublishVolume, JsonMapper.ToJson(param),
                out _result);
        }

        public override int GetAudioMixingPublishVolume()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineGetAudioMixingPublishVolume,
                JsonMapper.ToJson(param), out _result);
        }

        public override int GetAudioMixingDuration()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineGetAudioMixingDuration,
                JsonMapper.ToJson(param), out _result);
        }

        public override int GetAudioMixingCurrentPosition()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineGetAudioMixingCurrentPosition, JsonMapper.ToJson(param),
                out _result);
        }

        public override int SetAudioMixingPosition(int pos)
        {
            var param = new
            {
                pos
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetAudioMixingPosition,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetAudioMixingPitch(int pitch)
        {
            var param = new
            {
                pitch
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetAudioMixingPitch,
                JsonMapper.ToJson(param), out _result);
        }

        public override int GetEffectsVolume()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineGetEffectsVolume,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetEffectsVolume(int volume)
        {
            var param = new
            {
                volume
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetEffectsVolume,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetVolumeOfEffect(int soundId, int volume)
        {
            var param = new
            {
                soundId,
                volume
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetVolumeOfEffect,
                JsonMapper.ToJson(param), out _result);
        }

        public override int EnableFaceDetection(bool enable)
        {
            var param = new
            {
                enable
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineEnableFaceDetection,
                JsonMapper.ToJson(param), out _result);
        }

        public override int PlayEffect(int soundId, string filePath, int loopCount, double pitch = 1.0,
            double pan = 0.0, int gain = 100, bool publish = false)
        {
            var param = new
            {
                soundId,
                filePath,
                loopCount,
                pitch,
                pan,
                gain,
                publish
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEnginePlayEffect,
                JsonMapper.ToJson(param), out _result);
        }

        public override int StopEffect(int soundId)
        {
            var param = new
            {
                soundId
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineStopEffect,
                JsonMapper.ToJson(param), out _result);
        }

        public override int StopAllEffects()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineStopAllEffects,
                JsonMapper.ToJson(param), out _result);
        }

        public override int PreloadEffect(int soundId, string filePath)
        {
            var param = new
            {
                soundId,
                filePath
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEnginePreloadEffect,
                JsonMapper.ToJson(param), out _result);
        }

        public override int UnloadEffect(int soundId)
        {
            var param = new
            {
                soundId
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineUnloadEffect,
                JsonMapper.ToJson(param), out _result);
        }

        public override int PauseEffect(int soundId)
        {
            var param = new
            {
                soundId
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEnginePauseEffect,
                JsonMapper.ToJson(param), out _result);
        }

        public override int PauseAllEffects()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEnginePauseAllEffects,
                JsonMapper.ToJson(param), out _result);
        }

        public override int ResumeEffect(int soundId)
        {
            var param = new
            {
                soundId
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineResumeEffect,
                JsonMapper.ToJson(param), out _result);
        }

        public override int ResumeAllEffects()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineResumeAllEffects,
                JsonMapper.ToJson(param), out _result);
        }

        public override int EnableDeepLearningDenoise(bool enable)
        {
            var param = new
            {
                enable
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineEnableDeepLearningDenoise,
                JsonMapper.ToJson(param), out _result);
        }

        public override int EnableSoundPositionIndication(bool enabled)
        {
            var param = new
            {
                enabled
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineEnableSoundPositionIndication, JsonMapper.ToJson(param),
                out _result);
        }

        public override int SetRemoteVoicePosition(uint uid, double pan, double gain)
        {
            var param = new
            {
                uid,
                pan,
                gain
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetRemoteVoicePosition,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetLocalVoicePitch(double pitch)
        {
            var param = new
            {
                pitch
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetLocalVoicePitch,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetLocalVoiceEqualization(AUDIO_EQUALIZATION_BAND_FREQUENCY bandFrequency, int bandGain)
        {
            var param = new
            {
                bandFrequency,
                bandGain
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetLocalVoiceEqualization,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetLocalVoiceReverb(AUDIO_REVERB_TYPE reverbKey, int value)
        {
            var param = new
            {
                reverbKey,
                value
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetLocalVoiceReverb,
                JsonMapper.ToJson(param), out _result);
        }

        [Obsolete(ObsoleteMethodWarning.GeneralWarning, false)]
        public override int SetLocalVoiceChanger(VOICE_CHANGER_PRESET voiceChanger)
        {
            var param = new
            {
                voiceChanger
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetLocalVoiceChanger,
                JsonMapper.ToJson(param), out _result);
        }

        [Obsolete(ObsoleteMethodWarning.SetLocalVoiceReverbPresetWarning, false)]
        public override int SetLocalVoiceReverbPreset(AUDIO_REVERB_PRESET reverbPreset)
        {
            var param = new
            {
                reverbPreset
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetLocalVoiceReverbPreset,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetVoiceBeautifierPreset(VOICE_BEAUTIFIER_PRESET preset)
        {
            var param = new
            {
                preset
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetVoiceBeautifierPreset,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetAudioEffectPreset(AUDIO_EFFECT_PRESET preset)
        {
            var param = new
            {
                preset
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetAudioEffectPreset,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetVoiceConversionPreset(VOICE_CONVERSION_PRESET preset)
        {
            var param = new
            {
                preset
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetVoiceConversionPreset,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetAudioEffectParameters(AUDIO_EFFECT_PRESET preset, int param1, int param2)
        {
            var param = new
            {
                preset,
                param1,
                param2
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetAudioEffectParameters,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetVoiceBeautifierParameters(VOICE_BEAUTIFIER_PRESET preset, int param1, int param2)
        {
            var param = new
            {
                preset,
                param1,
                param2
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetVoiceBeautifierParameters, JsonMapper.ToJson(param),
                out _result);
        }

        [Obsolete(ObsoleteMethodWarning.SetLogFileWarning, false)]
        public override int SetLogFile(string filePath)
        {
            var param = new
            {
                filePath
            };

            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetLogFile,
                JsonMapper.ToJson(param), out _result);
        }

        [Obsolete(ObsoleteMethodWarning.SetLogFilterWarning, false)]
        public override int SetLogFilter(uint filter)
        {
            var param = new
            {
                filter
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetLogFilter,
                JsonMapper.ToJson(param), out _result);
        }

        [Obsolete(ObsoleteMethodWarning.SetLogFileSizeWarning, false)]
        public override int SetLogFileSize(uint fileSizeInKBytes)
        {
            var param = new
            {
                fileSizeInKBytes
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetLogFileSize,
                JsonMapper.ToJson(param), out _result);
        }

        public override string UploadLogFile()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineUploadLogFile,
                JsonMapper.ToJson(param), out _result) != 0
                ? null
                : _result.Result;
        }

        [Obsolete(ObsoleteMethodWarning.GeneralWarning, false)]
        public override int SetLocalRenderMode(RENDER_MODE_TYPE renderMode)
        {
            var param = new
            {
                renderMode
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetLocalRenderMode,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetLocalRenderMode(RENDER_MODE_TYPE renderMode, VIDEO_MIRROR_MODE_TYPE mirrorMode)
        {
            var param = new
            {
                renderMode,
                mirrorMode
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetLocalRenderMode,
                JsonMapper.ToJson(param), out _result);
        }

        [Obsolete(ObsoleteMethodWarning.GeneralWarning, false)]
        public override int SetRemoteRenderMode(uint userId, RENDER_MODE_TYPE renderMode)
        {
            var param = new
            {
                userId,
                renderMode
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetRemoteRenderMode,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetRemoteRenderMode(uint userId, RENDER_MODE_TYPE renderMode,
            VIDEO_MIRROR_MODE_TYPE mirrorMode)
        {
            var param = new
            {
                userId,
                renderMode,
                mirrorMode
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetRemoteRenderMode,
                JsonMapper.ToJson(param), out _result);
        }

        [Obsolete(ObsoleteMethodWarning.SetLocalVideoMirrorModeWarning, false)]
        public override int SetLocalVideoMirrorMode(VIDEO_MIRROR_MODE_TYPE mirrorMode)
        {
            var param = new
            {
                mirrorMode
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetLocalVideoMirrorMode,
                JsonMapper.ToJson(param), out _result);
        }

        public override int EnableDualStreamMode(bool enabled)
        {
            var param = new
            {
                enabled
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineEnableDualStreamMode,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetExternalAudioSource(bool enabled, int sampleRate, int channels)
        {
            var param = new
            {
                enabled,
                sampleRate,
                channels
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetExternalAudioSource,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetExternalAudioSink(bool enabled, int sampleRate, int channels)
        {
            var param = new
            {
                enabled,
                sampleRate,
                channels
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetExternalAudioSink,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetRecordingAudioFrameParameters(int sampleRate, int channel,
            RAW_AUDIO_FRAME_OP_MODE_TYPE mode,
            int samplesPerCall)
        {
            var param = new
            {
                sampleRate,
                channel,
                mode,
                samplesPerCall
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetRecordingAudioFrameParameters, JsonMapper.ToJson(param),
                out _result);
        }

        public override int SetPlaybackAudioFrameParameters(int sampleRate, int channel,
            RAW_AUDIO_FRAME_OP_MODE_TYPE mode, int samplesPerCall)
        {
            var param = new
            {
                sampleRate,
                channel,
                mode,
                samplesPerCall
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetPlaybackAudioFrameParameters, JsonMapper.ToJson(param),
                out _result);
        }

        public override int SetMixedAudioFrameParameters(int sampleRate, int samplesPerCall)
        {
            var param = new
            {
                sampleRate,
                samplesPerCall
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetMixedAudioFrameParameters, JsonMapper.ToJson(param),
                out _result);
        }

        public override int AdjustRecordingSignalVolume(int volume)
        {
            var param = new
            {
                volume
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineAdjustRecordingSignalVolume,
                JsonMapper.ToJson(param), out _result);
        }

        public override int AdjustPlaybackSignalVolume(int volume)
        {
            var param = new
            {
                volume
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineAdjustPlaybackSignalVolume,
                JsonMapper.ToJson(param), out _result);
        }

        [Obsolete(ObsoleteMethodWarning.GeneralWarning, false)]
        public override int EnableWebSdkInteroperability(bool enabled)
        {
            var param = new
            {
                enabled
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineEnableWebSdkInteroperability, JsonMapper.ToJson(param),
                out _result);
        }

        [Obsolete(ObsoleteMethodWarning.GeneralWarning, false)]
        public override int SetVideoQualityParameters(bool preferFrameRateOverImageQuality)
        {
            var param = new
            {
                preferFrameRateOverImageQuality
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetVideoQualityParameters,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetLocalPublishFallbackOption(STREAM_FALLBACK_OPTIONS option)
        {
            var param = new
            {
                option
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetLocalPublishFallbackOption, JsonMapper.ToJson(param),
                out _result);
        }

        public override int SetRemoteSubscribeFallbackOption(STREAM_FALLBACK_OPTIONS option)
        {
            var param = new
            {
                option
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetRemoteSubscribeFallbackOption, JsonMapper.ToJson(param),
                out _result);
        }

        public override int SwitchCamera()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSwitchCamera,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetDefaultAudioRouteToSpeakerphone(bool defaultToSpeaker)
        {
            var param = new
            {
                defaultToSpeaker
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetDefaultAudioRouteToSpeakerPhone,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetEnableSpeakerphone(bool speakerOn)
        {
            var param = new
            {
                speakerOn
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetEnableSpeakerPhone,
                JsonMapper.ToJson(param), out _result);
        }

        public override int EnableInEarMonitoring(bool enabled)
        {
            var param = new
            {
                enabled
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineEnableInEarMonitoring,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetInEarMonitoringVolume(int volume)
        {
            var param = new
            {
                volume
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetInEarMonitoringVolume,
                JsonMapper.ToJson(param), out _result);
        }

        public override bool IsSpeakerphoneEnabled()
        {
            var param = new { };
            var ret = AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineIsSpeakerPhoneEnabled,
                JsonMapper.ToJson(param), out _result);
            if (ret < 0) return false;
            return ret == 1;
        }

        public override int SetAudioSessionOperationRestriction(AUDIO_SESSION_OPERATION_RESTRICTION restriction)
        {
            var param = new
            {
                restriction
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetAudioSessionOperationRestriction,
                JsonMapper.ToJson(param), out _result);
        }

        public override int EnableLoopbackRecording(bool enabled, string deviceName)
        {
            var param = new
            {
                enabled,
                deviceName
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineEnableLoopBackRecording,
                JsonMapper.ToJson(param), out _result);
        }

        public override int StartScreenCaptureByDisplayId(uint displayId, Rectangle regionRect,
            ScreenCaptureParameters captureParams)
        {
            // var ewl = new ulong[captureParams.excludeWindowCount];
            // for (var i = 0; i < captureParams.excludeWindowCount; i++)
            // {
            //     ewl[i] = (ulong) captureParams.excludeWindowList[i];
            // }
            //
            // var param = new
            // {
            //     displayId,
            //     regionRect,
            //     captureParams = new
            //     {
            //         captureParams.dimensions,
            //         captureParams.frameRate,
            //         captureParams.bitrate,
            //         captureParams.windowFocus,
            //         excludeWindowList = ewl,
            //         captureParams.excludeWindowCount
            //     }
            // };
            var param = new
            {
                displayId,
                regionRect,
                captureParams
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineStartScreenCaptureByDisplayId, JsonMapper.ToJson(param),
                out _result);
        }

        public override int StartScreenCaptureByScreenRect(Rectangle screenRect, Rectangle regionRect,
            ScreenCaptureParameters captureParams)
        {
            // var ewl = new ulong[captureParams.excludeWindowCount];
            // for (var i = 0; i < captureParams.excludeWindowCount; i++)
            // {
            //     ewl[i] = (ulong) captureParams.excludeWindowList[i];
            // }
            //
            // var param = new
            // {
            //     screenRect,
            //     regionRect,
            //     captureParams = new
            //     {
            //         captureParams.dimensions,
            //         captureParams.frameRate,
            //         captureParams.bitrate,
            //         captureParams.windowFocus,
            //         excludeWindowList = ewl,
            //         captureParams.excludeWindowCount
            //     }
            // };
            var param = new
            {
                screenRect,
                regionRect,
                captureParams
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineStartScreenCaptureByScreenRect, JsonMapper.ToJson(param),
                out _result);
        }

        public override int StartScreenCaptureByWindowId(view_t windowId, Rectangle regionRect,
            ScreenCaptureParameters captureParams)
        {
            // var ewl = new ulong[captureParams.excludeWindowCount];
            // for (var i = 0; i < captureParams.excludeWindowCount; i++)
            // {
            //     ewl[i] = captureParams.excludeWindowList[i];
            // }

            // var param = new
            // {
            //     windowId,
            //     regionRect,
            //     captureParams = new
            //     {
            //         captureParams.dimensions,
            //         captureParams.frameRate,
            //         captureParams.bitrate,
            //         captureParams.windowFocus,
            //         excludeWindowList = ewl,
            //         captureParams.excludeWindowCount
            //     }
            // };
            var param = new
            {
                windowId,
                regionRect,
                captureParams
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineStartScreenCaptureByWindowId, JsonMapper.ToJson(param),
                out _result);
        }

        public override int SetScreenCaptureContentHint(VideoContentHint contentHint)
        {
            var param = new
            {
                contentHint
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetScreenCaptureContentHint,
                JsonMapper.ToJson(param), out _result);
        }

        public override int UpdateScreenCaptureParameters(ScreenCaptureParameters captureParams)
        {
            // var ewl = new ulong[captureParams.excludeWindowCount];
            // for (var i = 0; i < captureParams.excludeWindowCount; i++)
            // {
            //     ewl[i] = (ulong) captureParams.excludeWindowList[i];
            // }
            //
            // var param = new
            // {
            //     captureParams = new
            //     {
            //         captureParams.dimensions,
            //         captureParams.frameRate,
            //         captureParams.bitrate,
            //         captureParams.windowFocus,
            //         excludeWindowList = ewl,
            //         captureParams.excludeWindowCount
            //     }
            // };
            var param = new
            {
                captureParams
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineUpdateScreenCaptureParameters, JsonMapper.ToJson(param),
                out _result);
        }

        public override int UpdateScreenCaptureRegion(Rectangle regionRect)
        {
            var param = new
            {
                regionRect
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineUpdateScreenCaptureRegion,
                JsonMapper.ToJson(param), out _result);
        }

        public override int StopScreenCapture()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineStopScreenCapture,
                JsonMapper.ToJson(param), out _result);
        }

        public override string GetCallId()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineGetCallId,
                JsonMapper.ToJson(param), out _result) != 0
                ? null
                : _result.Result;
        }

        public override int Rate(string callId, int rating, string description = "")
        {
            var param = new
            {
                callId,
                rating,
                description
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineRate,
                JsonMapper.ToJson(param), out _result);
        }

        public override int Complain(string callId, string description = "")
        {
            var param = new
            {
                callId,
                description
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineComplain,
                JsonMapper.ToJson(param), out _result);
        }

        public override string GetVersion()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineGetVersion,
                JsonMapper.ToJson(param), out _result) != 0
                ? null
                : _result.Result;
        }

        [Obsolete(
            "GetSdkVersion is deprecated, please call GetVersion instead after IAgoraRtcEngine instance has been initialized.",
            true)]
        public static string GetSdkVersion()
        {
            return "";
        }

        public override int EnableLastmileTest()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineEnableLastMileTest,
                JsonMapper.ToJson(param), out _result);
        }

        public override int DisableLastmileTest()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineDisableLastMileTest,
                JsonMapper.ToJson(param), out _result);
        }

        public override int StartLastmileProbeTest(LastmileProbeConfig config)
        {
            var param = new
            {
                config
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineStartLastMileProbeTest,
                JsonMapper.ToJson(param), out _result);
        }

        public override int StopLastmileProbeTest()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineStopLastMileProbeTest,
                JsonMapper.ToJson(param), out _result);
        }

        public override string GetErrorDescription(int code)
        {
            var param = new
            {
                code
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineGetErrorDescription,
                JsonMapper.ToJson(param), out _result) != 0
                ? null
                : _result.Result;
        }

        [Obsolete(ObsoleteMethodWarning.SetEncryptionSecretWarning, false)]
        public override int SetEncryptionSecret(string secret)
        {
            var param = new
            {
                secret
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetEncryptionSecret,
                JsonMapper.ToJson(param), out _result);
        }

        [Obsolete(ObsoleteMethodWarning.SetEncryptionModeWarning, false)]
        public override int SetEncryptionMode(string encryptionMode)
        {
            var param = new
            {
                encryptionMode
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetEncryptionMode,
                JsonMapper.ToJson(param), out _result);
        }

        public override int EnableEncryption(bool enabled, EncryptionConfig config)
        {
            var param = new
            {
                enabled,
                config
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineEnableEncryption,
                JsonMapper.ToJson(param), out _result);
        }

        public override int RegisterPacketObserver(IPacketObserver observer)
        {
            var param = new
            {
                observer
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineRegisterPacketObserver,
                JsonMapper.ToJson(param), out _result);
        }

        [Obsolete(ObsoleteMethodWarning.GeneralWarning, false)]
        public override int CreateDataStream(bool reliable, bool ordered)
        {
            var param = new
            {
                reliable,
                ordered
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineCreateDataStream,
                JsonMapper.ToJson(param), out _result);
        }

        public override int CreateDataStream(DataStreamConfig config)
        {
            var param = new
            {
                config
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineCreateDataStream,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SendStreamMessage(int streamId, byte[] data)
        {
            var param = new
            {
                streamId,
                length = data.Length
            };
            return AgoraRtcNative.CallIrisRtcEngineApiWithBuffer(_irisRtcEngine,
                ApiTypeEngine.kEngineSendStreamMessage,
                JsonMapper.ToJson(param), data, out _result);
        }

        public override int AddPublishStreamUrl(string url, bool transcodingEnabled)
        {
            var param = new
            {
                url,
                transcodingEnabled
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineAddPublishStreamUrl,
                JsonMapper.ToJson(param), out _result);
        }

        public override int RemovePublishStreamUrl(string url)
        {
            var param = new
            {
                url
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineRemovePublishStreamUrl,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetLiveTranscoding(LiveTranscoding transcoding)
        {
            var param = new
            {
                transcoding
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetLiveTranscoding,
                JsonMapper.ToJson(param), out _result);
        }

        [Obsolete(ObsoleteMethodWarning.GeneralWarning, false)]
        public override int AddVideoWatermark(RtcImage watermark)
        {
            var param = new
            {
                watermark
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineAddVideoWaterMark,
                JsonMapper.ToJson(param), out _result);
        }

        public override int AddVideoWatermark(string watermarkUrl, WatermarkOptions options)
        {
            var param = new
            {
                watermarkUrl,
                options
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineAddVideoWaterMark,
                JsonMapper.ToJson(param), out _result);
        }

        public override int ClearVideoWatermarks()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineClearVideoWaterMarks,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetBeautyEffectOptions(bool enabled, BeautyOptions options)
        {
            var param = new
            {
                enabled,
                options
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetBeautyEffectOptions,
                JsonMapper.ToJson(param), out _result);
        }

        public override int AddInjectStreamUrl(string url, InjectStreamConfig config)
        {
            var param = new
            {
                url,
                config
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineAddInjectStreamUrl,
                JsonMapper.ToJson(param), out _result);
        }

        public override int StartChannelMediaRelay(ChannelMediaRelayConfiguration configuration)
        {
            var param = new
            {
                configuration
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineStartChannelMediaRelay,
                JsonMapper.ToJson(param), out _result);
        }

        public override int UpdateChannelMediaRelay(ChannelMediaRelayConfiguration configuration)
        {
            var param = new
            {
                configuration
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineUpdateChannelMediaRelay,
                JsonMapper.ToJson(param), out _result);
        }

        public override int StopChannelMediaRelay()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineStopChannelMediaRelay,
                JsonMapper.ToJson(param), out _result);
        }

        public override int RemoveInjectStreamUrl(string url)
        {
            var param = new
            {
                url
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineRemoveInjectStreamUrl,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SendCustomReportMessage(string id, string category, string events, string label, int value)
        {
            var param = new
            {
                id,
                category,
                events,
                label,
                value
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSendCustomReportMessage,
                JsonMapper.ToJson(param), out _result);
        }

        public override CONNECTION_STATE_TYPE GetConnectionState()
        {
            var param = new { };
            return (CONNECTION_STATE_TYPE) AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineGetConnectionState, JsonMapper.ToJson(param), out _result);
        }

        public override int RegisterMediaMetadataObserver(METADATA_TYPE type)
        {
            var param = new
            {
                type
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineRegisterMediaMetadataObserver, JsonMapper.ToJson(param),
                out _result);
        }

        public override int UnRegisterMediaMetadataObserver(METADATA_TYPE type)
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineUnRegisterMediaMetadataObserver, JsonMapper.ToJson(param),
                out _result);
        }

        public override int EnableRemoteSuperResolution(uint userId, bool enable)
        {
            var param = new
            {
                userId,
                enable
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineEnableRemoteSuperResolution,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetParameters(string parameters)
        {
            var param = new
            {
                parameters
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetParameters,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetMaxMetadataSize(int size)
        {
            var param = new
            {
                size
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetMaxMetadataSize,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SendMetadata(Metadata metadata)
        {
            var param = new
            {
                metadata = new
                {
                    metadata.uid,
                    metadata.size,
                    metadata.timeStampMs
                }
            };
            return AgoraRtcNative.CallIrisRtcEngineApiWithBuffer(_irisRtcEngine,
                ApiTypeEngine.kEngineSendMetadata,
                JsonMapper.ToJson(param), metadata.buffer, out _result);
        }

        public override int PushAudioFrame(MEDIA_SOURCE_TYPE type, AudioFrame frame, bool wrap)
        {
            var param = new
            {
                type,
                frame = new
                {
                    frame.type,
                    frame.samples,
                    frame.bytesPerSample,
                    frame.channels,
                    frame.samplesPerSec,
                    frame.renderTimeMs,
                    frame.avsync_type
                },
                wrap
            };
            return AgoraRtcNative.CallIrisRtcEngineApiWithBuffer(_irisRtcEngine,
                ApiTypeEngine.kMediaPushAudioFrame,
                JsonMapper.ToJson(param), frame.buffer, out _result);
        }

        public override int PushAudioFrame(AudioFrame frame)
        {
            var param = new
            {
                frame = new
                {
                    frame.type,
                    frame.samples,
                    frame.bytesPerSample,
                    frame.channels,
                    frame.samplesPerSec,
                    frame.renderTimeMs,
                    frame.avsync_type
                }
            };
            return AgoraRtcNative.CallIrisRtcEngineApiWithBuffer(_irisRtcEngine,
                ApiTypeEngine.kMediaPushAudioFrame,
                JsonMapper.ToJson(param), frame.buffer, out _result);
        }

        public override int PullAudioFrame(AudioFrame frame)
        {
            var param = new { };
            var ret = AgoraRtcNative.CallIrisRtcEngineApiWithBuffer(_irisRtcEngine,
                ApiTypeEngine.kMediaPullAudioFrame,
                JsonMapper.ToJson(param), frame.buffer, out _result);
            var f = _result.Result.Length == 0
                ? new AudioFrameWithoutBuffer()
                : AgoraJson.JsonToStruct<AudioFrameWithoutBuffer>(_result.Result);
            frame.avsync_type = f.avsync_type;
            frame.channels = f.channels;
            frame.samples = f.samples;
            frame.type = f.type;
            frame.bytesPerSample = f.bytesPerSample;
            frame.renderTimeMs = f.renderTimeMs;
            frame.samplesPerSec = f.samplesPerSec;
            return ret;
        }

        public override int SetExternalVideoSource(bool enable, bool useTexture = false)
        {
            var param = new
            {
                enable,
                useTexture
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kMediaSetExternalVideoSource, JsonMapper.ToJson(param),
                out _result);
        }

        public override int PushVideoFrame(ExternalVideoFrame frame)
        {
            var param = new
            {
                frame = new
                {
                    frame.type,
                    frame.format,
                    frame.stride,
                    frame.height,
                    frame.cropLeft,
                    frame.cropTop,
                    frame.cropRight,
                    frame.cropBottom,
                    frame.rotation,
                    frame.timestamp
                }
            };
            return AgoraRtcNative.CallIrisRtcEngineApiWithBuffer(_irisRtcEngine,
                ApiTypeEngine.kMediaPushVideoFrame,
                JsonMapper.ToJson(param), frame.buffer, out _result);
        }

        ~AgoraRtcEngine()
        {
            Dispose(false, false);
        }
    }
}