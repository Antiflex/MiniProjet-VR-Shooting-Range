#if UNITY_XR_MANAGEMENT && UNITY_OPENXR_PACKAGE_1_6
using UnityEngine.XR.OpenXR.Features;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.XR.OpenXR.Features;
#endif

namespace VRBuilder.Core
{
    // TODO remove this once the issue is resolved by Unity
    /// <summary>
    /// Workaround for OpenXR issue OXRB-656 where XR controllers are inverted by Y axis when using Meta Quest 3 with OpenXR plugin.
    /// </summary>
    /// <remarks>
    /// When removing this also remove the enabling code in <see cref="OpenXRPackageEnabler"/>.
    /// </remarks>
#if UNITY_EDITOR
    [OpenXRFeature(
        TargetOpenXRApiVersion = "1.1.53",
        UiName = "Workaround for issue OXRB-656",
        BuildTargetGroups = new[] { BuildTargetGroup.Android, BuildTargetGroup.Standalone },
        DocumentationLink = "https://issuetracker.unity3d.com/issues/xr-interaction-toolkit-xr-controllers-are-inverted-by-y-axis-when-using-meta-quest-3-with-openxr-plugin",
        FeatureId = featureId
    )]
#endif
    public class WorkaroundForOXRB656 : OpenXRFeature
    {
        /// <summary>
        /// The feature ID for this workaround.
        /// </summary>
        public const string featureId = "com.vrbuilder.openxr.workaround.oxrb656";
        private static void EnableWorkaroundForOXRB656(BuildTargetGroup buildTargetGroup)
        {
        #if UNITY_OPENXR_PACKAGE_1_6
            OpenXRFeature workaroundFeature = FeatureHelpers.GetFeatureWithIdForBuildTarget(buildTargetGroup, WorkaroundForOXRB656.featureId);
            {
                if (workaroundFeature != null)
                {
                    workaroundFeature.enabled = true;
                }
                else
                {
                    UnityEngine.Debug.LogError("[VR Builder] Could not find OpenXR feature: WorkaroundForOXRB656. Please enable it manually in Project Settings > XR Plug-in Management > OpenXR.");
                }
            }
        #endif
    }
}
#endif

using UnityEditor;

/// <summary>
/// Enable WorkaroundForOXRB656, which is caused by OpenXR 1.16.x and affects Meta Quest headsets. For details, see https://issuetracker-mig.prd.it.unity3d.com/issues/xr-interaction-toolkit-xr-controllers-are-inverted-by-y-axis-when-using-meta-quest-3-with-openxr-plugin
/// </summary>
/// <param name="buildTargetGroup"></param>