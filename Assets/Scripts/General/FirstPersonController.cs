using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{
    public bool CanMove { get; set; } = true;
    public bool isMoving { get; set; } = false;

    [Header("Functional Options")] [SerializeField]
    private bool canUseHeadbob = true;

    [Header("Movement Parameters")] [SerializeField]
    private float walkSpeed = 3.0f;

    [SerializeField] private float gravity = 30.0f;

    [Header("Look Parameters")] [SerializeField, Range(1, 10)]
    private float lookSpeedX = 2.0f;

    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1, 180)] private float upperLookLimit = 80.0f;
    [SerializeField, Range(1, 180)] private float lowerLookLimit = 80.0f;

    [Header("Headbob Parameters")] [SerializeField]
    private float walkBobSpeed = 14.0f;

    [SerializeField] private float walkBobAmount = 0.05f;
    private float bobDefaultYPos = 0;
    private float bobTimer;
    private bool stepTaken = false;

    private Camera playerCamera;
    private CharacterController characterController;

    private Vector3 moveDirection;
    private Vector2 currentInput;
    private float rotationX = 0;

    private GameObject _previousLookedAtObject = null;
    private Clue _previousLookedAtClue = null;

    private bool _headbobEnabled = true;

    [SerializeField] private float highlightDistance = 6.0f;

    [SerializeField] private CanvasGroup clueUnlockedUi;


    // Start is called before the first frame update
    void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        bobDefaultYPos = playerCamera.transform.localPosition.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _headbobEnabled = PlayerPrefs.GetInt("headbob", 1) == 1;

        if (clueUnlockedUi)
            clueUnlockedUi.alpha = 0;
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

            HandleLookingAtThings();
            HandleInteract();
        }
    }

    private void HandleHeadbob()
    {
        if (!characterController.isGrounded)
        {
            return;
        }

        if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
        {
            bobTimer += Time.deltaTime * walkBobSpeed;
            // Debug.Log(Mathf.Sin(bobTimer));

            if (_headbobEnabled)
            {
                playerCamera.transform.localPosition = new Vector3(
                    playerCamera.transform.localPosition.x,
                    bobDefaultYPos + Mathf.Sin(bobTimer) * walkBobAmount,
                    playerCamera.transform.localPosition.z
                );
            }

            // player steps:
            if (Mathf.Sin(bobTimer) <= -0.99f && !stepTaken)
            {
                // player head is near lowest point -> play step sound once
                AudioController.Instance.PlaySound("steps_snow");
                stepTaken = true;
            }

            if (Mathf.Sin(bobTimer) >= 0.99f)
            {
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
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) +
                        (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }

    private void HandleLookingAtThings()
    {
        // Raycast to screen center
        Debug.DrawRay(playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)).origin,
            playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)).direction * highlightDistance, Color.red);
        if (!Physics.Raycast(playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out var hit,
                highlightDistance))
        {
            UnhighlightObject();
            return;
        }

        var obj = hit.collider.gameObject;
        if (_previousLookedAtObject == obj)
            return;

        UnhighlightObject();

        if (!obj || hit.distance > highlightDistance)
            return;

        _previousLookedAtObject = obj;
        _previousLookedAtClue = null;

        var clue = obj.GetComponent<Clue>();
        if (clue)
        {
            clue.LookedAt = true;
            _previousLookedAtObject = obj;
            _previousLookedAtClue = clue;
        }
    }

    private void UnhighlightObject()
    {
        if (!_previousLookedAtObject)
            return;

        if (_previousLookedAtClue)
        {
            _previousLookedAtClue.LookedAt = false;
        }
    }

    private void HandleInteract()
    {
        if (!_previousLookedAtClue)
            return;

        var isPressingInteract = Mouse.current.leftButton.wasPressedThisFrame ||
                                 Keyboard.current.eKey.wasPressedThisFrame ||
                                 Keyboard.current.spaceKey.wasPressedThisFrame;

        if (!isPressingInteract)
            return;

        if (_previousLookedAtClue.Interact() && clueUnlockedUi)
        {
            // Fade sequence
            var sequence = DOTween.Sequence();
            sequence.Append(clueUnlockedUi.DOFade(1, 0.5f));
            sequence.AppendInterval(1.5f);
            sequence.Append(clueUnlockedUi.DOFade(0, 0.5f));
            sequence.Play();
        }
    }
}