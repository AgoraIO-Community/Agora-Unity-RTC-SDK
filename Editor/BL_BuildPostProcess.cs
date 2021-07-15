#if UNITY_STANDALONE_WIN
using System.IO;
using UnityEditor;

public class BL_BuildPostProcess
{
    [UnityEditor.Callbacks.PostProcessBuild(999)]
    public static void OnPostprocessBuild(UnityEditor.BuildTarget BuildTarget, string path)
    {
        var arch = BuildTarget == BuildTarget.StandaloneWindows64 ? "x86_64/" : "x86/";
        var exeName = "AgoraRtcScreenSharing.exe";
        var strPathFrom = UnityEngine.Application.dataPath + "/Agora-Plugin/Plugins/" + arch + exeName;
        UnityEngine.Debug.LogFormat("src path: {0}", strPathFrom);
        var nIdxSlash = path.LastIndexOf('/');
        var nIdxDot = path.LastIndexOf('.');
        var strRootTarget = path.Substring(0, nIdxSlash);
        var strPluginsTarget = strRootTarget + path.Substring(nIdxSlash, nIdxDot - nIdxSlash) + "_Data/Plugins/";
        var strPathTargetFile = File.Exists(strPluginsTarget + arch)
            ? strPluginsTarget + arch + exeName
            : strPluginsTarget + exeName;
        var strPathTargetFileBackup = strPluginsTarget + arch + exeName;
        File.Copy(strPathFrom, strPathTargetFile);
        File.Copy(strPathFrom, strPathTargetFileBackup);
        UnityEngine.Debug.Log("Copy " + strPathFrom + " to " + strPathTargetFile);
        UnityEngine.Debug.Log("Copy " + strPathFrom + " to " + strPathTargetFileBackup);
    }
}
#endif