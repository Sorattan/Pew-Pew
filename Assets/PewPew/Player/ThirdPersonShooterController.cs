using StarterAssets;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class ThirdPersonShooterController : MonoBehaviour
{
    public Transform LeftHandIKTarget;
    public Transform RightHandIKTarget;
    public Transform LeftElbowIKTarget;
    public Transform RightElbowIKTarget;

    [Range(0f, 1f)]
    public float HandIKAmount = 1f;
    [Range(0f, 1f)]
    public float ElbowIKAmount = 1f;

    private Animator Animator;

    [SerializeField] private CinemachineCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform pfBulletProjectile;
    [SerializeField] private Transform spawnBulletPosition;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;

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

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        //Transform hitTransfrom = null;
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            mouseWorldPosition = raycastHit.point;
            //hitTransfrom = raycastHit.transform;
        }

        if (starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
            Animator.SetLayerWeight(1, Mathf.Lerp(Animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
            Animator.SetLayerWeight(1, Mathf.Lerp(Animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }

        if (starterAssetsInputs.shoot)
        {
            //Hitscan
            //if (hitTransfrom != null)
            //{
            //    // hit something
            //    if (hitTransfrom.GetComponent<BulletTarget>() != null)
            //    {
            //        //hit target
            //    }
            //    else
            //    {
            //        //hit something else
            //    }
            //    Destroy(gameObject);
            //}
            
            //Projectile
            Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
            Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            starterAssetsInputs.shoot = false;
        }
    }
}
