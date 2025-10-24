using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
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
        // Handle movement only if allowed
        if (canMove)
        {
            rb.linearVelocity = moveInput * speed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
        
        // Toggle inventory with Tab key
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
    /// Toggles inventory open/closed and controls player movement
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
    }
    
    /// <summary>
    /// Disable player movement (called by other scripts if needed)
    /// </summary>
    public void DisableMovement()
    {
        canMove = false;
        moveInput = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
    }
}