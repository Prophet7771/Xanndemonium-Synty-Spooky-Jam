using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SanitySystem : MonoBehaviour
{
    #region Variables

    [Header("Sanity System")]
    float maxSanity = 100; // This is for when we restore sanity

    [SerializeField]
    float currentSanity = 100;

    [SerializeField]
    PostProcessVolume sanityVolume;
    Vignette vignette;

    [Header("Damage Over Time")]
    Coroutine damageCoroutine; // To keep track of the DoT coroutine
    public float interval = 1f; // Time between each damage tick

    [Header("UI Components")]
    Image sanityMeter;

    #endregion

    #region Start Functions

    private void Awake()
    {
        sanityVolume.profile.TryGetSettings<Vignette>(out vignette);
    }

    #endregion

    #region Update Functions

    private void Update()
    {
        vignette.enabled.Override(true);
        vignette.intensity.value = 1 - Mathf.Clamp01(currentSanity / 100f);
    }

    #endregion

    #region Base Functions

    public void DamageSanity(float value)
    {
        currentSanity -= value;
    }

    public void StartSanityDrain(float value)
    {
        damageCoroutine = StartCoroutine(ApplyDoT(value));
    }

    public void StopSanityDrain()
    {
        StopCoroutine(damageCoroutine);
        damageCoroutine = null;
    }

    private IEnumerator ApplyDoT(float value)
    {
        while (true) // Continuously apply damage while in trigger
        {
            // Apply damage logic here
            DamageSanity(value);

            // Wait for the interval before applying damage again
            yield return new WaitForSeconds(interval);
        }
    }

    #endregion
}
