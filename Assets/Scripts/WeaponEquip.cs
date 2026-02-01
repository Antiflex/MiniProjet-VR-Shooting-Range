using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class WeaponEquip : MonoBehaviour
{
    public Transform weaponSocket;
    public Transform weaponModel;

    public void AttachWeapon()
    {
        Debug.Log("attaching weapon");
        weaponModel.gameObject.GetComponent<Collider>().enabled = false;
        weaponModel.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Animator>().SetTrigger("Equip");
        GetComponent<Animator>().SetBool("IsEquipped", true);
        StartCoroutine(nameof(resettrigger));
    }

    public IEnumerator resettrigger()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<Animator>().ResetTrigger("Equip");
    }
    public void DetachWeapon()
    {
        weaponModel.gameObject.GetComponent<Collider>().enabled = true;
        weaponModel.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Animator>().SetBool("IsEquipped", false);
    }

    public void OnGrabEnter(GameObject grabbedObject)
    {
        if (grabbedObject == weaponModel.gameObject)
        {
            AttachWeapon();
        }
    }

    public void OnGrabExit(GameObject releasedObject)
    {
        if (releasedObject == weaponModel.gameObject)
        {
            DetachWeapon();
        }
    }
}
