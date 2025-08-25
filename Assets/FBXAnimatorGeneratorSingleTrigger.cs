using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.IO;
using System.Collections.Generic;

public class FBXAnimatorGeneratorSingleTrigger : Editor
{
    [MenuItem("Tools/Generate Animator From FBX (No Base Layer, Weight=1)")]
    static void GenerateAnimatorFromSelectedFBX()
    {
        Object selected = Selection.activeObject;
        if (selected == null)
        {
            Debug.LogError("No FBX selected!");
            return;
        }

        string path = AssetDatabase.GetAssetPath(selected);
        if (!path.EndsWith(".fbx", System.StringComparison.OrdinalIgnoreCase))
        {
            Debug.LogError("Selected asset is not an FBX!");
            return;
        }

        // Load all animation clips
        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);
        List<AnimationClip> clipList = new List<AnimationClip>();
        foreach (Object a in assets)
        {
            if (a is AnimationClip clip)
            {
                if (clip.name.Contains("__preview__")) // skip preview junk
                    continue;
                clipList.Add(clip);
            }
        }
        AnimationClip[] clips = clipList.ToArray();

        if (clips.Length == 0)
        {
            Debug.LogError("No animation clips found in FBX!");
            return;
        }

        // Create AnimatorController
        string controllerPath = Path.ChangeExtension(path, ".controller");
        AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);

        // Clear the default base layer
        controller.layers = new AnimatorControllerLayer[0];

        // Add one global trigger
        string triggerName = "PlayAll";
        controller.AddParameter(triggerName, AnimatorControllerParameterType.Trigger);

        // --- Create a layer per animation clip ---
        foreach (AnimationClip clip in clips)
        {
            string layerName = clip.name;
            controller.AddLayer(layerName);
            AnimatorControllerLayer layer = controller.layers[controller.layers.Length - 1];

            // Set weight to 1
            layer.defaultWeight = 1f;
            controller.layers[controller.layers.Length - 1] = layer; // must reassign to apply change

            AnimatorStateMachine sm = layer.stateMachine;

            // Create Idle state for this layer
            AnimatorState idle = sm.AddState("Idle");
            sm.defaultState = idle;

            // Create animation state
            AnimatorState animState = sm.AddState(clip.name);
            animState.motion = clip;

            // Transition Idle -> Animation (trigger)
            AnimatorStateTransition toAnim = idle.AddTransition(animState);
            toAnim.AddCondition(AnimatorConditionMode.If, 0, triggerName);
            toAnim.hasExitTime = false;
            toAnim.duration = 0f;

            // Transition Animation -> Idle (after clip length + 1s)
            AnimatorStateTransition backToIdle = animState.AddTransition(idle);
            backToIdle.hasExitTime = true;
            backToIdle.hasFixedDuration = true;
            backToIdle.exitTime = (clip.length + 1f) / clip.length; // normalized (clip + 1s)
            backToIdle.duration = 0f; // instant return after wait
        }

        Debug.Log("Animator Controller (no base layer, weight=1) created at: " + controllerPath);
    }
}
