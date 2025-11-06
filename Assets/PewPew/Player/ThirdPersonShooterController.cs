using StarterAssets;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private GameObject pfPlayerBullet;
    [SerializeField] private Transform spawnBulletPosition;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Animator animator;

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
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
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));

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
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }

   if (starterAssetsInputs.shoot)
        {
            // 1. 'mouseWorldPosition' değişkeni, Update() fonksiyonunun en başında,
            //    nişangahın baktığı (veya ıskaladığı) 'gerçek dünya' noktasını zaten hesapladı.
            
            // 2. Merminin gitmesi gereken yönü hesapla:
            //    Yön = (Hedef Nokta 'mouseWorldPosition') - (Çıkış Noktası 'spawnBulletPosition')
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
