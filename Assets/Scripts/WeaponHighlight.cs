using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WeaponHighlight : MonoBehaviour
{
    [Header("Smooth Settings")]
    public float hoverSpeed = 6f;
    public float grabSpeed = 8f;

    private List<Renderer> rend;
    private MaterialPropertyBlock mpb;

    private float hoverValue = 0f;
    private float grabValue = 0f;

    private float targetHover = 0f;
    private float targetGrab = 0f;

    void Start()
    {
        rend = GetComponentsInChildren<Renderer>().ToList();
        mpb = new MaterialPropertyBlock();
    }

    void Update()
    {
        // transitions douces
        hoverValue = Mathf.Lerp(hoverValue, targetHover, Time.deltaTime * hoverSpeed);
        grabValue = Mathf.Lerp(grabValue, targetGrab, Time.deltaTime * grabSpeed);

        // mise à jour MPB
        foreach (Renderer rend in rend)
        {
            UpdateMaterialPropertyBlock(rend);
        }
    }

    private void UpdateMaterialPropertyBlock(Renderer rend)
    {
        rend.GetPropertyBlock(mpb);
        mpb.SetFloat("_Hover", hoverValue);
        mpb.SetFloat("_Grab", grabValue);
        rend.SetPropertyBlock(mpb);
    }


    // Appelé par XR Interaction Toolkit
    public void OnHoverEnter() => targetHover = 1f;
    public void OnHoverExit() => targetHover = 0f;
    public void OnGrabEnter() => targetGrab = 1f;
    public void OnGrabExit() => targetGrab = 0f;
}
