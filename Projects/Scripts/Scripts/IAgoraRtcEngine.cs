//  IAgoraRtcEngine.cs
//
//  Created by Yiqing Huang on June 1, 2021.
//  Modified by Yiqing Huang on June 2, 2021.
//
//  Copyright © 2021 Agora. All rights reserved.
//

using System;

namespace agora_gaming_rtc
{
    using view_t = UInt64;

    public abstract class IAgoraRtcEngine
    {
        public abstract int Initialize(RtcEngineContext context);
        public abstract void InitEventHandler(IRtcEngineEventHandlerBase eventHandlerBase);
        public abstract void Dispose();
        public abstract IAudioEffectManager GetAudioEffectManager();
        public abstract IAudioRecordingDeviceManager GetAudioRecordingDeviceManager();
        public abstract IAudioPlaybackDeviceManager GetAudioPlaybackDeviceManager();
        public abstract IVideoDeviceManager GetVideoDeviceManager();
        public abstract IAudioRawDataManager GetAudioRawDataManager();
        public abstract IVideoRawDataManager GetVideoRawDataManager();
        public abstract IVideoRender GetVideoRender();
        public abstract IAgoraRtcChannel CreateChannel(string channelId);
        public abstract int SetChannelProfile(CHANNEL_PROFILE_TYPE profile);
        public abstract int SetClientRole(CLIENT_ROLE_TYPE role);
        public abstract int SetClientRole(CLIENT_ROLE_TYPE role, ClientRoleOptions options);
        public abstract int JoinChannel(string token, string channelId, string info = "", uint uid = 0);

        public abstract int JoinChannel(string token, string channelId, string info, uint uid,
            ChannelMediaOptions options);

        [Obsolete("This method is deprecated.", false)]
        public abstract int JoinChannel(string channelId, string info = "", uint uid = 0);

        [Obsolete("This method is deprecated. Please call JoinChannel instead.", false)]
        public abstract int JoinChannelByKey(string token, string channelId, string info = "", uint uid = 0);

        public abstract int SwitchChannel(string token, string channelId);
        public abstract int SwitchChannel(string token, string channelId, ChannelMediaOptions options);
        public abstract int LeaveChannel();
        public abstract int RenewToken(string token);
        public abstract int RegisterLocalUserAccount(string appId, string userAccount);
        public abstract int JoinChannelWithUserAccount(string token, string channelId, string userAccount);

        public abstract int JoinChannelWithUserAccount(string token, string channelId, string userAccount,
            ChannelMediaOptions options);

        public abstract int GetUserInfoByUserAccount(string userAccount, out UserInfo userInfo);

        [Obsolete("This method is deprecated.", false)]
        public abstract UserInfo GetUserInfoByUserAccount(string userAccount);

        public abstract int GetUserInfoByUid(uint uid, out UserInfo userInfo);

        [Obsolete("This method is deprecated.", false)]
        public abstract UserInfo GetUserInfoByUid(uint uid);

        [Obsolete("This method is deprecated.", false)]
        public abstract int StartEchoTest();

        public abstract int StartEchoTest(int intervalInSeconds);
        public abstract int StopEchoTest();
        public abstract int SetCloudProxy(CLOUD_PROXY_TYPE proxyType);
        public abstract int EnableVideo();
        public abstract int DisableVideo();
        public abstract int EnableVideoObserver();
        public abstract int DisableVideoObserver();

        [Obsolete("This method is deprecated. Please call SetVideoEncoderConfiguration instead.", false)]
        public abstract int SetVideoProfile(VIDEO_PROFILE_TYPE profile, bool swapWidthAndHeight = false);

        public abstract int SetVideoEncoderConfiguration(VideoEncoderConfiguration config);
        public abstract int SetCameraCapturerConfiguration(CameraCapturerConfiguration config);
        public abstract int SetupLocalVideo(VideoCanvas canvas);
        public abstract int SetupRemoteVideo(VideoCanvas canvas);
        public abstract int StartPreview();
        public abstract int SetRemoteUserPriority(uint uid, PRIORITY_TYPE userPriority);
        public abstract int StopPreview();
        public abstract int EnableAudio();
        public abstract int EnableLocalAudio(bool enabled);
        public abstract int DisableAudio();
        public abstract int SetAudioProfile(AUDIO_PROFILE_TYPE profile, AUDIO_SCENARIO_TYPE scenario);
        public abstract int MuteLocalAudioStream(bool mute);
        public abstract int MuteAllRemoteAudioStreams(bool mute);

        [Obsolete("This method is deprecated.", false)]
        public abstract int SetDefaultMuteAllRemoteAudioStreams(bool mute);

        public abstract int AdjustUserPlaybackSignalVolume(uint uid, int volume);
        public abstract int MuteRemoteAudioStream(uint userId, bool mute);
        public abstract int MuteLocalVideoStream(bool mute);
        public abstract int EnableLocalVideo(bool enabled);
        public abstract int MuteAllRemoteVideoStreams(bool mute);

        [Obsolete("This method is deprecated.", false)]
        public abstract int SetDefaultMuteAllRemoteVideoStreams(bool mute);

        public abstract int MuteRemoteVideoStream(uint userId, bool mute);
        public abstract int SetRemoteVideoStreamType(uint userId, REMOTE_VIDEO_STREAM_TYPE streamType);
        public abstract int SetRemoteDefaultVideoStreamType(REMOTE_VIDEO_STREAM_TYPE streamType);
        public abstract int EnableAudioVolumeIndication(int interval, int smooth, bool reportVad = false);

        [Obsolete("This method is deprecated.", false)]
        public abstract int StartAudioRecording(string filePath, AUDIO_RECORDING_QUALITY_TYPE quality);

        public abstract int StartAudioRecording(string filePath, int sampleRate, AUDIO_RECORDING_QUALITY_TYPE quality);
        public abstract int StopAudioRecording();
        public abstract int StartAudioMixing(string filePath, bool loopback, bool replace, int cycle);
        public abstract int StopAudioMixing();
        public abstract int PauseAudioMixing();
        public abstract int ResumeAudioMixing();

        [Obsolete("This method is deprecated.", false)]
        public abstract int SetHighQualityAudioParameters(bool fullband, bool stereo, bool fullBitrate);

        public abstract int AdjustAudioMixingVolume(int volume);
        public abstract int AdjustAudioMixingPlayoutVolume(int volume);
        public abstract int GetAudioMixingPlayoutVolume();
        public abstract int AdjustAudioMixingPublishVolume(int volume);
        public abstract int GetAudioMixingPublishVolume();
        public abstract int GetAudioMixingDuration();
        public abstract int GetAudioMixingCurrentPosition();
        public abstract int SetAudioMixingPosition(int pos);
        public abstract int SetAudioMixingPitch(int pitch);
        public abstract int GetEffectsVolume();
        public abstract int SetEffectsVolume(int volume);
        public abstract int SetVolumeOfEffect(int soundId, int volume);
        public abstract int EnableFaceDetection(bool enable);

        public abstract int PlayEffect(int soundId, string filePath, int loopCount, double pitch, double pan, int gain,
            bool publish);

        public abstract int StopEffect(int soundId);
        public abstract int StopAllEffects();
        public abstract int PreloadEffect(int soundId, string filePath);
        public abstract int UnloadEffect(int soundId);
        public abstract int PauseEffect(int soundId);
        public abstract int PauseAllEffects();
        public abstract int ResumeEffect(int soundId);
        public abstract int ResumeAllEffects();
        public abstract int EnableDeepLearningDenoise(bool enable);
        public abstract int EnableSoundPositionIndication(bool enabled);
        public abstract int SetRemoteVoicePosition(uint uid, double pan, double gain);
        public abstract int SetLocalVoicePitch(double pitch);
        public abstract int SetLocalVoiceEqualization(AUDIO_EQUALIZATION_BAND_FREQUENCY bandFrequency, int bandGain);
        public abstract int SetLocalVoiceReverb(AUDIO_REVERB_TYPE reverbKey, int value);

        [Obsolete("This method is deprecated.", false)]
        public abstract int SetLocalVoiceChanger(VOICE_CHANGER_PRESET voiceChanger);

        [Obsolete("This method is deprecated. Please call SetAudioEffectPresent or SetVoiceBeautifierPresent instead.",
            false)]
        public abstract int SetLocalVoiceReverbPreset(AUDIO_REVERB_PRESET reverbPreset);

        public abstract int SetVoiceBeautifierPreset(VOICE_BEAUTIFIER_PRESET preset);
        public abstract int SetAudioEffectPreset(AUDIO_EFFECT_PRESET preset);
        public abstract int SetVoiceConversionPreset(VOICE_CONVERSION_PRESET preset);
        public abstract int SetAudioEffectParameters(AUDIO_EFFECT_PRESET preset, int param1, int param2);
        public abstract int SetVoiceBeautifierParameters(VOICE_BEAUTIFIER_PRESET preset, int param1, int param2);

        [Obsolete("This method is deprecated. Please use logConfig in the initialize method instead.", false)]
        public abstract int SetLogFile(string filePath);

        [Obsolete("This method is deprecated. Please use logConfig in the initialize method instead.", false)]
        public abstract int SetLogFilter(uint filter);

        [Obsolete("This method is deprecated. Please use logConfig in the Initialize method instead.", false)]
        public abstract int SetLogFileSize(uint fileSizeInKBytes);

        public abstract string UploadLogFile();

        [Obsolete("This method is deprecated.", false)]
        public abstract int SetLocalRenderMode(RENDER_MODE_TYPE renderMode);

        public abstract int SetLocalRenderMode(RENDER_MODE_TYPE renderMode, VIDEO_MIRROR_MODE_TYPE mirrorMode);

        [Obsolete("This method is deprecated.", false)]
        public abstract int SetRemoteRenderMode(uint userId, RENDER_MODE_TYPE renderMode);

        public abstract int SetRemoteRenderMode(uint userId, RENDER_MODE_TYPE renderMode,
            VIDEO_MIRROR_MODE_TYPE mirrorMode);

        [Obsolete("This method is deprecated. Please call SetupLocalVideo or SetLocalRenderMode instead.", false)]
        public abstract int SetLocalVideoMirrorMode(VIDEO_MIRROR_MODE_TYPE mirrorMode);

        public abstract int EnableDualStreamMode(bool enabled);
        public abstract int SetExternalAudioSource(bool enabled, int sampleRate, int channels);
        public abstract int SetExternalAudioSink(bool enabled, int sampleRate, int channels);

        public abstract int SetRecordingAudioFrameParameters(int sampleRate, int channel,
            RAW_AUDIO_FRAME_OP_MODE_TYPE mode, int samplesPerCall);

        public abstract int SetPlaybackAudioFrameParameters(int sampleRate, int channel,
            RAW_AUDIO_FRAME_OP_MODE_TYPE mode, int samplesPerCall);

        public abstract int SetMixedAudioFrameParameters(int sampleRate, int samplesPerCall);
        public abstract int AdjustRecordingSignalVolume(int volume);
        public abstract int AdjustPlaybackSignalVolume(int volume);

        [Obsolete("This method is deprecated.", false)]
        public abstract int EnableWebSdkInteroperability(bool enabled);

        [Obsolete("This method is deprecated.", false)]
        public abstract int SetVideoQualityParameters(bool preferFrameRateOverImageQuality);

        public abstract int SetLocalPublishFallbackOption(STREAM_FALLBACK_OPTIONS option);
        public abstract int SetRemoteSubscribeFallbackOption(STREAM_FALLBACK_OPTIONS option);
        public abstract int SwitchCamera();
        public abstract int SetDefaultAudioRouteToSpeakerphone(bool defaultToSpeaker);
        public abstract int SetEnableSpeakerphone(bool speakerOn);
        public abstract int EnableInEarMonitoring(bool enabled);
        public abstract int SetInEarMonitoringVolume(int volume);
        public abstract bool IsSpeakerphoneEnabled();
        public abstract int SetAudioSessionOperationRestriction(AUDIO_SESSION_OPERATION_RESTRICTION restriction);
        public abstract int EnableLoopbackRecording(bool enabled, string deviceName);

        public abstract int StartScreenCaptureByDisplayId(uint displayId, Rectangle regionRect,
            ScreenCaptureParameters captureParams);

        public abstract int StartScreenCaptureByScreenRect(Rectangle screenRect, Rectangle regionRect,
            ScreenCaptureParameters captureParams);

        public abstract int StartScreenCaptureByWindowId(view_t windowId, Rectangle regionRect,
            ScreenCaptureParameters captureParams);

        public abstract int SetScreenCaptureContentHint(VideoContentHint contentHint);
        public abstract int UpdateScreenCaptureParameters(ScreenCaptureParameters captureParams);
        public abstract int UpdateScreenCaptureRegion(Rectangle regionRect);
        public abstract int StopScreenCapture();
        public abstract string GetCallId();
        public abstract int Rate(string callId, int rating, string description = "");
        public abstract int Complain(string callId, string description = "");
        public abstract string GetVersion();
        public abstract int EnableLastmileTest();
        public abstract int DisableLastmileTest();
        public abstract int StartLastmileProbeTest(LastmileProbeConfig config);
        public abstract int StopLastmileProbeTest();
        public abstract int GetErrorDescription(int code);

        [Obsolete("This method is deprecated. Please call EnableEncryption instead.", false)]
        public abstract int SetEncryptionSecret(string secret);

        [Obsolete("This method is deprecated. Please call EnableEncryption instead.", false)]
        public abstract int SetEncryptionMode(string encryptionMode);

        public abstract int EnableEncryption(bool enabled, EncryptionConfig config);
        public abstract int RegisterPacketObserver(IPacketObserver observer);

        [Obsolete("This method is deprecated.", false)]
        public abstract int CreateDataStream(bool reliable, bool ordered);

        public abstract int CreateDataStream(DataStreamConfig config);
        public abstract int SendStreamMessage(int streamId, byte[] data);
        public abstract int AddPublishStreamUrl(string url, bool transcodingEnabled);
        public abstract int RemovePublishStreamUrl(string url);
        public abstract int SetLiveTranscoding(LiveTranscoding transcoding);

        [Obsolete("This method is deprecated.", false)]
        public abstract int AddVideoWatermark(RtcImage watermark);

        public abstract int AddVideoWatermark(string watermarkUrl, WatermarkOptions options);
        public abstract int ClearVideoWatermarks();
        public abstract int SetBeautyEffectOptions(bool enabled, BeautyOptions options);
        public abstract int AddInjectStreamUrl(string url, InjectStreamConfig config);
        public abstract int StartChannelMediaRelay(ChannelMediaRelayConfiguration configuration);
        public abstract int UpdateChannelMediaRelay(ChannelMediaRelayConfiguration configuration);
        public abstract int StopChannelMediaRelay();
        public abstract int RemoveInjectStreamUrl(string url);
        public abstract int SendCustomReportMessage(string id, string category, string events, string label, int value);
        public abstract CONNECTION_STATE_TYPE GetConnectionState();
        public abstract int RegisterMediaMetadataObserver(METADATA_TYPE type);
        public abstract int UnRegisterMediaMetadataObserver(METADATA_TYPE type);
        public abstract int EnableRemoteSuperResolution(uint userId, bool enable);
        public abstract int SetParameters(string parameters);
        public abstract int SetMaxMetadataSize(int size);
        public abstract int SendMetadata(Metadata metadata);
        public abstract int PushAudioFrame(MEDIA_SOURCE_TYPE type, AudioFrame frame, bool wrap);
        public abstract int PushAudioFrame(AudioFrame frame);
        public abstract int PullAudioFrame(ref AudioFrame frame);
        public abstract int SetExternalVideoSource(bool enable, bool useTexture = false);
        public abstract int PushVideoFrame(ExternalVideoFrame frame);
    }


    public abstract class IRtcEngineEventHandlerBase
    {
    }
}