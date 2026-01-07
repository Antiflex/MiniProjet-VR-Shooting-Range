using UnityEngine;

public class WeaponEquip : MonoBehaviour
{
    public Transform weaponSocket;

    public void AttachWeapon()
    {
        transform.SetParent(weaponSocket);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void DetachWeapon()
    {
        transform.SetParent(null);
    }
}
