using System;
using FL;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    #region Variables

    [Header("Other Important Things"), SerializeField]
    Camera camera;
    Rigidbody rb;

    [Header("Movement Values")]
    Vector3 moveDirection;

    [SerializeField]
    float moveSpeed = 20f;

    [SerializeField]
    float walkSpeed = 20f;

    [SerializeField]
    float sprintMultiplier = 3f;

    // bool isSprinting;
    bool isGrounded = true;

    [Header("Look Values"), SerializeField]
    float xRotation = 0f;

    [SerializeField]
    float xSensitivity = 30f;

    [SerializeField]
    float ySensitivity = 30f;

    [Header("Input Actions")]
    PlayerInputs playerInputs;

    #endregion

    #region Delegates

    public Action<bool> OnShoot = delegate { };

    #endregion

    #region Start Functions

    private void Awake()
    {
        playerInputs = new PlayerInputs();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent the Rigidbody from rotating due to physics.

        // playerInputs.OnFoot.Move.started += HandleMovement;
        playerInputs.OnFoot.Look.performed += HandleLook;
        playerInputs.OnFoot.Jump.performed += HandleJump;
        playerInputs.OnFoot.Sprint.started += HandleSprint;
        playerInputs.OnFoot.Sprint.canceled += HandleSprint;
        playerInputs.OnFoot.Interact.performed += HandleInteract;
        playerInputs.OnFoot.Aim.performed += HandleAim;
        playerInputs.OnFoot.Shoot.started += StartShoot;
        playerInputs.OnFoot.Shoot.canceled += StopShoot;
        playerInputs.OnFoot.SwitchWeapons.performed += HandleWeaponSwitch;

        playerInputs.OnFoot.Escape.performed += HandleEscape;
    }

    private void OnDestroy()
    {
        // playerInputs.OnFoot.Move.performed -= HandleMovement;
        playerInputs.OnFoot.Look.performed -= HandleLook;
        playerInputs.OnFoot.Jump.performed -= HandleJump;
        playerInputs.OnFoot.Sprint.started -= HandleSprint;
        playerInputs.OnFoot.Sprint.canceled -= HandleSprint;
        playerInputs.OnFoot.Interact.performed -= HandleInteract;
        playerInputs.OnFoot.Aim.performed -= HandleAim;
        playerInputs.OnFoot.Shoot.started -= StartShoot;
        playerInputs.OnFoot.Shoot.canceled -= StopShoot;
        playerInputs.OnFoot.SwitchWeapons.performed -= HandleWeaponSwitch;

        playerInputs.OnFoot.Escape.performed -= HandleEscape;
    }

    private void OnEnable()
    {
        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }

    void Start()
    {
        MouseHandler.ToggleCursor(false);
    }

    #endregion

    #region Update Functions

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDirection * Time.fixedDeltaTime);
    }

    void Update()
    {
        HandleMovement(playerInputs.OnFoot.Move.ReadValue<Vector2>());
    }

    #endregion

    #region Event Handlers

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Ground")
            isGrounded = true;
        else
            isGrounded = false;
    }

    #endregion

    #region Base Functions

    void HandleMovement(Vector2 input)
    {
        // Calculate the movement direction based on the camera’s forward direction
        Vector3 forward = camera.transform.forward;
        Vector3 right = camera.transform.right;

        // Ignore vertical movement (Y-axis)
        forward.y = 0;
        forward.Normalize();
        right.y = 0;
        right.Normalize();

        moveDirection = (forward * input.y + right * input.x).normalized * moveSpeed;
    }

    void HandleLook(CallbackContext ctx)
    {
        // Storing the mouse move values.
        float mouseX = ctx.ReadValue<Vector2>().x;
        float mouseY = ctx.ReadValue<Vector2>().y;

        // Calculated camera rotation for looking up and down.
        xRotation -= mouseY * Time.deltaTime * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        // Apply this to our camera transform
        camera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        // Rotate the player to look left and right.
        transform.Rotate(Vector3.up * mouseX * Time.deltaTime * xSensitivity);
    }

    void HandleJump(CallbackContext ctx)
    {
        if (isGrounded)
            rb.AddForce(new Vector3(0, 2, 0), ForceMode.Impulse);
    }

    void HandleSprint(CallbackContext ctx)
    {
        moveSpeed = ctx.ReadValue<float>() > 0 ? walkSpeed * sprintMultiplier : walkSpeed;
    }

    void HandleInteract(CallbackContext ctx)
    {
        if (PlayerCharacter.Instance.GetCurrInteractable != null)
            PlayerCharacter.Instance.GetCurrInteractable.Interact();
    }

    void HandleAim(CallbackContext ctx) { }

    void StartShoot(CallbackContext ctx)
    {
        OnShoot?.Invoke(true);
    }

    void StopShoot(CallbackContext ctx)
    {
        OnShoot?.Invoke(false);
    }

    void HandleEscape(CallbackContext ctx)
    {
        MouseHandler.ToggleCursor(true);
    }

    void HandleWeaponSwitch(CallbackContext ctx) => PlayerCharacter.Instance.SwitchWeapons();

    #endregion
}
