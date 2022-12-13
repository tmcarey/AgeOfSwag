using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class CelestialBodies : MonoBehaviour
{
    public Gradient sunColor;
    public Gradient moonColor;
    public AnimationCurve sunAngle;
    public Light moon;
    public Light sun;

    private float initialIntensity;

    private void Awake()
    {
        initialIntensity = sun.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        sun.transform.localRotation = Quaternion.Euler(sunAngle.Evaluate(GameManager.Instance.currentTime.time) * 360.0f, 0.0f, 0.0f);
        
        Color currentSunColor =sunColor.Evaluate(GameManager.Instance.currentTime.time);
        sun.color = currentSunColor;
        sun.intensity = currentSunColor.a * initialIntensity;
        
        Color currentMoonColor = moonColor.Evaluate(GameManager.Instance.currentTime.time);
        moon.color = currentMoonColor;
        moon.intensity = currentMoonColor.a * initialIntensity;
    }
}
