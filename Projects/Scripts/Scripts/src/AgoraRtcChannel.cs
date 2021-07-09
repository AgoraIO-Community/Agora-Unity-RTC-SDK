//  AgoraRtcChannel.cs
//
//  Created by Yiqing Huang on June 6, 2021.
//  Modified by Yiqing Huang on June 6, 2021.
//
//  Copyright © 2021 Agora. All rights reserved.
//

using System;
using System.Runtime.InteropServices;
using LitJson;

namespace agora_gaming_rtc
{
    using IrisRtcChannelPtr = IntPtr;
    using IrisEventHandlerHandleNative = IntPtr;

    internal sealed class RtcChannelEventHandlerNative
    {
        private IAgoraRtcChannelEventHandler _channelEventHandler;

        private AgoraCallbackObject _callbackObject;

        private IrisRtcChannelPtr _irisRtcChannelPtr;
        private readonly string _channelId;
        private IrisEventHandlerHandleNative _irisChannelEventHandlerHandleNative;
        private IrisCEventHandler _irisCEventHandler;
        private IrisEventHandlerHandleNative _irisCChannelEventHandlerNative;

        internal RtcChannelEventHandlerNative(IrisRtcChannelPtr irisRtcChannelPtr, string channelId)
        {
            _irisRtcChannelPtr = irisRtcChannelPtr;
            _channelId = channelId;
            var name = "Agora" + GetHashCode().ToString();

            _irisCEventHandler = new IrisCEventHandler()
            {
                OnEvent = OnEvent,
                OnEventWithBuffer = OnEventWithBuffer
            };

            var cChannelEventHandlerNativeLocal = new IrisCEventHandlerNative
            {
                onEvent = Marshal.GetFunctionPointerForDelegate(_irisCEventHandler.OnEvent),
                onEventWithBuffer = Marshal.GetFunctionPointerForDelegate(_irisCEventHandler.OnEventWithBuffer)
            };

            _irisCChannelEventHandlerNative = Marshal.AllocHGlobal(Marshal.SizeOf(cChannelEventHandlerNativeLocal));
            Marshal.StructureToPtr(cChannelEventHandlerNativeLocal, _irisCChannelEventHandlerNative, true);
            _irisChannelEventHandlerHandleNative = AgoraRtcNative.RegisterIrisRtcChannelEventHandler(_irisRtcChannelPtr,
                _channelId,
                _irisCChannelEventHandlerNative);

            _callbackObject = new AgoraCallbackObject(name);
        }

        internal void Dispose()
        {
            _channelEventHandler = null;
            if (_callbackObject != null) _callbackObject.Release();
            _callbackObject = null;
            AgoraRtcNative.UnRegisterIrisRtcChannelEventHandler(_irisRtcChannelPtr,
                _irisChannelEventHandlerHandleNative, _channelId);
            _irisChannelEventHandlerHandleNative = IntPtr.Zero;
        }

        internal void SetEventHandler(IAgoraRtcChannelEventHandler channelEventHandler)
        {
            _channelEventHandler = channelEventHandler;
        }

        internal void OnEvent(string @event, string data)
        {
            if (_callbackObject == null || _callbackObject._CallbackQueue == null) return;
            switch (@event)
            {
                case "onChannelWarning":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnChannelWarning((string) AgoraJson.GetData<string>(data, "channelId"),
                                (int) AgoraJson.GetData<int>(data, "warn"),
                                (string) AgoraJson.GetData<string>(data, "msg"));
                        }
                    });
                    break;
                case "onChannelError":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnChannelError((string) AgoraJson.GetData<string>(data, "channelId"),
                                (int) AgoraJson.GetData<int>(data, "err"),
                                (string) AgoraJson.GetData<string>(data, "msg"));
                        }
                    });
                    break;
                case "onJoinChannelSuccess":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnJoinChannelSuccess(
                                (string) AgoraJson.GetData<string>(data, "channelId"),
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (int) AgoraJson.GetData<int>(data, "elapsed"));
                        }
                    });
                    break;
                case "onRejoinChannelSuccess":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnRejoinChannelSuccess(
                                (string) AgoraJson.GetData<string>(data, "channelId"),
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (int) AgoraJson.GetData<int>(data, "elapsed"));
                        }
                    });
                    break;
                case "onLeaveChannel":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnLeaveChannel((string) AgoraJson.GetData<string>(data, "channelId"),
                                AgoraJson.JsonToStruct<RtcStats>(data, "stats"));
                        }
                    });
                    break;
                case "onClientRoleChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnClientRoleChanged(
                                (string) AgoraJson.GetData<string>(data, "channelId"),
                                (CLIENT_ROLE_TYPE) AgoraJson.GetData<int>(data, "oldRole"),
                                (CLIENT_ROLE_TYPE) AgoraJson.GetData<int>(data, "newRole"));
                        }
                    });
                    break;
                case "onUserJoined":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnUserJoined((string) AgoraJson.GetData<string>(data, "channelId"),
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (int) AgoraJson.GetData<int>(data, "elapsed"));
                        }
                    });
                    break;
                case "onUserOffline":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnUserOffline((string) AgoraJson.GetData<string>(data, "channelId"),
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (USER_OFFLINE_REASON_TYPE) AgoraJson.GetData<int>(data, "reason"));
                        }
                    });
                    break;
                case "onConnectionLost":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnConnectionLost(
                                (string) AgoraJson.GetData<string>(data, "channelId"));
                        }
                    });
                    break;
                case "onRequestToken":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnRequestToken((string) AgoraJson.GetData<string>(data, "channelId"));
                        }
                    });
                    break;
                case "onTokenPrivilegeWillExpire":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnTokenPrivilegeWillExpire(
                                (string) AgoraJson.GetData<string>(data, "channelId"),
                                (string) AgoraJson.GetData<string>(data, "token"));
                        }
                    });
                    break;
                case "onRtcStats":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnRtcStats((string) AgoraJson.GetData<string>(data, "channelId"),
                                AgoraJson.JsonToStruct<RtcStats>(data, "stats"));
                        }
                    });
                    break;
                case "onNetworkQuality":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnNetworkQuality((string) AgoraJson.GetData<string>(data, "channelId"),
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (int) AgoraJson.GetData<int>(data, "txQuality"),
                                (int) AgoraJson.GetData<int>(data, "rxQuality"));
                        }
                    });
                    break;
                case "onRemoteVideoStats":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnRemoteVideoStats(
                                (string) AgoraJson.GetData<string>(data, "channelId"),
                                AgoraJson.JsonToStruct<RemoteVideoStats>(data, "stats"));
                        }
                    });
                    break;
                case "onRemoteAudioStats":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnRemoteAudioStats(
                                (string) AgoraJson.GetData<string>(data, "channelId"),
                                AgoraJson.JsonToStruct<RemoteAudioStats>(data, "stats"));
                        }
                    });
                    break;
                case "onRemoteAudioStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnRemoteAudioStateChanged(
                                (string) AgoraJson.GetData<string>(data, "channelId"),
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
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnAudioPublishStateChanged(
                                (string) AgoraJson.GetData<string>(data, "channelId"),
                                (STREAM_PUBLISH_STATE) AgoraJson.GetData<int>(data, "oldState"),
                                (STREAM_PUBLISH_STATE) AgoraJson.GetData<int>(data, "newState"),
                                (int) AgoraJson.GetData<int>(data, "elapseSinceLastState"));
                        }
                    });
                    break;
                case "onVideoPublishStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnVideoPublishStateChanged(
                                (string) AgoraJson.GetData<string>(data, "channelId"),
                                (STREAM_PUBLISH_STATE) AgoraJson.GetData<int>(data, "oldState"),
                                (STREAM_PUBLISH_STATE) AgoraJson.GetData<int>(data, "newState"),
                                (int) AgoraJson.GetData<int>(data, "elapseSinceLastState"));
                        }
                    });
                    break;
                case "onAudioSubscribeStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnAudioSubscribeStateChanged(
                                (string) AgoraJson.GetData<string>(data, "channelId"),
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
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnVideoSubscribeStateChanged(
                                (string) AgoraJson.GetData<string>(data, "channelId"),
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (STREAM_SUBSCRIBE_STATE) AgoraJson.GetData<int>(data, "oldState"),
                                (STREAM_SUBSCRIBE_STATE) AgoraJson.GetData<int>(data, "newState"),
                                (int) AgoraJson.GetData<int>(data, "elapseSinceLastState"));
                        }
                    });
                    break;
                case "onUserSuperResolutionEnabled":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnUserSuperResolutionEnabled(
                                (string) AgoraJson.GetData<string>(data, "channelId"),
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (bool) AgoraJson.GetData<bool>(data, "enabled"),
                                (SUPER_RESOLUTION_STATE_REASON) AgoraJson.GetData<int>(data, "reason"));
                        }
                    });
                    break;
                case "onActiveSpeaker":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnActiveSpeaker((string) AgoraJson.GetData<string>(data, "channelId"),
                                (uint) AgoraJson.GetData<uint>(data, "uid"));
                        }
                    });
                    break;
                case "onVideoSizeChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnVideoSizeChanged(
                                (string) AgoraJson.GetData<string>(data, "channelId"),
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (int) AgoraJson.GetData<int>(data, "width"),
                                (int) AgoraJson.GetData<int>(data, "height"),
                                (int) AgoraJson.GetData<int>(data, "rotation"));
                        }
                    });
                    break;
                case "onRemoteVideoStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnRemoteVideoStateChanged(
                                (string) AgoraJson.GetData<string>(data, "channelId"),
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (REMOTE_VIDEO_STATE) AgoraJson.GetData<int>(data, "state"),
                                (REMOTE_VIDEO_STATE_REASON) AgoraJson.GetData<int>(data, "reason"),
                                (int) AgoraJson.GetData<int>(data, "elapsed"));
                        }
                    });
                    break;
                case "onStreamMessageError":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnStreamMessageError(
                                (string) AgoraJson.GetData<string>(data, "channelId"),
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (int) AgoraJson.GetData<int>(data, "streamId"),
                                (int) AgoraJson.GetData<int>(data, "code"),
                                (int) AgoraJson.GetData<int>(data, "missed"),
                                (int) AgoraJson.GetData<int>(data, "cached"));
                        }
                    });
                    break;
                case "onChannelMediaRelayStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnChannelMediaRelayStateChanged(
                                (string) AgoraJson.GetData<string>(data, "channelId"),
                                (CHANNEL_MEDIA_RELAY_STATE) AgoraJson.GetData<int>(data, "state"),
                                (CHANNEL_MEDIA_RELAY_ERROR) AgoraJson.GetData<int>(data, "code"));
                        }
                    });
                    break;
                case "onChannelMediaRelayEvent":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnChannelMediaRelayEvent(
                                (string) AgoraJson.GetData<string>(data, "channelId"),
                                (CHANNEL_MEDIA_RELAY_EVENT) AgoraJson.GetData<int>(data, "code"));
                        }
                    });
                    break;
                case "onRtmpStreamingStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnRtmpStreamingStateChanged(
                                (string) AgoraJson.GetData<string>(data, "channelId"),
                                (string) AgoraJson.GetData<string>(data, "url"),
                                (RTMP_STREAM_PUBLISH_STATE) AgoraJson.GetData<int>(data, "state"),
                                (RTMP_STREAM_PUBLISH_ERROR) AgoraJson.GetData<int>(data, "errCode"));
                        }
                    });
                    break;
                case "onRtmpStreamingEvent":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnRtmpStreamingEvent(
                                (string) AgoraJson.GetData<string>(data, "channelId"),
                                (string) AgoraJson.GetData<string>(data, "url"),
                                (RTMP_STREAMING_EVENT) AgoraJson.GetData<int>(data, "eventCode"));
                        }
                    });
                    break;
                case "onTranscodingUpdated":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnTranscodingUpdated(
                                (string) AgoraJson.GetData<string>(data, "channelId"));
                        }
                    });
                    break;
                case "onStreamInjectedStatus":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnStreamInjectedStatus(
                                (string) AgoraJson.GetData<string>(data, "channelId"),
                                (string) AgoraJson.GetData<string>(data, "url"),
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (int) AgoraJson.GetData<int>(data, "status"));
                        }
                    });
                    break;
                case "onLocalPublishFallbackToAudioOnly":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnLocalPublishFallbackToAudioOnly(
                                (string) AgoraJson.GetData<string>(data, "channelId"),
                                (bool) AgoraJson.GetData<bool>(data, "isFallbackOrRecover"));
                        }
                    });
                    break;
                case "onRemoteSubscribeFallbackToAudioOnly":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnRemoteSubscribeFallbackToAudioOnly(
                                (string) AgoraJson.GetData<string>(data, "channelId"),
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (bool) AgoraJson.GetData<bool>(data, "isFallbackOrRecover"));
                        }
                    });
                    break;
                case "onConnectionStateChanged":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnConnectionStateChanged(
                                (string) AgoraJson.GetData<string>(data, "channelId"),
                                (CONNECTION_STATE_TYPE) AgoraJson.GetData<int>(data, "state"),
                                (CONNECTION_CHANGED_REASON_TYPE) AgoraJson.GetData<int>(data, "reason"));
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
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnStreamMessage((string) AgoraJson.GetData<string>(data, "channelId"),
                                (uint) AgoraJson.GetData<uint>(data, "uid"),
                                (int) AgoraJson.GetData<int>(data, "streamId"), byteData, length);
                        }
                    });
                    break;
                case "onReadyToSendMetadata":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        var metadata = new Metadata((uint) AgoraJson.GetData<uint>(data, "uid"),
                            (uint) AgoraJson.GetData<uint>(data, "size"), byteData,
                            (long) AgoraJson.GetData<long>(data, "timeStampMs"));
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnReadyToSendMetadata(metadata);
                        }
                    });
                    break;
                case "onMetadataReceived":
                    _callbackObject._CallbackQueue.EnQueue(() =>
                    {
                        var metadata = new Metadata((uint) AgoraJson.GetData<uint>(data, "uid"),
                            (uint) AgoraJson.GetData<uint>(data, "size"), byteData,
                            (long) AgoraJson.GetData<long>(data, "timeStampMs"));
                        if (_channelEventHandler != null)
                        {
                            _channelEventHandler.OnMetadataReceived(metadata);
                        }
                    });
                    break;
            }
        }
    }

    public sealed class AgoraRtcChannel : IAgoraRtcChannel, IDisposable
    {
        private bool _disposed;

        private readonly string _channelId;
        private IrisRtcChannelPtr _irisRtcChannel = IntPtr.Zero;

        private AgoraRtcEngine _rtcEngine;

        private RtcChannelEventHandlerNative _rtcChannelEventHandlerNative;

        private CharArrayAssistant _result;

        internal AgoraRtcChannel(AgoraRtcEngine rtcEngine, string channelId)
        {
            _result = new CharArrayAssistant();
            _rtcEngine = rtcEngine;
            _channelId = channelId;
            _irisRtcChannel = AgoraRtcNative.GetIrisRtcChannel(rtcEngine.GetNativeHandler());
            var param = new
            {
                channelId = _channelId
            };
            AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelCreateChannel,
                JsonMapper.ToJson(param), out _result);
        }

        [Obsolete(ObsoleteMethodWarning.CreateChannelWarning, true)]
        public static AgoraChannel CreateChannel(IRtcEngine rtcEngine, string channelId)
        {
            return rtcEngine.CreateChannel(channelId);
        }

        public override void InitEventHandler(IAgoraRtcChannelEventHandler channelEventHandler)
        {
            if (_rtcChannelEventHandlerNative != null)
                _rtcChannelEventHandlerNative.SetEventHandler(channelEventHandler);
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                if (_rtcChannelEventHandlerNative != null) _rtcChannelEventHandlerNative.Dispose();
                _rtcChannelEventHandlerNative = null;
            }

            Release();
            _disposed = true;
        }

        private void Release()
        {
            var param = new
            {
                channelId = _channelId
            };
            AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelRelease,
                JsonMapper.ToJson(param), out _result);
            _rtcEngine.ReleaseChannel(_channelId);
            _rtcEngine = null;
            _irisRtcChannel = IntPtr.Zero;
            _result = new CharArrayAssistant();
        }

        [Obsolete(ObsoleteMethodWarning.ReleaseChannelWarning, false)]
        public override int ReleaseChannel()
        {
            Dispose();
            return 0;
        }

        public override int JoinChannel(string token, string info, uint uid, ChannelMediaOptions options)
        {
            var param = new
            {
                channelId = _channelId,
                token,
                info,
                uid,
                options
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelJoinChannel,
                JsonMapper.ToJson(param), out _result);
        }

        public override int JoinChannelWithUserAccount(string token, string userAccount, ChannelMediaOptions options)
        {
            var param = new
            {
                channelId = _channelId,
                token,
                userAccount,
                options
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel,
                ApiTypeChannel.kChannelJoinChannelWithUserAccount, JsonMapper.ToJson(param),
                out _result);
        }

        public override int LeaveChannel()
        {
            var param = new
            {
                channelId = _channelId
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelLeaveChannel,
                JsonMapper.ToJson(param), out _result);
        }

        public override int Publish()
        {
            var param = new
            {
                channelId = _channelId
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelPublish,
                JsonMapper.ToJson(param), out _result);
        }

        public override int Unpublish()
        {
            var param = new
            {
                channelId = _channelId
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelUnPublish,
                JsonMapper.ToJson(param), out _result);
        }

        public override string ChannelId()
        {
            return _channelId;
        }

        public override string GetCallId()
        {
            var param = new
            {
                channelId = _channelId
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelGetCallId,
                JsonMapper.ToJson(param), out _result) != 0
                ? null
                : _result.Result;
        }

        public override int RenewToken(string token)
        {
            var param = new
            {
                channelId = _channelId,
                token
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelRenewToken,
                JsonMapper.ToJson(param), out _result);
        }

        [Obsolete(ObsoleteMethodWarning.SetEncryptionSecretWarning, false)]
        public override int SetEncryptionSecret(string secret)
        {
            var param = new
            {
                channelId = _channelId,
                secret
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelSetEncryptionSecret,
                JsonMapper.ToJson(param), out _result);
        }

        [Obsolete(ObsoleteMethodWarning.SetEncryptionModeWarning, false)]
        public override int SetEncryptionMode(string encryptionMode)
        {
            var param = new
            {
                channelId = _channelId,
                encryptionMode
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelSetEncryptionMode,
                JsonMapper.ToJson(param), out _result);
        }

        public override int EnableEncryption(bool enabled, EncryptionConfig config)
        {
            var param = new
            {
                channelId = _channelId,
                enabled,
                config
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelEnableEncryption,
                JsonMapper.ToJson(param), out _result);
        }

        public override int RegisterPacketObserver(IPacketObserver observer)
        {
            var param = new
            {
                channelId = _channelId,
                observer
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelRegisterPacketObserver,
                JsonMapper.ToJson(param), out _result);
        }

        public override int RegisterMediaMetadataObserver(METADATA_TYPE type)
        {
            var param = new
            {
                channelId = _channelId,
                type
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel,
                ApiTypeChannel.kChannelRegisterMediaMetadataObserver, JsonMapper.ToJson(param),
                out _result);
        }

        public override int UnRegisterMediaMetadataObserver(METADATA_TYPE type)
        {
            var param = new
            {
                channelId = _channelId,
                type
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel,
                ApiTypeChannel.kChannelUnRegisterMediaMetadataObserver,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetMaxMetadataSize(int size)
        {
            var param = new
            {
                channelId = _channelId,
                size
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelSetMaxMetadataSize,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SendMetadata(Metadata metadata)
        {
            var param = new
            {
                channelId = _channelId,
                metadata
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelSendMetadata,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetClientRole(CLIENT_ROLE_TYPE role)
        {
            var param = new
            {
                channelId = _channelId,
                role
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelSetClientRole,
                JsonMapper.ToJson(param), out _result);
            ;
        }

        public override int SetClientRole(CLIENT_ROLE_TYPE role, ClientRoleOptions options)
        {
            var param = new
            {
                channelId = _channelId,
                role,
                options
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelSetClientRole,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetRemoteUserPriority(uint uid, PRIORITY_TYPE userPriority)
        {
            var param = new
            {
                channelId = _channelId,
                uid,
                userPriority
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelSetRemoteUserPriority,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetRemoteVoicePosition(uint uid, double pan, double gain)
        {
            var param = new
            {
                channelId = _channelId,
                uid,
                pan,
                gain
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelSetRemoteVoicePosition,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetRemoteRenderMode(uint userId, RENDER_MODE_TYPE renderMode,
            VIDEO_MIRROR_MODE_TYPE mirrorMode)
        {
            var param = new
            {
                channelId = _channelId,
                userId,
                renderMode,
                mirrorMode
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelSetRemoteRenderMode,
                JsonMapper.ToJson(param), out _result);
        }

        [Obsolete(ObsoleteMethodWarning.GeneralWarning, false)]
        public override int SetDefaultMuteAllRemoteAudioStreams(bool mute)
        {
            var param = new
            {
                channelId = _channelId,
                mute
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel,
                ApiTypeChannel.kChannelSetDefaultMuteAllRemoteAudioStreams,
                JsonMapper.ToJson(param), out _result);
        }

        [Obsolete(ObsoleteMethodWarning.GeneralWarning, false)]
        public override int SetDefaultMuteAllRemoteVideoStreams(bool mute)
        {
            var param = new
            {
                channelId = _channelId,
                mute
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel,
                ApiTypeChannel.kChannelSetDefaultMuteAllRemoteVideoStreams,
                JsonMapper.ToJson(param), out _result);
        }

        public override int MuteAllRemoteAudioStreams(bool mute)
        {
            var param = new
            {
                channelId = _channelId,
                mute
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel,
                ApiTypeChannel.kChannelMuteAllRemoteAudioStreams, JsonMapper.ToJson(param),
                out _result);
        }

        public override int AdjustUserPlaybackSignalVolume(uint uid, int volume)
        {
            var param = new
            {
                channelId = _channelId,
                uid,
                volume
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel,
                ApiTypeChannel.kChannelAdjustUserPlaybackSignalVolume, JsonMapper.ToJson(param),
                out _result);
        }

        public override int MuteRemoteAudioStream(uint userId, bool mute)
        {
            var param = new
            {
                channelId = _channelId,
                userId,
                mute
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelMuteRemoteAudioStream,
                JsonMapper.ToJson(param), out _result);
        }

        public override int MuteAllRemoteVideoStreams(bool mute)
        {
            var param = new
            {
                channelId = _channelId,
                mute
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel,
                ApiTypeChannel.kChannelMuteAllRemoteVideoStreams, JsonMapper.ToJson(param),
                out _result);
        }

        public override int MuteRemoteVideoStream(uint userId, bool mute)
        {
            var param = new
            {
                channelId = _channelId,
                userId,
                mute
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelMuteRemoteVideoStream,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetRemoteVideoStreamType(uint userId, REMOTE_VIDEO_STREAM_TYPE streamType)
        {
            var param = new
            {
                channelId = _channelId,
                userId,
                streamType
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel,
                ApiTypeChannel.kChannelSetRemoteVideoStreamType, JsonMapper.ToJson(param),
                out _result);
        }

        public override int SetRemoteDefaultVideoStreamType(REMOTE_VIDEO_STREAM_TYPE streamType)
        {
            var param = new
            {
                channelId = _channelId
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel,
                ApiTypeChannel.kChannelSetRemoteDefaultVideoStreamType,
                JsonMapper.ToJson(param), out _result);
        }

        [Obsolete(ObsoleteMethodWarning.GeneralWarning, false)]
        public override int CreateDataStream(bool reliable, bool ordered)
        {
            var param = new
            {
                channelId = _channelId,
                reliable,
                ordered
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelCreateDataStream,
                JsonMapper.ToJson(param), out _result);
        }

        public override int CreateDataStream(DataStreamConfig config)
        {
            var param = new
            {
                channelId = _channelId,
                config
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelCreateDataStream,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SendStreamMessage(int streamId, byte[] data)
        {
            var param = new
            {
                channelId = _channelId,
                streamId,
                length = data.Length
            };
            return AgoraRtcNative.CallIrisRtcChannelApiWithBuffer(_irisRtcChannel,
                ApiTypeChannel.kChannelSendStreamMessage,
                JsonMapper.ToJson(param), data, out _result);
        }

        public override int AddPublishStreamUrl(string url, bool transcodingEnabled)
        {
            var param = new
            {
                channelId = _channelId,
                url,
                transcodingEnabled
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelAddPublishStreamUrl,
                JsonMapper.ToJson(param), out _result);
        }

        public override int RemovePublishStreamUrl(string url)
        {
            var param = new
            {
                channelId = _channelId,
                url
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelRemovePublishStreamUrl,
                JsonMapper.ToJson(param), out _result);
        }

        public override int SetLiveTranscoding(LiveTranscoding transcoding)
        {
            var param = new
            {
                channelId = _channelId,
                transcoding
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelSetLiveTranscoding,
                JsonMapper.ToJson(param), out _result);
        }

        public override int AddInjectStreamUrl(string url, InjectStreamConfig config)
        {
            var param = new
            {
                channelId = _channelId,
                url,
                config
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelAddInjectStreamUrl,
                JsonMapper.ToJson(param), out _result);
        }

        public override int RemoveInjectStreamUrl(string url)
        {
            var param = new
            {
                channelId = _channelId,
                url
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelRemoveInjectStreamUrl,
                JsonMapper.ToJson(param), out _result);
        }

        public override int StartChannelMediaRelay(ChannelMediaRelayConfiguration configuration)
        {
            var param = new
            {
                channelId = _channelId,
                configuration
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelStartChannelMediaRelay,
                JsonMapper.ToJson(param), out _result);
        }

        public override int UpdateChannelMediaRelay(ChannelMediaRelayConfiguration configuration)
        {
            var param = new
            {
                channelId = _channelId,
                configuration
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelUpdateChannelMediaRelay,
                JsonMapper.ToJson(param), out _result);
        }

        public override int StopChannelMediaRelay()
        {
            var param = new
            {
                channelId = _channelId
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel, ApiTypeChannel.kChannelStopChannelMediaRelay,
                JsonMapper.ToJson(param), out _result);
        }

        public override CONNECTION_STATE_TYPE GetConnectionState()
        {
            var param = new
            {
                channelId = _channelId
            };
            return (CONNECTION_STATE_TYPE) AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel,
                ApiTypeChannel.kChannelGetConnectionState, JsonMapper.ToJson(param),
                out _result);
        }

        public override int EnableRemoteSuperResolution(uint userId, bool enable)
        {
            var param = new
            {
                channelId = _channelId,
                userId,
                enable
            };
            return AgoraRtcNative.CallIrisRtcChannelApi(_irisRtcChannel,
                ApiTypeChannel.kChannelEnableRemoteSuperResolution, JsonMapper.ToJson(param),
                out _result);
        }

        ~AgoraRtcChannel()
        {
            Dispose(false);
        }
    }
}