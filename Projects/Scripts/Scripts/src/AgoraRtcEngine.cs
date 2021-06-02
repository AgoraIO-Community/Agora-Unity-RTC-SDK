//  AgoraRtcEngine.cs
//
//  Created by Yiqing Huang on June 2, 2021.
//  Modified by Yiqing Huang on June 2, 2021.
//
//  Copyright © 2021 Agora. All rights reserved.
//

using System;
using System.Text;
using LitJson;

namespace agora_gaming_rtc
{
    using view_t = UInt64;
    using IrisRtcEnginePtr = IntPtr;

    public class AgoraRtcEngine : IAgoraRtcEngine
    {
        private bool _disposed = false;
        private static AgoraRtcEngine _instance;
        private IrisRtcEnginePtr _irisRtcEngine = IntPtr.Zero;
        private CharArrayAssistant _result;


        public override int Initialize(RtcEngineContext context)
        {
            throw new NotImplementedException();
        }

        public override void InitEventHandler(IRtcEngineEventHandlerBase eventHandlerBase)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
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

        public override int SetChannelProfile(CHANNEL_PROFILE_TYPE profile)
        {
            var param = new
            {
                profile
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetChannelProfile,
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

        [Obsolete("The method is deprecated.", false)]
        public override int JoinChannel(string channelId, string info = "", uint uid = 0)
        {
            return JoinChannel(null, channelId, info, uid);
        }

        [Obsolete("This method is deprecated. Please call JoinChannel instead.", false)]
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
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineRegisterLocalUserAccount,
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
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineJoinChannelWithUserAccount,
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
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineJoinChannelWithUserAccount,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int GetUserInfoByUserAccount(string userAccount, out UserInfo userInfo)
        {
            var param = new
            {
                userAccount
            };
            var ret = AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineGetUserInfoByUserAccount,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
            userInfo = _result.Result.Length == 0 ? new UserInfo() : AgoraJson.JsonToStruct<UserInfo>(_result.Result);
            return ret;
        }

        [Obsolete("The method is deprecated.", false)]
        public override UserInfo GetUserInfoByUserAccount(string userAccount)
        {
            var param = new
            {
                userAccount
            };
            AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineGetUserInfoByUserAccount,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
            return _result.Result.Length == 0 ? new UserInfo() : AgoraJson.JsonToStruct<UserInfo>(_result.Result);
        }

        public override int GetUserInfoByUid(uint uid, out UserInfo userInfo)
        {
            var param = new
            {
                uid
            };
            var ret = AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineGetUserInfoByUid,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
            userInfo = _result.Result.Length == 0 ? new UserInfo() : AgoraJson.JsonToStruct<UserInfo>(_result.Result);
            return ret;
        }

        [Obsolete("The method is deprecated.", false)]
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

        [Obsolete("The method is deprecated.", false)]
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
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetRemoteUserPriority,
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
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineMuteLocalAudioStream,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int MuteAllRemoteAudioStreams(bool mute)
        {
            var param = new
            {
                mute
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineMuteAllRemoteAudioStreams,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        [Obsolete("The method is deprecated.", false)]
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
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineMuteRemoteAudioStream,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int MuteLocalVideoStream(bool mute)
        {
            var param = new
            {
                mute
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineMuteLocalVideoStream,
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
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineMuteAllRemoteVideoStreams,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        [Obsolete("The method is deprecated.", false)]
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
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineMuteRemoteVideoStream,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int SetRemoteVideoStreamType(uint userId, REMOTE_VIDEO_STREAM_TYPE streamType)
        {
            var param = new
            {
                userId,
                streamType
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetRemoteVideoStreamType,
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
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineEnableAudioVolumeIndication,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        [Obsolete("The method is deprecated.", false)]
        public override int StartAudioRecording(string filePath, AUDIO_RECORDING_QUALITY_TYPE quality)
        {
            var param = new
            {
                filePath,
                quality
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineStartAudioRecording,
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
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineStartAudioRecording,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int StopAudioRecording()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineStopAudioRecording,
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
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineResumeAudioMixing,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        [Obsolete("The method is deprecated.", false)]
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
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineAdjustAudioMixingVolume,
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
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineGetAudioMixingPlayoutVolume,
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
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineGetAudioMixingPublishVolume,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int GetAudioMixingDuration()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineGetAudioMixingDuration,
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
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetAudioMixingPosition,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int SetAudioMixingPitch(int pitch)
        {
            var param = new
            {
                pitch
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetAudioMixingPitch,
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
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetVolumeOfEffect,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int EnableFaceDetection(bool enable)
        {
            var param = new
            {
                enable
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineEnableFaceDetection,
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
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineEnableDeepLearningDenoise,
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
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetRemoteVoicePosition,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int SetLocalVoicePitch(double pitch)
        {
            var param = new
            {
                pitch
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetLocalVoicePitch,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int SetLocalVoiceEqualization(AUDIO_EQUALIZATION_BAND_FREQUENCY bandFrequency, int bandGain)
        {
            var param = new
            {
                bandFrequency,
                bandGain
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetLocalVoiceEqualization,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int SetLocalVoiceReverb(AUDIO_REVERB_TYPE reverbKey, int value)
        {
            var param = new
            {
                reverbKey,
                value
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetLocalVoiceReverb,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        [Obsolete("The method is deprecated.", false)]
        public override int SetLocalVoiceChanger(VOICE_CHANGER_PRESET voiceChanger)
        {
            var param = new
            {
                voiceChanger
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetLocalVoiceChanger,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        [Obsolete("This method is deprecated. Please call SetAudioEffectPresent or SetVoiceBeautifierPresent instead.",
            false)]
        public override int SetLocalVoiceReverbPreset(AUDIO_REVERB_PRESET reverbPreset)
        {
            var param = new
            {
                reverbPreset
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetLocalVoiceReverbPreset,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int SetVoiceBeautifierPreset(VOICE_BEAUTIFIER_PRESET preset)
        {
            var param = new
            {
                preset
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetVoiceBeautifierPreset,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int SetAudioEffectPreset(AUDIO_EFFECT_PRESET preset)
        {
            var param = new
            {
                preset
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetAudioEffectPreset,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int SetVoiceConversionPreset(VOICE_CONVERSION_PRESET preset)
        {
            var param = new
            {
                preset
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetVoiceConversionPreset,
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
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetAudioEffectParameters,
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

        [Obsolete("This method is deprecated. Please use logConfig in the initialize method instead.", false)]
        public override int SetLogFile(string filePath)
        {
            var param = new
            {
                filePath
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetLogFile,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        [Obsolete("This method is deprecated. Please use logConfig in the initialize method instead.", false)]
        public override int SetLogFilter(uint filter)
        {
            var param = new
            {
                filter
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetLogFilter,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        [Obsolete("This method is deprecated. Please use logConfig in the initialize method instead.", false)]
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

        [Obsolete("The method is deprecated.", false)]
        public override int SetLocalRenderMode(RENDER_MODE_TYPE renderMode)
        {
            var param = new
            {
                renderMode
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetLocalRenderMode,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int SetLocalRenderMode(RENDER_MODE_TYPE renderMode, VIDEO_MIRROR_MODE_TYPE mirrorMode)
        {
            var param = new
            {
                renderMode,
                mirrorMode
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetLocalRenderMode,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        [Obsolete("The method is deprecated.", false)]
        public override int SetRemoteRenderMode(uint userId, RENDER_MODE_TYPE renderMode)
        {
            var param = new
            {
                userId,
                renderMode
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetRemoteRenderMode,
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
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetRemoteRenderMode,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        [Obsolete("This method is deprecated. Please call SetupLocalVideo or SetLocalRenderMode instead.", false)]
        public override int SetLocalVideoMirrorMode(VIDEO_MIRROR_MODE_TYPE mirrorMode)
        {
            var param = new
            {
                mirrorMode
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetLocalVideoMirrorMode,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int EnableDualStreamMode(bool enabled)
        {
            var param = new
            {
                enabled
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineEnableDualStreamMode,
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
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetExternalAudioSource,
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
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetExternalAudioSink,
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
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineAdjustRecordingSignalVolume,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int AdjustPlaybackSignalVolume(int volume)
        {
            var param = new
            {
                volume
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineAdjustPlaybackSignalVolume,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        [Obsolete("The method is deprecated.", false)]
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

        [Obsolete("The method is deprecated.", false)]
        public override int SetVideoQualityParameters(bool preferFrameRateOverImageQuality)
        {
            var param = new
            {
                preferFrameRateOverImageQuality
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetVideoQualityParameters,
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
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetEnableSpeakerPhone,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int EnableInEarMonitoring(bool enabled)
        {
            var param = new
            {
                enabled
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineEnableInEarMonitoring,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int SetInEarMonitoringVolume(int volume)
        {
            var param = new
            {
                volume
            };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetInEarMonitoringVolume,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override bool IsSpeakerphoneEnabled()
        {
            var param = new { };
            var ret = AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineIsSpeakerPhoneEnabled,
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
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineEnableLoopBackRecording,
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
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineSetScreenCaptureContentHint,
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
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineUpdateScreenCaptureRegion,
                Encoding.UTF8.GetBytes(JsonMapper.ToJson(param)), out _result);
        }

        public override int StopScreenCapture()
        {
            var param = new { };
            return AgoraRtcNative.CallIrisRtcEngineApi(_irisRtcEngine, ApiTypeEngine.kEngineStopScreenCapture,
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
            throw new NotImplementedException();
        }

        public override int DisableLastmileTest()
        {
            throw new NotImplementedException();
        }

        public override int StartLastmileProbeTest(LastmileProbeConfig config)
        {
            throw new NotImplementedException();
        }

        public override int StopLastmileProbeTest()
        {
            throw new NotImplementedException();
        }

        public override int GetErrorDescription(int code)
        {
            throw new NotImplementedException();
        }

        [Obsolete("This method is deprecated. Please call EnableEncryption instead.", false)]
        public override int SetEncryptionSecret(string secret)
        {
            throw new NotImplementedException();
        }

        [Obsolete("This method is deprecated. Please call EnableEncryption instead.", false)]
        public override int SetEncryptionMode(string encryptionMode)
        {
            throw new NotImplementedException();
        }

        public override int EnableEncryption(bool enabled, EncryptionConfig config)
        {
            throw new NotImplementedException();
        }

        public override int RegisterPacketObserver(IPacketObserver observer)
        {
            throw new NotImplementedException();
        }

        [Obsolete("The method is deprecated.", false)]
        public override int CreateDataStream(bool reliable, bool ordered)
        {
            throw new NotImplementedException();
        }

        public override int CreateDataStream(DataStreamConfig config)
        {
            throw new NotImplementedException();
        }

        public override int SendStreamMessage(int streamId, byte[] data)
        {
            throw new NotImplementedException();
        }

        public override int AddPublishStreamUrl(string url, bool transcodingEnabled)
        {
            throw new NotImplementedException();
        }

        public override int RemovePublishStreamUrl(string url)
        {
            throw new NotImplementedException();
        }

        public override int SetLiveTranscoding(LiveTranscoding transcoding)
        {
            throw new NotImplementedException();
        }

        [Obsolete("The method is deprecated.", false)]
        public override int AddVideoWatermark(RtcImage watermark)
        {
            throw new NotImplementedException();
        }

        public override int AddVideoWatermark(string watermarkUrl, WatermarkOptions options)
        {
            throw new NotImplementedException();
        }

        public override int ClearVideoWatermarks()
        {
            throw new NotImplementedException();
        }

        public override int SetBeautyEffectOptions(bool enabled, BeautyOptions options)
        {
            throw new NotImplementedException();
        }

        public override int AddInjectStreamUrl(string url, InjectStreamConfig config)
        {
            throw new NotImplementedException();
        }

        public override int StartChannelMediaRelay(ChannelMediaRelayConfiguration configuration)
        {
            throw new NotImplementedException();
        }

        public override int UpdateChannelMediaRelay(ChannelMediaRelayConfiguration configuration)
        {
            throw new NotImplementedException();
        }

        public override int StopChannelMediaRelay()
        {
            throw new NotImplementedException();
        }

        public override int RemoveInjectStreamUrl(string url)
        {
            throw new NotImplementedException();
        }

        public override int SendCustomReportMessage(string id, string category, string events, string label, int value)
        {
            throw new NotImplementedException();
        }

        public override CONNECTION_STATE_TYPE GetConnectionState()
        {
            throw new NotImplementedException();
        }

        public override int RegisterMediaMetadataObserver(METADATA_TYPE type)
        {
            throw new NotImplementedException();
        }

        public override int UnRegisterMediaMetadataObserver(METADATA_TYPE type)
        {
            throw new NotImplementedException();
        }

        public override int EnableRemoteSuperResolution(uint userId, bool enable)
        {
            throw new NotImplementedException();
        }

        public override int SetParameters(string parameters)
        {
            throw new NotImplementedException();
        }

        public override int SetMaxMetadataSize(int size)
        {
            throw new NotImplementedException();
        }

        public override int SendMetadata(Metadata metadata)
        {
            throw new NotImplementedException();
        }

        public override int PushAudioFrame(MEDIA_SOURCE_TYPE type, AudioFrame frame, bool wrap)
        {
            throw new NotImplementedException();
        }

        public override int PushAudioFrame(AudioFrame frame)
        {
            throw new NotImplementedException();
        }

        public override int PullAudioFrame(ref AudioFrame frame)
        {
            throw new NotImplementedException();
        }

        public override int SetExternalVideoSource(bool enable, bool useTexture = false)
        {
            throw new NotImplementedException();
        }

        public override int PushVideoFrame(ExternalVideoFrame frame)
        {
            throw new NotImplementedException();
        }
    }
}