using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    #region Variables

    #region Singleton

    public static PlayerCharacter Instance { get; private set; }

    #endregion

    [Header("Player Controller")]
    PlayerController playerController;

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



    #endregion

    #region Base Functions



    #endregion
}
