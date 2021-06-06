//  IAgoraRtcEngine.cs
//
//  Created by Yiqing Huang on June 3, 2021.
//  Modified by Yiqing Huang on June 6, 2021.
//
//  Copyright © 2021 Agora. All rights reserved.
//

namespace agora_gaming_rtc
{
    internal static partial class ObsoleteMethodWarning
    {
        internal const string GeneralWarning = "This method is deprecated.";
        internal const string JoinChannelByKeyWarning = "This method is deprecated. Please call JoinChannel instead.";

        internal const string SetLocalVoiceReverbPresetWarning =
            "This method is deprecated. Please call SetAudioEffectPresent or SetVoiceBeautifierPresent instead.";

        internal const string SetLogFileWarning =
            "This method is deprecated. Please use logConfig in the initialize method instead.";

        internal const string SetLogFilterWarning =
            "This method is deprecated. Please use logConfig in the initialize method instead.";

        internal const string SetLogFileSizeWarning =
            "This method is deprecated. Please use logConfig in the Initialize method instead.";

        internal const string SetLocalVideoMirrorModeWarning =
            "This method is deprecated. Please call SetupLocalVideo or SetLocalRenderMode instead.";

        internal const string SetEncryptionSecretWarning =
            "This method is deprecated. Please call EnableEncryption instead.";

        internal const string SetEncryptionModeWarning =
            "This method is deprecated. Please call EnableEncryption instead.";

        internal const string DestroyWarning = "This method is deprecated. Please call Dispose instead.";

        internal const string GeneralStructureWarning = "This structure is deprecated";

        internal const string ReleaseChannelWarning = "This structure is deprecated. Please call Dispose instead";

        internal const string CreateChannelWarning =
            "This method is deprecated. Please call AgoraRtcEngine.CreateChannel instead";
    }
}