using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Handle movement
        if (canMove)
        {
            rb.linearVelocity = moveInput * speed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        // Toggle inventory with Tab
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Toggles inventory open/closed and handles player movement
    /// </summary>
    private void ToggleInventory()
    {
        if (InventoryUI.Instance == null)
        {
            Debug.LogError("InventoryUI.Instance not found!");
            return;
        }

        // Toggle the inventory
        InventoryUI.Instance.ToggleInventory();

        // Update movement state based on inventory state
        // If inventory just opened, disable movement
        // If inventory just closed, enable movement
        canMove = !InventoryUI.Instance.isOpen;

        if (!canMove)
        {
            // Stop player immediately when opening inventory
            moveInput = Vector2.zero;
            rb.linearVelocity = Vector2.zero;
        }
    }

    /// <summary>
    /// Enable player movement (called by other scripts if needed)
    /// </summary>
    public void EnableMovement()
    {
        canMove = true;
        Debug.Log("Player movement enabled");
    }

    /// <summary>
    /// Disable player movement (called by other scripts if needed)
    /// </summary>
    public void DisableMovement()
    {
        canMove = false;
        moveInput = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
        Debug.Log("Player movement disabled");
    }
}