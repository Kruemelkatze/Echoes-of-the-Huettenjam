using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public bool CanMove { get; set; } = true;
    public bool isMoving{ get; set; } = false;

    [Header("Functional Options")]
    [SerializeField] private bool canUseHeadbob = true;

    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float gravity = 30.0f;
    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1, 180)] private float upperLookLimit = 80.0f;
    [SerializeField, Range(1, 180)] private float lowerLookLimit = 80.0f;

    [Header("Headbob Parameters")]
    [SerializeField] private float walkBobSpeed = 14.0f;
    [SerializeField] private float walkBobAmount = 0.05f;
    private float bobDefaultYPos = 0;
    private float bobTimer;
    private bool stepTaken = false;

    private Camera playerCamera;
    private CharacterController characterController;

    private Vector3 moveDirection;
    private Vector2 currentInput;
    private float rotationX = 0;

    // Start is called before the first frame update
    void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        bobDefaultYPos = playerCamera.transform.localPosition.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove)
        {
            HandleMovementInput();

            HandleMouseLook();

            if (canUseHeadbob)
            {
                HandleHeadbob();
            }

            ApplyFinalMovements();
        }

    }

    private void HandleHeadbob()
    {
        if (!characterController.isGrounded)
        {
            return;
        }

        if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f) {
            bobTimer += Time.deltaTime * walkBobSpeed;
            // Debug.Log(Mathf.Sin(bobTimer));
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                bobDefaultYPos + Mathf.Sin(bobTimer) * walkBobAmount,
                playerCamera.transform.localPosition.z
            );

            // player steps:
            if (Mathf.Sin(bobTimer) <= -0.99f && !stepTaken) {
                // player head is near lowest point -> play step sound once
                AudioController.Instance.PlaySound("click");
                stepTaken = true;
            }
            if (Mathf.Sin(bobTimer) >= 0.99f) {
                // reset step taken for next sound
                stepTaken = false;
            }
        }
    }

    private void ApplyFinalMovements()
    {
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void HandleMouseLook()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);
    }

    private void HandleMovementInput()
    {
        if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f) isMoving = true;
        else isMoving = false;

        currentInput = new Vector2(walkSpeed * Input.GetAxis("Vertical"), walkSpeed * Input.GetAxis("Horizontal"));
        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }
}
