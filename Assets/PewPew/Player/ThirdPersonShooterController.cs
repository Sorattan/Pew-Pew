using StarterAssets;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ThirdPersonShooterController : MonoBehaviour
{
    private Animator Animator;

    [Header("Rigging")]
    [SerializeField] private Rig aimRig;
    [SerializeField] private Transform aimTarget;
    [SerializeField] private float rigBlendSpeed = 10f;

    [SerializeField] private CinemachineCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform pfBulletProjectile;
    [SerializeField] private Transform spawnBulletPosition;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;

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
            aimTarget.position = mouseWorldPosition;

            float targetRigW = starterAssetsInputs.aim ? 1f : 0f;
            aimRig.weight = Mathf.MoveTowards(aimRig.weight, targetRigW, Time.deltaTime * rigBlendSpeed);
            //hitTransfrom = raycastHit.transform;
        }
        else
        {
            aimTarget.position = ray.origin + ray.direction * 100f;
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
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
            // Animator.SetLayerWeight(1, Mathf.Lerp(Animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }
    }
}
