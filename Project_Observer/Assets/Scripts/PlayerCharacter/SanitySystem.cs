using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SanitySystem : MonoBehaviour
{
    #region Variables

    [Header("Sanity System")]
    float maxSanity = 100; // This is for when we restore sanity

    [SerializeField, Range(0, 100)]
    float currentSanity = 100;

    // PostProcessVolume sanityVolume;

    [SerializeField]
    Volume sanityVolume;
    Vignette vignette;

    [Header("Damage Over Time")]
    Coroutine damageCoroutine; // To keep track of the DoT coroutine
    Coroutine healCoroutine; // To keep track of the DoT coroutine
    public float dotInterval = 1f; // Time between each damage tick
    public float hotInterval = 1f; // Time between each heal tick
    public float sanityLerpInterval = 2.0f; // Time for sanity to drop

    [Header("UI Components")]
    Image sanityMeter;

    [SerializeField]
    TMP_Text sanityTextValue;

    #endregion

    #region Properties

    public float GetCurrentSanity
    {
        get { return currentSanity; }
    }

    #endregion

    #region Start Functions

    private void Awake()
    {
        sanityVolume.profile.TryGet<Vignette>(out vignette);
    }

    private void Start()
    {
        if (sanityVolume != null || vignette != null)
        {
            sanityVolume.enabled = false;
            sanityVolume.enabled = true;
        }
    }

    #endregion

    #region Update Functions

    private void Update()
    {
        sanityTextValue.text = currentSanity.ToString();

        if (vignette == null)
            sanityVolume.profile.TryGet<Vignette>(out vignette);

        float targetIntensity = 1 - Mathf.Clamp01(currentSanity / 100f);

        vignette.intensity.value = Mathf.Lerp(
            vignette.intensity.value,
            targetIntensity,
            Time.deltaTime * sanityLerpInterval
        );
    }

    #endregion

    #region Base Functions

    public void HealSanity(float value)
    {
        if (currentSanity + value < maxSanity)
            currentSanity = maxSanity;
        else
            currentSanity += value;
    }

    public void DamageSanity(float value)
    {
        if (currentSanity - value < 0)
            currentSanity = 0;
        else
            currentSanity -= value;
    }

    public void StartSanityDrain(float value)
    {
        if (damageCoroutine == null)
            damageCoroutine = StartCoroutine(ApplyDoT(value));
    }

    public void StartSanityHeal(float value)
    {
        if (healCoroutine == null)
            healCoroutine = StartCoroutine(ApplyHoT(value));
    }

    public void StopSanityDrain()
    {
        if (damageCoroutine == null)
            return;

        StopCoroutine(damageCoroutine);
        damageCoroutine = null;
    }

    public void StopSanityHeal()
    {
        if (healCoroutine == null)
            return;

        StopCoroutine(healCoroutine);
        healCoroutine = null;
    }

    private IEnumerator ApplyDoT(float value)
    {
        while (true) // Continuously apply damage while in trigger
        {
            // Apply damage logic here
            DamageSanity(value);

            // Wait for the interval before applying damage again
            yield return new WaitForSeconds(dotInterval);
        }
    }

    private IEnumerator ApplyHoT(float value)
    {
        while (true) // Continuously apply damage while in trigger
        {
            // Apply damage logic here
            HealSanity(value);

            // Wait for the interval before applying damage again
            yield return new WaitForSeconds(hotInterval);
        }
    }

    #endregion
}
