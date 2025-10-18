//Justin Gabon
//10/12/2025

/*
Simple inventory UI like Stardew Valley
Press Tab to open, shows all 36 slots
First row (slots 0-9) = hotbar (always visible at bottom)
Can drag items to reorganize
*/

using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject inventoryPanel;

    [Header("Slot Grid")]
    [SerializeField] private Transform slotsContainer;
    [SerializeField] private GameObject slotPrefab;

    private SimpleInventorySlot[] slotComponents;
    public bool isOpen = false;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CreateInventorySlots();
        CloseInventory();

        // Subscribe to inventory changes
        PlayerInventory.OnInventoryChanged += RefreshDisplay;
        PlayerInventory.OnSlotChanged += OnSlotChanged;
    }

    private void OnDestroy()
    {
        PlayerInventory.OnInventoryChanged -= RefreshDisplay;
        PlayerInventory.OnSlotChanged -= OnSlotChanged;
    }

    private void Update()
    {
        // Toggle inventory with Tab ONLY
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("TAB KEY DOWN DETECTED!");
            ToggleInventory();
        }

        // Close with Escape
        if (Input.GetKeyDown(KeyCode.Escape) && isOpen)
        {
            CloseInventory();
        }
    }

    /// <summary>
    /// Creates all inventory slot UI elements
    /// </summary>
    private void CreateInventorySlots()
    {
        Debug.Log("=== CreateInventorySlots called ===");

        if (PlayerInventory.Instance == null)
        {
            Debug.LogError("PlayerInventory.Instance is NULL!");
            return;
        }

        if (slotsContainer == null)
        {
            Debug.LogError("slotsContainer is NULL! Assign Content in Inspector!");
            return;
        }

        if (slotPrefab == null)
        {
            Debug.LogError("slotPrefab is NULL! Assign InventorySlot prefab in Inspector!");
            return;
        }

        int totalSlots = PlayerInventory.Instance.TotalSlots;
        Debug.Log($"Creating {totalSlots} inventory slots...");

        slotComponents = new SimpleInventorySlot[totalSlots];

        for (int i = 0; i < totalSlots; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotsContainer);
            slotObj.name = $"InventorySlot_{i}";

            SimpleInventorySlot slotComponent = slotObj.GetComponent<SimpleInventorySlot>();
            if (slotComponent != null)
            {
                slotComponent.Initialize(i);
                slotComponents[i] = slotComponent;
            }
            else
            {
                Debug.LogError($"InventorySlot_{i} has no SimpleInventorySlot component!");
            }

            // Visual separator after first row (hotbar)
            if (i == PlayerInventory.Instance.HotbarSize - 1)
            {
                // You can add a visual divider here if you want
            }
        }

        Debug.Log($"✓ Created {totalSlots} inventory slots successfully!");
        RefreshDisplay();
    }

    /// <summary>
    /// Toggles inventory open/closed
    /// </summary>
    public void ToggleInventory()
    {
        Debug.Log("ToggleInventory called! Current state: " + (isOpen ? "OPEN" : "CLOSED"));

        if (isOpen)
        {
            CloseInventory();
        }
        else
        {
            OpenInventory();
        }
    }

    /// <summary>
    /// Opens inventory
    /// </summary>
    public void OpenInventory()
    {
        if (inventoryPanel == null)
        {
            Debug.LogError("inventoryPanel is NULL! Assign InventoryPanel in Inspector!");
            return;
        }

        isOpen = true;
        inventoryPanel.SetActive(true);
        RefreshDisplay();

        // Disable player movement
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null)
        {
            player.DisableMovement();
        }

        Debug.Log("✓ Inventory opened (Tab to close)");
    }

    /// <summary>
    /// Closes inventory
    /// </summary>
    public void CloseInventory()
    {
        if (inventoryPanel == null)
        {
            Debug.LogError("inventoryPanel is NULL!");
            return;
        }

        isOpen = false;
        inventoryPanel.SetActive(false);

        // Enable player movement
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null)
        {
            player.EnableMovement();
        }

        Debug.Log("✓ Inventory closed");
    }

    /// <summary>
    /// Refreshes entire inventory display
    /// </summary>
    private void RefreshDisplay()
    {
        if (PlayerInventory.Instance == null || slotComponents == null) return;

        for (int i = 0; i < slotComponents.Length; i++)
        {
            if (slotComponents[i] != null)
            {
                InventorySlot slotData = PlayerInventory.Instance.GetSlot(i);
                slotComponents[i].UpdateDisplay(slotData);
            }
        }
    }

    /// <summary>
    /// Called when a specific slot changes
    /// </summary>
    private void OnSlotChanged(int slotIndex, InventorySlot slotData)
    {
        if (slotIndex >= 0 && slotIndex < slotComponents.Length && slotComponents[slotIndex] != null)
        {
            slotComponents[slotIndex].UpdateDisplay(slotData);
        }
    }
}