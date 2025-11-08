using StarterAssets;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private GameObject pfPlayerBullet;
    [SerializeField] private Transform spawnBulletPosition;

    private EquipWeapon equipWeapon;
    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        equipWeapon = GetComponent<EquipWeapon>(); 
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            mouseWorldPosition = raycastHit.point;
            float targetRigW = starterAssetsInputs.aim ? 1f : 0f;
        }

        if (starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);

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
        }

   if (starterAssetsInputs.shoot)
        {
            // Only shoot if a weapon is actually equipped
            if (equipWeapon == null || !equipWeapon.HasWeapon)
            {
                starterAssetsInputs.shoot = false;
                return;
            }

            // If only shoot while aiming
            // if (!equipWeapon.IsAiming) { starterAssetsInputs.shoot = false; return; }

            Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;

            // 3. Mermiyi namlu ucunda oluştur ve yönünü 'aimDir' olarak ayarla
            GameObject bullet = Instantiate(pfPlayerBullet, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            
            // 4. Rigidbody'sini al
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // 5. Mermiyi, paralel bir yöne ('ray.direction') DEĞİL,
                //    hesapladığımız bu 'aimDir' (gerçek hedef) yönüne doğru ATEŞLE.
                rb.AddForce(aimDir * 45f, ForceMode.Impulse); 
            }
            
            // 6. Ateş etme girdisini sıfırla
            starterAssetsInputs.shoot = false;
        }

    }
}
