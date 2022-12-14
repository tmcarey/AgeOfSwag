using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class CelestialBodies : MonoBehaviour
{
    public Gradient sunColor;
    public Gradient moonColor;
    public Gradient ambientColor;
    public float ambientIntensityScale;
    
    public AnimationCurve sunAngle;
    public List<CelestialBody> moons;
    public List<CelestialBody> suns;
    public Transform sunParent;

    [Serializable]
    public class CelestialBody
    {
        public Light light;
        public float initialIntensity;
    }

    private void Awake()
    {
        foreach (CelestialBody sun in suns)
        {
            sun.initialIntensity = sun.light.intensity;
        }
        foreach (CelestialBody moon in moons)
        {
            moon.initialIntensity = moon.light.intensity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.ambientLight =
            ambientColor.Evaluate(GameManager.Instance.currentTime.time);
        RenderSettings.ambientIntensity =
            ambientColor.Evaluate(GameManager.Instance.currentTime.time).a *
            ambientIntensityScale;
        sunParent.transform.localRotation = Quaternion.Euler(sunAngle.Evaluate(GameManager.Instance.currentTime.time) * 360, 0, 0);
        foreach (CelestialBody sun in suns)
        {
            Color currentSunColor =sunColor.Evaluate(GameManager.Instance.currentTime.time);
            sun.light.color = currentSunColor;
            sun.light.intensity = currentSunColor.a * sun.initialIntensity;
        }

        foreach (CelestialBody moon in moons)
        {
            Color currentMoonColor = moonColor.Evaluate(GameManager.Instance.currentTime.time);
            moon.light.color = currentMoonColor;
            moon.light.intensity = currentMoonColor.a * moon.initialIntensity;
        }
    }
}
