using UnityEngine;

public class PlayerIK : MonoBehaviour
{
    public Transform LeftHandIKTarget;
    public Transform RightHandIKTarget;
    public Transform LeftElbowIKTarget;
    public Transform RightElbowIKTarget;

    [Range(0f, 1f)] public float HandIKAmount = 1f;
    [Range(0f, 1f)] public float ElbowIKAmount = 1f;

    private Animator Animator;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (LeftHandIKTarget != null)
        {
            Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, HandIKAmount);
            Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, HandIKAmount);
            Animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandIKTarget.position);
            Animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandIKTarget.rotation);
        }
        if (RightHandIKTarget != null)
        {
            Animator.SetIKRotationWeight(AvatarIKGoal.RightHand, HandIKAmount);
            Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, HandIKAmount);
            Animator.SetIKPosition(AvatarIKGoal.RightHand, RightHandIKTarget.position);
            Animator.SetIKRotation(AvatarIKGoal.RightHand, RightHandIKTarget.rotation);
        }
        if (LeftElbowIKTarget != null)
        {
            Animator.SetIKHintPosition(AvatarIKHint.LeftElbow, LeftElbowIKTarget.position);
            Animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, ElbowIKAmount);
        }
        if (RightElbowIKTarget != null)
        {
            Animator.SetIKHintPosition(AvatarIKHint.RightElbow, RightElbowIKTarget.position);
            Animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, ElbowIKAmount);
        }
    }
}
