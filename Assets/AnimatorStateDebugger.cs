using UnityEngine;

public class AnimatorStateDebugger : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (animator == null) return;

        int layerCount = animator.layerCount;
        for (int i = 0; i < layerCount; i++)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(i);

            // Convert hash back to name
            string stateName = animator.GetCurrentAnimatorClipInfo(i).Length > 0
                ? animator.GetCurrentAnimatorClipInfo(i)[0].clip.name
                : "<Empty>";

            Debug.Log($"Frame {Time.frameCount} | Layer {i} | State: {stateName} | Normalized Time: {stateInfo.normalizedTime:F2}");
        }
    }
}
