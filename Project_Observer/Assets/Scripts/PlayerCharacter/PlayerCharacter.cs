using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    #region Variables

    #region Singleton

    public static PlayerCharacter Instance { get; private set; }

    #endregion

    [Header("Player Data")]
    PlayerController playerController;

    [SerializeField]
    Camera playerCam;

    [SerializeField]
    public SanitySystem sanity;

    [SerializeField]
    float sanityHealAmount = 4f;

    [SerializeField]
    float sanityDrainAmount = 2f;
    bool isInLargeLightSrc = false;

    [Header("Raycast Data"), SerializeField]
    float rayDistance = 10f;
    Ray pointerRay;
    RaycastHit hit;
    bool didPointerHit;
    bool canInteract = false;

    Interactable currInteractable;

    [Header("UI Componenets")]
    public TMP_Text interactionMessage;

    [Header("Audio Data"), SerializeField]
    AudioSource breathSrc;

    [Header("Weapon System"), SerializeField]
    GameObject lantern;

    [SerializeField]
    GameObject teddyBear;

    public bool isTeddyPickedUp = false;

    #region Properties

    public Interactable GetCurrInteractable
    {
        get { return currInteractable; }
    }

    public Camera GetPlayerCam
    {
        get { return playerCam; }
    }

    #endregion

    #endregion

    #region Delegates

    public static Action OnSpotlightTurnedOff = delegate()
    {
        Debug.Log($"Spotlight OFF");
    };

    #endregion

    #region Start Functions

    private void Awake()
    {
        // Check if an instance of the Player already exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy this instance if one already exists
            return;
        }

        Instance = this; // Assign the correct instance of the player
        DontDestroyOnLoad(gameObject); // Make sure it exists throughout all scenes

        playerController = GetComponent<PlayerController>();
    }

    #endregion

    #region Update Functions

    private void Update()
    {
        pointerRay = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        // Cast the ray and specify a max distance
        didPointerHit = Physics.Raycast(pointerRay, out hit, rayDistance);

        HandleInteraction();
        MatchBreathing();
    }

    #endregion

    #region Base Functions

    public void SwitchWeapons()
    {
        if (!isTeddyPickedUp)
            return;

        if (lantern.activeSelf)
        {
            lantern.SetActive(false);
            teddyBear.SetActive(true);

            if (isInLargeLightSrc)
                StartSanityHeal(sanityHealAmount);
            // else
            //     StartSanityDrain(sanityDrainAmount);
        }
        else
        {
            teddyBear.SetActive(false);
            lantern.SetActive(true);

            StopSanityHeal();
            StopSanityDrain();
        }
    }

    public void PickupTeddy() => isTeddyPickedUp = true;

    #region Sanity Functions

    public void StartSanityDrain(float value) => sanity.StartSanityDrain(value);

    public void StartSanityHeal(float value) => sanity.StartSanityHeal(value);

    public void StopSanityDrain() => sanity.StopSanityDrain();

    public void StopSanityHeal() => sanity.StopSanityHeal();

    public void HealSanity(float value) => sanity.HealSanity(value);

    public void DamageSanity(float value) => sanity.DamageSanity(value);

    void MatchBreathing()
    {
        float targetVolume = 1 - Mathf.Clamp01(sanity.GetCurrentSanity / 100f);

        breathSrc.volume = Mathf.Lerp(breathSrc.volume, targetVolume, Time.deltaTime * 2f);
    }

    #endregion

    private void HandleInteraction()
    {
        if (didPointerHit)
        {
            GameObject hitObject = hit.transform.gameObject;

            if (hitObject.tag == "InteractableItem")
            {
                if (currInteractable)
                {
                    if (currInteractable.gameObject == hitObject)
                        currInteractable = hitObject.GetComponent<Interactable>();
                    else
                    {
                        currInteractable.HoverVisual(false);
                        currInteractable = hitObject.GetComponent<Interactable>();
                    }

                    canInteract =
                        Vector3.Distance(transform.position, currInteractable.transform.position)
                        <= 2;

                    interactionMessage.text = currInteractable.GetInteractMsg;
                    currInteractable.HoverVisual(true, canInteract);
                }
                else
                    currInteractable = hitObject.GetComponent<Interactable>();
            }
            else
            {
                interactionMessage.text = "";

                if (currInteractable)
                {
                    currInteractable.HoverVisual(false);
                    currInteractable = null;
                }
            }
        }
        else
        {
            interactionMessage.text = "";

            if (currInteractable)
            {
                currInteractable.HoverVisual(false);
                currInteractable = null;
            }
        }
    }

    #region UI Functions

    public void ToggleInteractMessage(bool value)
    {
        interactionMessage.gameObject.SetActive(value);
    }

    public void ClearInteractMessage() => interactionMessage.text = "";

    #endregion

    #endregion
}
