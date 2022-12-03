using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day : MonoBehaviour
{
    [SerializeField] private float tickPerRealTime;
    [SerializeField] private float nightFogDensity;
    [SerializeField] private float fogDensityCalc;
    [SerializeField] private float nightFarClip;
    [SerializeField] private float farClipCalc;

    public static bool isNight = false;
    private float dayFogDensity;
    private float dayFarClip;
    public float currentFogDensity;
    public float currentFarClip;

    private Camera view;

    private void Start()
    {
        view = FindObjectOfType<Camera>();
        dayFogDensity = RenderSettings.fogDensity;
        dayFarClip = view.farClipPlane;
    }

    private void Update()
    {
        transform.Rotate(Vector3.right, 0.1f * tickPerRealTime * Time.deltaTime);

        if (transform.eulerAngles.x >= 170f)
        {
            isNight = true;
        }
        else if (transform.eulerAngles.x <= 10f)
        {
            isNight = false;
        }

        if (isNight)
        {
            if (currentFogDensity <= nightFogDensity)
            {
                currentFogDensity += 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
            if (currentFarClip <= nightFarClip)
            {
                currentFarClip += 0.1f * farClipCalc * Time.deltaTime;
                view.farClipPlane = currentFarClip;
            }
        }
        else
        {
            if (currentFogDensity >= dayFogDensity)
            {
                currentFogDensity -= 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
            if (currentFarClip >= dayFarClip)
            {
                currentFarClip -= 0.1f * farClipCalc * Time.deltaTime;
                view.farClipPlane = currentFarClip;
            }
        }
    }
}
