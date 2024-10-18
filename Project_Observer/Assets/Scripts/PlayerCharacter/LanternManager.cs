using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternManager : MonoBehaviour
{
    #region Variables

    [Header("Animations")]
    Animator animator;

    [Header("Player Character"), SerializeField]
    PlayerController playerController;

    #endregion

    #region Start Functions

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController.OnShoot += ToggleSpotlight;
    }

    void Start() { }

    #endregion

    #region Update Functions

    void Update() { }

    #endregion

    #region Base Functions



    #endregion

    #region Animations

    public void ToggleSpotlight(bool value)
    {
        animator.SetBool("toggleSpotlight", value);
    }

    #endregion
}
