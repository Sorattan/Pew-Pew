using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[System.Serializable]
public struct SocketSet
{
    public WeaponType type;
    public Transform equipSocket;
    public Transform aimSocket;
}

public class EquipWeapon : MonoBehaviour
{
    [Header("Ray Settings")]
    [SerializeField][Range(0.0f, 2.0f)] private float rayLength = 1.2f;
    [SerializeField] private Vector3 rayOffset = new Vector3(0f, 0.9f, 0f);
    [SerializeField] private LayerMask weaponMask;
    
    private RaycastHit topRayHitInfo;
    private RaycastHit bottomRayHitInfo;

    [Header("Fallback Sockets (optional)")]
    [SerializeField] private Transform defaultEquipPos;   // used if no mapping found
    [SerializeField] private Transform defaultAimPos;

    [Header("Per-Weapon Sockets")]
    [SerializeField] private SocketSet[] socketSets;
    private Dictionary<WeaponType, SocketSet> socketMap;

    [Header("Right Hand Target")]
    [SerializeField] private TwoBoneIKConstraint rightHandIK;
    [SerializeField] private Transform rightHandTarget;
    
    [Header("Left Hand Target")]
    [SerializeField] private TwoBoneIKConstraint leftHandIK;
    [SerializeField] private Transform leftHandTarget;

    [Header("Placement")]
    [SerializeField, Tooltip("High = snappy, Low = smoother")]
    private float snapLerp = 25f;

    private Weapon currentWeapon;
    private Animator playerAnimator;
    private bool isAiming;
    private bool isEquipped;

    public bool HasWeapon => currentWeapon != null && isEquipped;
    public Weapon CurrentWeapon => currentWeapon;
    public bool IsEquipped => isEquipped;
    // public bool IsAiming => isAiming; // optional "shoot only while aiming"

    void Awake()
    {
        // Build quick lookup for sockets
        socketMap = new Dictionary<WeaponType, SocketSet>();
        foreach (var set in socketSets)
        {
            if (set.equipSocket != null && set.aimSocket != null && !socketMap.ContainsKey(set.type))
                socketMap.Add(set.type, set);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) Equip();
        if (Input.GetKeyDown(KeyCode.Q)) UnEquip();

        isAiming = Input.GetButton("Fire2");

        if (currentWeapon)
        {
            // 1) animator
            playerAnimator.SetBool("PistolAim", false);
            playerAnimator.SetBool("RifleAim", false);
            playerAnimator.SetBool(currentWeapon.AnimatorAimBool, isAiming);

            // 2) sockets: move ONLY if target parent changed
            Transform equipSock, aimSock;
            GetSockets(currentWeapon.Type, out equipSock, out aimSock);
            Transform want = isAiming ? aimSock : equipSock;

            if (currentWeapon.transform.parent != want)
                AttachToSocket(currentWeapon.transform, want);

            // 3) IK
            rightHandIK.weight = 1f;
            if (currentWeapon.RightHandGrip)
            {
                rightHandTarget.position = currentWeapon.RightHandGrip.position;
                rightHandTarget.rotation = currentWeapon.RightHandGrip.rotation;
            }

            // LEFT HAND IK — on for rifles always; for pistols only when aiming
            bool hasLeftGrip = currentWeapon.LeftHandGrip != null;
            bool wantsLeftIK = hasLeftGrip && (currentWeapon.Type == WeaponType.Rifle || isAiming);

            leftHandIK.weight = wantsLeftIK ? 1f : 0f;

            if (wantsLeftIK)
            {
                leftHandTarget.position = currentWeapon.LeftHandGrip.position;
                leftHandTarget.rotation = currentWeapon.LeftHandGrip.rotation;
            }

        }
        else
        {
            leftHandIK.weight = 0f;
            rightHandIK.weight = 0f;
            playerAnimator.SetBool("PistolAim", false);
            playerAnimator.SetBool("RifleAim", false);
        }
    }
    private void GetSockets(WeaponType type, out Transform equip, out Transform aim)
    {
        if (socketMap != null && socketMap.TryGetValue(type, out var set))
        {
            equip = set.equipSocket; aim = set.aimSocket; return;
        }
        equip = defaultEquipPos; aim = defaultAimPos; // graceful fallback
    }
    private void RaycastHandler()
    {
        Ray topRay = new Ray(transform.position + rayOffset, transform.forward);
        Ray bottomRay = new Ray(transform.position + Vector3.up * 0.175f, transform.forward);

        Debug.DrawRay(transform.position + rayOffset, transform.forward * rayLength, Color.red);
        Debug.DrawRay(transform.position + Vector3.up * 0.175f, transform.forward * rayLength, Color.green);

        Physics.Raycast(topRay, out topRayHitInfo, rayLength, weaponMask);
        Physics.Raycast(bottomRay, out bottomRayHitInfo, rayLength, weaponMask);
    }
    private void Equip()
    {
        // Block picking up a new weapon if we're already holding one
        if (isEquipped && currentWeapon != null)
        {
            Debug.Log("Already holding a weapon. Press Q to unequip first.");
            return;
        }

        RaycastHandler();

        // pick whichever ray hit last
        if (topRayHitInfo.collider) currentWeapon = topRayHitInfo.transform.GetComponent<Weapon>();
        if (bottomRayHitInfo.collider) currentWeapon = bottomRayHitInfo.transform.GetComponent<Weapon>();
        if (!currentWeapon) return;

        // turn off spin & world physics while equipped
        currentWeapon.IsRotating = false;

        var rb = currentWeapon.GetComponent<Rigidbody>();
        var col = currentWeapon.GetComponent<Collider>();
        if (rb) { rb.isKinematic = true; rb.detectCollisions = false; rb.linearVelocity = Vector3.zero; rb.angularVelocity = Vector3.zero; }
        if (col) col.enabled = false;

        // snap to the correct EQUIP socket immediately
        Transform equipSock, aimSock;
        GetSockets(currentWeapon.Type, out equipSock, out aimSock);
        AttachToSocket(currentWeapon.transform, equipSock);

        isEquipped = true;
    }
    private void UnEquip()
    {
        if (!isEquipped || !currentWeapon) return;

        // IK & anim off
        rightHandIK.weight = 0f;
        leftHandIK.weight = 0f;
        playerAnimator.SetBool("PistolAim", false);
        playerAnimator.SetBool("RifleAim", false);

        // detach and re-enable physics
        var rb = currentWeapon.GetComponent<Rigidbody>();
        var col = currentWeapon.GetComponent<Collider>();

        currentWeapon.transform.SetParent(null, true);
        if (rb) { rb.isKinematic = false; rb.detectCollisions = true; }
        if (col) col.enabled = true;

        currentWeapon = null;
        isEquipped = false;
        isAiming = false;
    }
    private void AttachToSocket(Transform item, Transform socket)
    {
        if (item == null || socket == null) return;
        item.SetParent(socket, false);       // <� keepLocal = false so it MATCHES the socket
        item.localPosition = Vector3.zero;
        item.localRotation = Quaternion.identity;
    }

}
