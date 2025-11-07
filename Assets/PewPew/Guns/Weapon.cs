using UnityEngine;

public enum WeaponType { Pistol, Rifle }

public class Weapon : MonoBehaviour
{
    private Rigidbody weaponBody;
    [SerializeField] private float rotationSpeed;

    [Header("Weapon Info")]
    [SerializeField] private WeaponType type;
    [SerializeField] private string animatorAimBool;

    [Header("IK Anchors on this weapon")]
    [SerializeField] private Transform rightHandGrip;
    [SerializeField] private Transform leftHandGrip;

    [SerializeField] private Transform leftHandGripEquip; // idle/low-ready
    [SerializeField] private Transform leftHandGripAim;   // ADS
    public Transform GetLeftGrip(bool aiming) => aiming && leftHandGripAim ? leftHandGripAim : leftHandGripEquip ? leftHandGripEquip : leftHandGrip; // fallback to single anchor if assigned

    public bool IsRotating { get; set; }

    public WeaponType Type => type;
    public string AnimatorAimBool => animatorAimBool;
    public Transform RightHandGrip => rightHandGrip;
    public Transform LeftHandGrip => leftHandGrip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weaponBody = GetComponent<Rigidbody>();
        if (weaponBody)
        {
            weaponBody.isKinematic = true;
        }
        IsRotating = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsRotating) return;
        transform.Rotate(Vector3.up * rotationSpeed * (1 - Mathf.Exp(-rotationSpeed * Time.deltaTime)));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (weaponBody)
        {
            weaponBody.constraints = RigidbodyConstraints.FreezePosition;
        }

        IsRotating = true;
    }

    public void ChangeWeaponBehaviour()
    {
        if (weaponBody)
        {
            weaponBody.isKinematic = true;
            weaponBody.constraints = RigidbodyConstraints.None;
        }
    }
}
