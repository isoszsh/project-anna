using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerTester : MonoBehaviour
{
    public PlayerControls controls; // Input Actions class reference
    private Vector2 moveInput;

    private void Awake()
    {
        controls = new PlayerControls(); // Your Input Actions file name
    }

    private void OnEnable()
    {
        controls.Enable();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Jump.performed += _ => Jump();
        controls.Player.Crouch.performed += _ => Crouch();
        controls.Player.Interact.performed += _ => Interact();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        transform.Translate(move * Time.deltaTime);
    }

    private void Jump()
    {
        Debug.Log("Jump");
    }

    private void Crouch()
    {
        Debug.Log("Crouch");
    }

    private void Interact()
    {
        Debug.Log("Interact");
    }
}
