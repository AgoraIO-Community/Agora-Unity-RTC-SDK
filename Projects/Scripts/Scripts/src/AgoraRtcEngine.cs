//  AgoraRtcEngine.cs
//
//  Created by Yiqing Huang on June 2, 2021.
//  Modified by Yiqing Huang on June 6, 2021.
//
//  Copyright © 2021 Agora. All rights reserved.
//

using System;
using System.Runtime.InteropServices;
using System.Text;
using LitJson;

namespace agora_gaming_rtc
{
    using view_t = UInt64;
    using IrisRtcEnginePtr = IntPtr;
    using IrisEventHandlerHandleNative = IntPtr;

    internal sealed class NativeRtcEngineEventHandler
    {
        private IRtcEngineEventHandler _engineEventHandler;
        private AgoraCallbackObject _callbackObject;

        internal NativeRtcEngineEventHandler()
        {
            _callbackObject = new AgoraCallbackObject(GetHashCode().ToString());
        }

        internal void SetEventHandler(IRtcEngineEventHandler engineEventHandler)
        {
            _engineEventHandler = engineEventHandler;
        }

        internal void OnEvent(string @event, string data)
        {
            switch (@event)
            {
                case "onWarning":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnWarning((int) AgoraJson.GetData<int>(data, "warn"),
                            (string) AgoraJson.GetData<string>(data, "msg"));
                    });
                    break;
                case "onError":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnError((int) AgoraJson.GetData<int>(data, "err"),
                            (string) AgoraJson.GetData<string>(data, "msg"));
                    });
                    break;
                case "onJoinChannelSuccess":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnJoinChannelSuccess(
                            (string) AgoraJson.GetData<string>(data, "channel"),
                            (uint) AgoraJson.GetData<uint>(data, "uid"), (int) AgoraJson.GetData<int>(data, "elapsed"));
                    });
                    break;
                case "onRejoinChannelSuccess":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnRejoinChannelSuccess(
                            (string) AgoraJson.GetData<string>(data, "channel"),
                            (uint) AgoraJson.GetData<uint>(data, "uid"), (int) AgoraJson.GetData<int>(data, "elapsed"));
                    });
                    break;
                case "onLeaveChannel":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnLeaveChannel(
                            AgoraJson.JsonToStruct<RtcStats>(data, "stats"));
                    });
                    break;
                case "onClientRoleChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnClientRoleChanged(
                            (CLIENT_ROLE_TYPE) AgoraJson.GetData<int>(data, "oldRole"),
                            (CLIENT_ROLE_TYPE) AgoraJson.GetData<int>(data, "newRole"));
                    });
                    break;
                case "onUserJoined":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnUserJoined((uint) AgoraJson.GetData<uint>(data, "uid"),
                            (int) AgoraJson.GetData<int>(data, "elapsed"));
                    });
                    break;
                case "onUserOffline":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnUserOffline((uint) AgoraJson.GetData<uint>(data, "uid"),
                            (USER_OFFLINE_REASON_TYPE) AgoraJson.GetData<int>(data, "reason"));
                    });
                    break;
                case "onLastmileQuality":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnLastmileQuality(
                            (int) AgoraJson.GetData<int>(data, "quality"));
                    });
                    break;
                case "onLastmileProbeResult":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnLastmileProbeResult(
                            AgoraJson.JsonToStruct<LastmileProbeResult>(data, "result"));
                    });
                    break;
                case "onConnectionInterrupted":
                    _callbackObject._CallbackQueue.EnQueue(() => { _engineEventHandler?.OnConnectionInterrupted(); });
                    break;
                case "onConnectionLost":
                    _callbackObject._CallbackQueue.EnQueue(() => { _engineEventHandler?.OnConnectionLost(); });
                    break;
                case "onConnectionBanned":
                    _callbackObject._CallbackQueue.EnQueue(() => { _engineEventHandler?.OnConnectionBanned(); });
                    break;
                case "onApiCallExecuted":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnApiCallExecuted(
                            (int) AgoraJson.GetData<int>(data, "err"), (string) AgoraJson.GetData<string>(data, "api"),
                            (string) AgoraJson.GetData<string>(data, "result"));
                    });
                    break;
                case "onRequestToken":
                    _callbackObject._CallbackQueue.EnQueue(() => { _engineEventHandler?.OnRequestToken(); });
                    break;
                case "onTokenPrivilegeWillExpire":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnTokenPrivilegeWillExpire(
                            (string) AgoraJson.GetData<string>(data, "token"));
                    });
                    break;
                case "onAudioQuality":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnAudioQuality((uint) AgoraJson.GetData<uint>(data, "uid"),
                            (int) AgoraJson.GetData<int>(data, "quality"),
                            (ushort) AgoraJson.GetData<ushort>(data, "delay"),
                            (ushort) AgoraJson.GetData<ushort>(data, "lost"));
                    });
                    break;
                case "onRtcStats":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnRtcStats(
                            AgoraJson.JsonToStruct<RtcStats>(data, "stats"));
                    });
                    break;
                case "onNetworkQuality":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnNetworkQuality((uint) AgoraJson.GetData<uint>(data, "uid"),
                            (int) AgoraJson.GetData<int>(data, "txQuality"),
                            (int) AgoraJson.GetData<int>(data, "rxQuality"));
                    });
                    break;
                case "onLocalVideoStats":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnLocalVideoStats(
                            AgoraJson.JsonToStruct<LocalVideoStats>(data, "stats"));
                    });
                    break;
                case "onRemoteVideoStats":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnRemoteVideoStats(
                            AgoraJson.JsonToStruct<RemoteVideoStats>(data, "stats"));
                    });
                    break;
                case "onLocalAudioStats":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnLocalAudioStats(
                            AgoraJson.JsonToStruct<LocalAudioStats>(data, "stats"));
                    });
                    break;
                case "onRemoteAudioStats":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnRemoteAudioStats(
                            AgoraJson.JsonToStruct<RemoteAudioStats>(data, "stats"));
                    });
                    break;
                case "onLocalAudioStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnLocalAudioStateChanged(
                            (LOCAL_AUDIO_STREAM_STATE) AgoraJson.GetData<int>(data, "state"),
                            (LOCAL_AUDIO_STREAM_ERROR) AgoraJson.GetData<int>(data, "error"));
                    });
                    break;
                case "onRemoteAudioStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnRemoteAudioStateChanged(
                            (uint) AgoraJson.GetData<uint>(data, "uid"),
                            (REMOTE_AUDIO_STATE) AgoraJson.GetData<int>(data, "state"),
                            (REMOTE_AUDIO_STATE_REASON) AgoraJson.GetData<int>(data, "reason"),
                            (int) AgoraJson.GetData<int>(data, "elapsed"));
                    });
                    break;
                case "onAudioPublishStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnAudioPublishStateChanged(
                            (string) AgoraJson.GetData<string>(data, "channel"),
                            (STREAM_PUBLISH_STATE) AgoraJson.GetData<int>(data, "oldState"),
                            (STREAM_PUBLISH_STATE) AgoraJson.GetData<int>(data, "newState"),
                            (int) AgoraJson.GetData<int>(data, "elapseSinceLastState"));
                    });
                    break;
                case "onVideoPublishStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnVideoPublishStateChanged(
                            (string) AgoraJson.GetData<string>(data, "channel"),
                            (STREAM_PUBLISH_STATE) AgoraJson.GetData<int>(data, "oldState"),
                            (STREAM_PUBLISH_STATE) AgoraJson.GetData<int>(data, "newState"),
                            (int) AgoraJson.GetData<int>(data, "elapseSinceLastState"));
                    });
                    break;
                case "onAudioSubscribeStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnAudioSubscribeStateChanged(
                            (string) AgoraJson.GetData<string>(data, "channel"),
                            (uint) AgoraJson.GetData<uint>(data, "uid"),
                            (STREAM_SUBSCRIBE_STATE) AgoraJson.GetData<int>(data, "oldState"),
                            (STREAM_SUBSCRIBE_STATE) AgoraJson.GetData<int>(data, "newState"),
                            (int) AgoraJson.GetData<int>(data, "elapseSinceLastState"));
                    });
                    break;
                case "onVideoSubscribeStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnVideoSubscribeStateChanged(
                            (string) AgoraJson.GetData<string>(data, "channel"),
                            (uint) AgoraJson.GetData<uint>(data, "uid"),
                            (STREAM_SUBSCRIBE_STATE) AgoraJson.GetData<int>(data, "oldState"),
                            (STREAM_SUBSCRIBE_STATE) AgoraJson.GetData<int>(data, "newState"),
                            (int) AgoraJson.GetData<int>(data, "elapseSinceLastState"));
                    });
                    break;
                case "onAudioVolumeIndication":
                    var speakerNumber = (uint) AgoraJson.GetData<uint>(data, "speakerNumber");
                    var speakers = AgoraJson.JsonToStructArray<AudioVolumeInfo>(data, "speakers", speakerNumber);
                    var totalVolume = (int) AgoraJson.GetData<int>(data, "totalVolume");
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnAudioVolumeIndication(speakers, speakerNumber, totalVolume);
                    });
                    break;
                case "onActiveSpeaker":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnActiveSpeaker((uint) AgoraJson.GetData<uint>(data, "uid"));
                    });
                    break;
                case "onVideoStopped":
                    _callbackObject._CallbackQueue.EnQueue(() => { _engineEventHandler?.OnVideoStopped(); });
                    break;
                case "onFirstLocalVideoFrame":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnFirstLocalVideoFrame(
                            (int) AgoraJson.GetData<int>(data, "width"),
                            (int) AgoraJson.GetData<int>(data, "height"),
                            (int) AgoraJson.GetData<int>(data, "elapsed"));
                    });
                    break;
                case "onFirstLocalVideoFramePublished":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnFirstLocalVideoFramePublished(
                            (int) AgoraJson.GetData<int>(data, "elapsed"));
                    });
                    break;
                case "onFirstRemoteVideoDecoded":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnFirstRemoteVideoDecoded(
                            (uint) AgoraJson.GetData<uint>(data, "uid"),
                            (int) AgoraJson.GetData<int>(data, "width"), (int) AgoraJson.GetData<int>(data, "height"),
                            (int) AgoraJson.GetData<int>(data, "elapsed"));
                    });
                    break;
                case "onFirstRemoteVideoFrame":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnFirstRemoteVideoFrame(
                            (uint) AgoraJson.GetData<uint>(data, "uid"),
                            (int) AgoraJson.GetData<int>(data, "width"), (int) AgoraJson.GetData<int>(data, "height"),
                            (int) AgoraJson.GetData<int>(data, "elapsed"));
                    });
                    break;
                case "onUserMuteAudio":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnUserMuteAudio((uint) AgoraJson.GetData<uint>(data, "uid"),
                            (bool) AgoraJson.GetData<bool>(data, "muted"));
                    });
                    break;
                case "onUserMuteVideo":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnUserMuteVideo((uint) AgoraJson.GetData<uint>(data, "uid"),
                            (bool) AgoraJson.GetData<bool>(data, "muted"));
                    });
                    break;
                case "onUserEnableVideo":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnUserEnableVideo((uint) AgoraJson.GetData<uint>(data, "uid"),
                            (bool) AgoraJson.GetData<bool>(data, "enabled"));
                    });
                    break;
                case "onAudioDeviceStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnAudioDeviceStateChanged(
                            (string) AgoraJson.GetData<string>(data, "deviceId"),
                            (int) AgoraJson.GetData<int>(data, "deviceType"),
                            (int) AgoraJson.GetData<int>(data, "deviceState"));
                    });
                    break;
                case "onAudioDeviceVolumeChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnAudioDeviceVolumeChanged(
                            (MEDIA_DEVICE_TYPE) AgoraJson.GetData<int>(data, "deviceType"),
                            (int) AgoraJson.GetData<int>(data, "volume"),
                            (bool) AgoraJson.GetData<bool>(data, "muted"));
                    });
                    break;
                case "onCameraReady":
                    _callbackObject._CallbackQueue.EnQueue(() => { _engineEventHandler?.OnCameraReady(); });
                    break;
                case "onCameraFocusAreaChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnCameraFocusAreaChanged(
                            (int) AgoraJson.GetData<int>(data, "x"),
                            (int) AgoraJson.GetData<int>(data, "y"), (int) AgoraJson.GetData<int>(data, "width"),
                            (int) AgoraJson.GetData<int>(data, "height"));
                    });
                    break;
                case "onFacePositionChanged":
                    var numFaces = (int) AgoraJson.GetData<int>(data, "numFaces");
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnFacePositionChanged(
                            (int) AgoraJson.GetData<int>(data, "imageWidth"),
                            (int) AgoraJson.GetData<int>(data, "imageHeight"),
                            AgoraJson.JsonToStruct<Rectangle>((string) AgoraJson.GetData<string>(data, "vecRectangle")),
                            AgoraJson.JsonToStructArray<int>(data, "vecDistance", (uint) numFaces), numFaces);
                    });
                    break;
                case "onCameraExposureAreaChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnCameraExposureAreaChanged(
                            (int) AgoraJson.GetData<int>(data, "x"),
                            (int) AgoraJson.GetData<int>(data, "y"), (int) AgoraJson.GetData<int>(data, "width"),
                            (int) AgoraJson.GetData<int>(data, "height"));
                    });
                    break;
                case "onAudioMixingFinished":
                    _callbackObject._CallbackQueue.EnQueue(() => { _engineEventHandler?.OnAudioMixingFinished(); });
                    break;
                case "onAudioMixingStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnAudioMixingStateChanged(
                            (AUDIO_MIXING_STATE_TYPE) AgoraJson.GetData<int>(data, "state"),
                            (AUDIO_MIXING_ERROR_TYPE) AgoraJson.GetData<int>(data, "errorCode"));
                    });
                    break;
                case "onRemoteAudioMixingBegin":
                    _callbackObject._CallbackQueue.EnQueue(() => { _engineEventHandler?.OnRemoteAudioMixingBegin(); });
                    break;
                case "onRemoteAudioMixingEnd":
                    _callbackObject._CallbackQueue.EnQueue(() => { _engineEventHandler?.OnRemoteAudioMixingEnd(); });
                    break;
                case "onAudioEffectFinished":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnAudioEffectFinished(
                            (int) AgoraJson.GetData<int>(data, "soundId"));
                    });
                    break;
                case "onFirstRemoteAudioDecoded":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnFirstRemoteAudioDecoded(
                            (uint) AgoraJson.GetData<uint>(data, "uid"),
                            (int) AgoraJson.GetData<int>(data, "elapsed"));
                    });
                    break;
                case "onVideoDeviceStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnVideoDeviceStateChanged(
                            (string) AgoraJson.GetData<string>(data, "deviceId"),
                            (int) AgoraJson.GetData<int>(data, "deviceType"),
                            (int) AgoraJson.GetData<int>(data, "deviceState"));
                    });
                    break;
                case "onLocalVideoStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnLocalVideoStateChanged(
                            (LOCAL_VIDEO_STREAM_STATE) AgoraJson.GetData<int>(data, "localVideoState"),
                            (LOCAL_VIDEO_STREAM_ERROR) AgoraJson.GetData<int>(data, "error"));
                    });
                    break;
                case "onVideoSizeChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnVideoSizeChanged((uint) AgoraJson.GetData<uint>(data, "uid"),
                            (int) AgoraJson.GetData<int>(data, "width"), (int) AgoraJson.GetData<int>(data, "height"),
                            (int) AgoraJson.GetData<int>(data, "rotation"));
                    });
                    break;
                case "onRemoteVideoStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnRemoteVideoStateChanged(
                            (uint) AgoraJson.GetData<uint>(data, "uid"),
                            (REMOTE_VIDEO_STATE) AgoraJson.GetData<int>(data, "state"),
                            (REMOTE_VIDEO_STATE_REASON) AgoraJson.GetData<int>(data, "reason"),
                            (int) AgoraJson.GetData<int>(data, "elapsed"));
                    });
                    break;
                case "onUserEnableLocalVideo":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnUserEnableLocalVideo(
                            (uint) AgoraJson.GetData<uint>(data, "uid"),
                            (bool) AgoraJson.GetData<bool>(data, "enabled"));
                    });
                    break;
                case "onStreamMessageError":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnStreamMessageError(
                            (uint) AgoraJson.GetData<uint>(data, "uid"),
                            (int) AgoraJson.GetData<int>(data, "streamId"), (int) AgoraJson.GetData<int>(data, "code"),
                            (int) AgoraJson.GetData<int>(data, "missed"), (int) AgoraJson.GetData<int>(data, "cached"));
                    });
                    break;
                case "onMediaEngineLoadSuccess":
                    _callbackObject._CallbackQueue.EnQueue(() => { _engineEventHandler?.OnMediaEngineLoadSuccess(); });
                    break;
                case "onMediaEngineStartCallSuccess":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnMediaEngineStartCallSuccess();
                    });
                    break;
                case "onUserSuperResolutionEnabled":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnUserSuperResolutionEnabled(
                            (uint) AgoraJson.GetData<uint>(data, "uid"),
                            (bool) AgoraJson.GetData<bool>(data, "enabled"),
                            (SUPER_RESOLUTION_STATE_REASON) AgoraJson.GetData<int>(data, "reason"));
                    });
                    break;
                case "onChannelMediaRelayStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnChannelMediaRelayStateChanged(
                            (CHANNEL_MEDIA_RELAY_STATE) AgoraJson.GetData<int>(data, "state"),
                            (CHANNEL_MEDIA_RELAY_ERROR) AgoraJson.GetData<int>(data, "code"));
                    });
                    break;
                case "onChannelMediaRelayEvent":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnChannelMediaRelayEvent(
                            (CHANNEL_MEDIA_RELAY_EVENT) AgoraJson.GetData<int>(data, "code"));
                    });
                    break;
                case "onFirstLocalAudioFrame":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnFirstLocalAudioFrame(
                            (int) AgoraJson.GetData<int>(data, "elapsed"));
                    });
                    break;
                case "onFirstLocalAudioFramePublished":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnFirstLocalAudioFramePublished(
                            (int) AgoraJson.GetData<int>(data, "elapsed"));
                    });
                    break;
                case "onFirstRemoteAudioFrame":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnFirstRemoteAudioFrame(
                            (uint) AgoraJson.GetData<uint>(data, "uid"),
                            (int) AgoraJson.GetData<int>(data, "elapsed"));
                    });
                    break;
                case "onRtmpStreamingStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnRtmpStreamingStateChanged(
                            (string) AgoraJson.GetData<string>(data, "url"),
                            (RTMP_STREAM_PUBLISH_STATE) AgoraJson.GetData<int>(data, "state"),
                            (RTMP_STREAM_PUBLISH_ERROR) AgoraJson.GetData<int>(data, "errCode"));
                    });
                    break;
                case "onRtmpStreamingEvent":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnRtmpStreamingEvent(
                            (string) AgoraJson.GetData<string>(data, "url"),
                            (RTMP_STREAMING_EVENT) AgoraJson.GetData<int>(data, "eventCode"));
                    });
                    break;
                case "onStreamPublished":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnStreamPublished(
                            (string) AgoraJson.GetData<string>(data, "url"),
                            (int) AgoraJson.GetData<int>(data, "error"));
                    });
                    break;
                case "onStreamUnpublished":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnStreamUnpublished(
                            (string) AgoraJson.GetData<string>(data, "url"));
                    });
                    break;
                case "onTranscodingUpdated":
                    _callbackObject._CallbackQueue.EnQueue(() => { _engineEventHandler?.OnTranscodingUpdated(); });
                    break;
                case "onStreamInjectedStatus":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnStreamInjectedStatus(
                            (string) AgoraJson.GetData<string>(data, "url"),
                            (uint) AgoraJson.GetData<uint>(data, "uid"), (int) AgoraJson.GetData<int>(data, "status"));
                    });
                    break;
                case "onAudioRouteChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnAudioRouteChanged(
                            (AUDIO_ROUTE_TYPE) AgoraJson.GetData<int>(data, "routing"));
                    });
                    break;
                case "onLocalPublishFallbackToAudioOnly":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnLocalPublishFallbackToAudioOnly(
                            (bool) AgoraJson.GetData<bool>(data, "isFallbackOrRecover"));
                    });
                    break;
                case "onRemoteSubscribeFallbackToAudioOnly":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnRemoteSubscribeFallbackToAudioOnly(
                            (uint) AgoraJson.GetData<uint>(data, "uid"),
                            (bool) AgoraJson.GetData<bool>(data, "isFallbackOrRecover"));
                    });
                    break;
                case "onRemoteAudioTransportStats":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnRemoteAudioTransportStats(
                            (uint) AgoraJson.GetData<uint>(data, "uid"),
                            (ushort) AgoraJson.GetData<ushort>(data, "delay"),
                            (ushort) AgoraJson.GetData<ushort>(data, "lost"),
                            (ushort) AgoraJson.GetData<ushort>(data, "rxKBitRate"));
                    });
                    break;
                case "onRemoteVideoTransportStats":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnRemoteVideoTransportStats(
                            (uint) AgoraJson.GetData<uint>(data, "uid"),
                            (ushort) AgoraJson.GetData<ushort>(data, "delay"),
                            (ushort) AgoraJson.GetData<ushort>(data, "lost"),
                            (ushort) AgoraJson.GetData<ushort>(data, "rxKBitRate"));
                    });
                    break;
                case "onMicrophoneEnabled":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnMicrophoneEnabled(
                            (bool) AgoraJson.GetData<bool>(data, "enabled"));
                    });
                    break;
                case "onConnectionStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnConnectionStateChanged(
                            (CONNECTION_STATE_TYPE) AgoraJson.GetData<int>(data, "state"),
                            (CONNECTION_CHANGED_REASON_TYPE) AgoraJson.GetData<int>(data, "reason"));
                    });
                    break;
                case "onNetworkTypeChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnNetworkTypeChanged(
                            (NETWORK_TYPE) AgoraJson.GetData<int>(data, "type"));
                    });
                    break;
                case "onLocalUserRegistered":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnLocalUserRegistered(
                            (uint) AgoraJson.GetData<uint>(data, "uid"),
                            (string) AgoraJson.GetData<string>(data, "userAccount"));
                    });
                    break;
                case "onUserInfoUpdated":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnUserInfoUpdated((uint) AgoraJson.GetData<uint>(data, "uid"),
                            AgoraJson.JsonToStruct<UserInfo>(data, "info"));
                    });
                    break;
                case "onUploadLogResult":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnUploadLogResult(
                            (string) AgoraJson.GetData<string>(data, "requestId"),
                            (bool) AgoraJson.GetData<bool>(data, "success"),
                            (UPLOAD_ERROR_REASON) AgoraJson.GetData<int>(data, "reason"));
                    });
                    break;
            }
        }

        internal void OnEventWithBuffer(string @event, string data, IntPtr buffer, uint length)
        {
            switch (@event)
            {
                case "onStreamMessage":
                    var streamData = new byte[length];
                    if (buffer != IntPtr.Zero) Marshal.Copy(buffer, streamData, 0, (int) length);
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        _engineEventHandler?.OnStreamMessage((uint) AgoraJson.GetData<uint>(data, "uid"),
                            (int) AgoraJson.GetData<int>(data, "streamId"), streamData, length);
                    });
                    break;
            }
        }

        internal void Dispose()
        {
            _engineEventHandler = null;
            _callbackObject.Release();
            _callbackObject = null;
        }
    }

    public sealed class AgoraRtcEngine : IAgoraRtcEngine, IDisposable
    {
        private bool _disposed;

        private static AgoraRtcEngine[] _engineInstance = {null, null};
        private IrisRtcEnginePtr _irisRtcEngine;

        private NativeRtcEngineEventHandler _nativeRtcEngineEventHandler;
        private IrisCEventHandlerNative _irisCEngineEventHandlerNative;
        private IrisCEventHandler _irisCEngineEventHandler;
        private IrisEventHandlerHandleNative _irisEngineEventHandlerHandleNative;

        private CharArrayAssistant _result;

        private AgoraRtcEngine(EngineType type = EngineType.kEngineTypeNormal)
        {
            _result = new CharArrayAssistant();
            _irisRtcEngine = type == EngineType.kEngineTypeNormal
                ? AgoraRtcNative.CreateIrisRtcEngine()
                : AgoraRtcNative.CreateIrisRtcEngine(EngineType.kEngineTypeSubProcess);
            SetIrisEngineEventHandler();
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // TODO: Unmanaged resources.
                UnsetIrisRtcEngineEventHandler();
            }

            Release();

            _disposed = true;
        }

        private void Release(bool sync = false)
        {
            var param = new
            {
                sync
            };
            AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineRelease,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
            AgoraRtcNative.DestroyIrisRtcEngine(_irisRtcEngine);
            _irisRtcEngine = IntPtr.Zero;
            _result = new CharArrayAssistant();
            for (var i = 0; i < _engineInstance.Length; i++)
            {
                if (_engineInstance[i] == this) _engineInstance[i] = null;
            }
        }

        internal IrisRtcEnginePtr GetNativeHandler()
        {
            return _irisRtcEngine;
        }

        public static IAgoraRtcEngine CreateAgoraRtcEngine()
        {
            return _engineInstance[0] ?? (_engineInstance[0] = new AgoraRtcEngine());
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
            return _engineInstance[0];
        }

        public static IAgoraRtcEngine CreateAgoraSubRtcEngine()
        {
            return _engineInstance[1] ?? (_engineInstance[1] = new AgoraRtcEngine(EngineType.kEngineTypeSubProcess));
        }

        private int SetAppType(AppType appType)
        {
            var param = new
            {
                appType
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetAppType,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int Initialize(RtcEngineContext context)
        {
            var param = new
            {
                context
            };
            var ret = AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineInitialize,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
            if (ret == 0) SetAppType(AppType.APP_TYPE_UNITY);
            return ret;
        }

        public override void InitEventHandler(IRtcEngineEventHandler engineEventHandler)
        {
            _nativeRtcEngineEventHandler.SetEventHandler(engineEventHandler);
        }

        private void SetIrisEngineEventHandler()
        {
            _nativeRtcEngineEventHandler = new NativeRtcEngineEventHandler();

            _irisCEngineEventHandler = new IrisCEventHandler
            {
                OnEvent = _nativeRtcEngineEventHandler.OnEvent,
                OnEventWithBuffer = _nativeRtcEngineEventHandler.OnEventWithBuffer
            };

            _irisCEngineEventHandlerNative = new IrisCEventHandlerNative
            {
                onEvent = Marshal.GetFunctionPointerForDelegate(_irisCEngineEventHandler.OnEvent),
                onEventWithBuffer = Marshal.GetFunctionPointerForDelegate(_irisCEngineEventHandler.OnEventWithBuffer)
            };

            _irisEngineEventHandlerHandleNative =
                AgoraRtcNative.SetIrisRtcEngineEventHandler(_irisRtcEngine, ref _irisCEngineEventHandlerNative);
        }

        private void UnsetIrisRtcEngineEventHandler()
        {
            AgoraRtcNative.UnsetIrisRtcEngineEventHandler(_irisRtcEngine, ref _irisEngineEventHandlerHandleNative);
            _irisEngineEventHandlerHandleNative = IntPtr.Zero;
            _nativeRtcEngineEventHandler?.Dispose();
            _nativeRtcEngineEventHandler = null;
            _irisCEngineEventHandler = new IrisCEventHandler();
            _irisCEngineEventHandlerNative = new IrisCEventHandlerNative();
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [Obsolete(ObsoleteMethodWarning.DestroyWarning, true)]
        public static void Destroy(AgoraRtcEngine rtcEngine = null)
        {
            if (rtcEngine == null)
            {
                _engineInstance[0]?.Dispose();
            }
            else
            {
                rtcEngine.Dispose();
            }
        }

        public override IAudioEffectManager GetAudioEffectManager()
        {
            throw new NotImplementedException();
        }

        public override IAudioRecordingDeviceManager GetAudioRecordingDeviceManager()
        {
            throw new NotImplementedException();
        }

        public override IAudioPlaybackDeviceManager GetAudioPlaybackDeviceManager()
        {
            throw new NotImplementedException();
        }

        public override IVideoDeviceManager GetVideoDeviceManager()
        {
            throw new NotImplementedException();
        }

        public override IAudioRawDataManager GetAudioRawDataManager()
        {
            throw new NotImplementedException();
        }

        public override IVideoRawDataManager GetVideoRawDataManager()
        {
            throw new NotImplementedException();
        }

        public override IVideoRender GetVideoRender()
        {
            throw new NotImplementedException();
        }

        public override IAgoraRtcChannel CreateChannel(string channelId)
        {
            throw new NotImplementedException();
        }

        internal void ReleaseChannel(string channelId)
        {
            throw new NotImplementedException();
        }

        public override int SetChannelProfile(CHANNEL_PROFILE_TYPE profile)
        {
            var param = new
            {
                profile
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetChannelProfile,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int SetClientRole(CLIENT_ROLE_TYPE role)
        {
            var param = new
            {
                role
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetClientRole,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int SetClientRole(CLIENT_ROLE_TYPE role, ClientRoleOptions options)
        {
            var param = new
            {
                role,
                options
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetClientRole,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int LeaveChannel()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineLeaveChannel,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int RenewToken(string token)
        {
            var param = new
            {
                token
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineRenewToken,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int GetUserInfoByUserAccount(string userAccount, out UserInfo userInfo)
        {
            var param = new
            {
                userAccount
            };
            var ret = AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineGetUserInfoByUserAccount,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
            return _result.Result.Length == 0 ? new UserInfo() : AgoraJson.JsonToStruct<UserInfo>(_result.Result);
        }

        [Obsolete(ObsoleteMethodWarning.GeneralWarning, false)]
        public override int StartEchoTest()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineStartEchoTest,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int StartEchoTest(int intervalInSeconds)
        {
            var param = new
            {
                intervalInSeconds
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineStartEchoTest,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int StopEchoTest()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineStopEchoTest,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int SetCloudProxy(CLOUD_PROXY_TYPE proxyType)
        {
            var param = new
            {
                proxyType
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetCloudProxy,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int EnableVideo()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineEnableVideo,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int DisableVideo()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineDisableVideo,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int SetVideoEncoderConfiguration(VideoEncoderConfiguration config)
        {
            var param = new
            {
                config
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetVideoEncoderConfiguration, Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)),
                out _result);
        }

        public override int SetCameraCapturerConfiguration(CameraCapturerConfiguration config)
        {
            var param = new
            {
                config
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetCameraCapturerConfiguration, Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)),
                out _result);
        }

        public override int SetupLocalVideo(VideoCanvas canvas)
        {
            var param = new
            {
                canvas
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetupLocalVideo,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int SetupRemoteVideo(VideoCanvas canvas)
        {
            var param = new
            {
                canvas
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetupRemoteVideo,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int StartPreview()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineStartPreview,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int StopPreview()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineStopPreview,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int EnableAudio()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineEnableAudio,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int EnableLocalAudio(bool enabled)
        {
            var param = new
            {
                enabled
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineEnableLocalAudio,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int DisableAudio()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineDisableAudio,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int SetAudioProfile(AUDIO_PROFILE_TYPE profile, AUDIO_SCENARIO_TYPE scenario)
        {
            var param = new
            {
                profile,
                scenario
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetAudioProfile,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int MuteLocalAudioStream(bool mute)
        {
            var param = new
            {
                mute
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineMuteLocalAudioStream,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int MuteAllRemoteAudioStreams(bool mute)
        {
            var param = new
            {
                mute
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineMuteAllRemoteAudioStreams,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int AdjustUserPlaybackSignalVolume(uint uid, int volume)
        {
            var param = new
            {
                uid,
                volume
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineAdjustUserPlaybackSignalVolume, Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)),
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int MuteLocalVideoStream(bool mute)
        {
            var param = new
            {
                mute
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineMuteLocalVideoStream,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int EnableLocalVideo(bool enabled)
        {
            var param = new
            {
                enabled
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineEnableLocalVideo,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int MuteAllRemoteVideoStreams(bool mute)
        {
            var param = new
            {
                mute
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineMuteAllRemoteVideoStreams,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int SetRemoteDefaultVideoStreamType(REMOTE_VIDEO_STREAM_TYPE streamType)
        {
            var param = new
            {
                streamType
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetRemoteDefaultVideoStreamType, Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)),
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int StopAudioRecording()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineStopAudioRecording,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int StopAudioMixing()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineStopAudioMixing,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int PauseAudioMixing()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEnginePauseAudioMixing,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int ResumeAudioMixing()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineResumeAudioMixing,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                ApiTypeEngine.kEngineSetHighQualityAudioParameters, Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)),
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int AdjustAudioMixingPlayoutVolume(int volume)
        {
            var param = new
            {
                volume
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineAdjustAudioMixingPlayoutVolume, Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)),
                out _result);
        }

        public override int GetAudioMixingPlayoutVolume()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineGetAudioMixingPlayoutVolume,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int AdjustAudioMixingPublishVolume(int volume)
        {
            var param = new
            {
                volume
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineAdjustAudioMixingPublishVolume, Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)),
                out _result);
        }

        public override int GetAudioMixingPublishVolume()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineGetAudioMixingPublishVolume,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int GetAudioMixingDuration()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineGetAudioMixingDuration,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int GetAudioMixingCurrentPosition()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineGetAudioMixingCurrentPosition, Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)),
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int SetAudioMixingPitch(int pitch)
        {
            var param = new
            {
                pitch
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetAudioMixingPitch,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int GetEffectsVolume()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineGetEffectsVolume,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int SetEffectsVolume(int volume)
        {
            var param = new
            {
                volume
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetEffectsVolume,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int EnableFaceDetection(bool enable)
        {
            var param = new
            {
                enable
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineEnableFaceDetection,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int PlayEffect(int soundId, string filePath, int loopCount, double pitch, double pan, int gain,
            bool publish)
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int StopEffect(int soundId)
        {
            var param = new
            {
                soundId
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineStopEffect,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int StopAllEffects()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineStopAllEffects,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int PreloadEffect(int soundId, string filePath)
        {
            var param = new
            {
                soundId,
                filePath
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEnginePreloadEffect,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int UnloadEffect(int soundId)
        {
            var param = new
            {
                soundId
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineUnloadEffect,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int PauseEffect(int soundId)
        {
            var param = new
            {
                soundId
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEnginePauseEffect,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int PauseAllEffects()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEnginePauseAllEffects,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int ResumeEffect(int soundId)
        {
            var param = new
            {
                soundId
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineResumeEffect,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int ResumeAllEffects()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineResumeAllEffects,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int EnableDeepLearningDenoise(bool enable)
        {
            var param = new
            {
                enable
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineEnableDeepLearningDenoise,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int EnableSoundPositionIndication(bool enabled)
        {
            var param = new
            {
                enabled
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineEnableSoundPositionIndication, Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)),
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int SetLocalVoicePitch(double pitch)
        {
            var param = new
            {
                pitch
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetLocalVoicePitch,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int SetVoiceBeautifierPreset(VOICE_BEAUTIFIER_PRESET preset)
        {
            var param = new
            {
                preset
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetVoiceBeautifierPreset,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int SetAudioEffectPreset(AUDIO_EFFECT_PRESET preset)
        {
            var param = new
            {
                preset
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetAudioEffectPreset,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int SetVoiceConversionPreset(VOICE_CONVERSION_PRESET preset)
        {
            var param = new
            {
                preset
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetVoiceConversionPreset,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                ApiTypeEngine.kEngineSetVoiceBeautifierParameters, Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)),
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        [Obsolete(ObsoleteMethodWarning.SetLogFilterWarning, false)]
        public override int SetLogFilter(uint filter)
        {
            var param = new
            {
                filter
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetLogFilter,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        [Obsolete(ObsoleteMethodWarning.SetLogFileSizeWarning, false)]
        public override int SetLogFileSize(uint fileSizeInKBytes)
        {
            var param = new
            {
                fileSizeInKBytes
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetLogFileSize,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override string UploadLogFile()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineUploadLogFile,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result) != 0
                ? ""
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int EnableDualStreamMode(bool enabled)
        {
            var param = new
            {
                enabled
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineEnableDualStreamMode,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                ApiTypeEngine.kEngineSetRecordingAudioFrameParameters, Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)),
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
                ApiTypeEngine.kEngineSetPlaybackAudioFrameParameters, Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)),
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
                ApiTypeEngine.kEngineSetMixedAudioFrameParameters, Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)),
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int AdjustPlaybackSignalVolume(int volume)
        {
            var param = new
            {
                volume
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineAdjustPlaybackSignalVolume,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        [Obsolete(ObsoleteMethodWarning.GeneralWarning, false)]
        public override int EnableWebSdkInteroperability(bool enabled)
        {
            var param = new
            {
                enabled
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineEnableWebSdkInteroperability, Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)),
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int SetLocalPublishFallbackOption(STREAM_FALLBACK_OPTIONS option)
        {
            var param = new
            {
                option
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetLocalPublishFallbackOption, Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)),
                out _result);
        }

        public override int SetRemoteSubscribeFallbackOption(STREAM_FALLBACK_OPTIONS option)
        {
            var param = new
            {
                option
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetRemoteSubscribeFallbackOption, Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)),
                out _result);
        }

        public override int SwitchCamera()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSwitchCamera,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int SetDefaultAudioRouteToSpeakerphone(bool defaultToSpeaker)
        {
            var param = new
            {
                defaultToSpeaker
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetDefaultAudioRouteToSpeakerPhone,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int SetEnableSpeakerphone(bool speakerOn)
        {
            var param = new
            {
                speakerOn
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetEnableSpeakerPhone,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int EnableInEarMonitoring(bool enabled)
        {
            var param = new
            {
                enabled
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineEnableInEarMonitoring,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int SetInEarMonitoringVolume(int volume)
        {
            var param = new
            {
                volume
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetInEarMonitoringVolume,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override bool IsSpeakerphoneEnabled()
        {
            var param = new { };
            var ret = AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineIsSpeakerPhoneEnabled,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int StartScreenCaptureByDisplayId(uint displayId, Rectangle regionRect,
            ScreenCaptureParameters captureParams)
        {
            var param = new
            {
                displayId,
                regionRect,
                captureParams
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineStartScreenCaptureByDisplayId, Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)),
                out _result);
        }

        public override int StartScreenCaptureByScreenRect(Rectangle screenRect, Rectangle regionRect,
            ScreenCaptureParameters captureParams)
        {
            var param = new
            {
                screenRect,
                regionRect,
                captureParams
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineStartScreenCaptureByScreenRect, Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)),
                out _result);
        }

        public override int StartScreenCaptureByWindowId(view_t windowId, Rectangle regionRect,
            ScreenCaptureParameters captureParams)
        {
            var param = new
            {
                windowId,
                regionRect,
                captureParams
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineStartScreenCaptureByWindowId, Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)),
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int UpdateScreenCaptureParameters(ScreenCaptureParameters captureParams)
        {
            var param = new
            {
                captureParams
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineUpdateScreenCaptureParameters, Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)),
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int StopScreenCapture()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineStopScreenCapture,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override string GetCallId()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineGetCallId,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result) != 0
                ? ""
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int Complain(string callId, string description = "")
        {
            var param = new
            {
                callId,
                description
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineComplain,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override string GetVersion()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineGetVersion,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result) != 0
                ? ""
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int DisableLastmileTest()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineDisableLastMileTest,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int StartLastmileProbeTest(LastmileProbeConfig config)
        {
            var param = new
            {
                config
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineStartLastMileProbeTest,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int StopLastmileProbeTest()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineStopLastMileProbeTest,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override string GetErrorDescription(int code)
        {
            var param = new
            {
                code
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineGetErrorDescription,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result) != 0
                ? ""
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int EnableEncryption(bool enabled, EncryptionConfig config)
        {
            var param = new
            {
                enabled,
                config
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineEnableEncryption,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int RegisterPacketObserver(IPacketObserver observer)
        {
            var param = new
            {
                observer
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineRegisterPacketObserver,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int CreateDataStream(DataStreamConfig config)
        {
            var param = new
            {
                config
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineCreateDataStream,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), data, out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int RemovePublishStreamUrl(string url)
        {
            var param = new
            {
                url
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineRemovePublishStreamUrl,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int SetLiveTranscoding(LiveTranscoding transcoding)
        {
            var param = new
            {
                transcoding
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetLiveTranscoding,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int ClearVideoWatermarks()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineClearVideoWaterMarks,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int StartChannelMediaRelay(ChannelMediaRelayConfiguration configuration)
        {
            var param = new
            {
                configuration
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineStartChannelMediaRelay,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int UpdateChannelMediaRelay(ChannelMediaRelayConfiguration configuration)
        {
            var param = new
            {
                configuration
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineUpdateChannelMediaRelay,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int StopChannelMediaRelay()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineStopChannelMediaRelay,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int RemoveInjectStreamUrl(string url)
        {
            var param = new
            {
                url
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineRemoveInjectStreamUrl,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override CONNECTION_STATE_TYPE GetConnectionState()
        {
            var param = new { };
            return (CONNECTION_STATE_TYPE) AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineGetConnectionState, Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int RegisterMediaMetadataObserver(METADATA_TYPE type)
        {
            var param = new
            {
                type
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineRegisterMediaMetadataObserver, Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)),
                out _result);
        }

        public override int UnRegisterMediaMetadataObserver(METADATA_TYPE type)
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineUnRegisterMediaMetadataObserver, Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)),
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int SetParameters(string parameters)
        {
            var param = new
            {
                parameters
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetParameters,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int SetMaxMetadataSize(int size)
        {
            var param = new
            {
                size
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine,
                ApiTypeEngine.kEngineSetMaxMetadataSize,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), metadata.buffer, out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), frame.buffer, out _result);
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), frame.buffer, out _result);
        }

        public override int PullAudioFrame(AudioFrame frame)
        {
            var param = new { };
            var ret = AgoraRtcNative.CallIrisRtcEngineApiWithBuffer(_irisRtcEngine,
                ApiTypeEngine.kMediaPullAudioFrame,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), frame.buffer, out _result);
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
                ApiTypeEngine.kMediaSetExternalVideoSource, Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)),
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
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), frame.buffer, out _result);
        }

        ~AgoraRtcEngine()
        {
            Dispose(false);
        }
    }
}