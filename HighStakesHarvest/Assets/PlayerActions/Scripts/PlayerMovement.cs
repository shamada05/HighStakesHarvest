using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float speed = 5f;
    [SerializeField] private float currentSpeed;
    private Rigidbody2D rb;
    private Vector2 moveInput; 

    /************** testing buffs temporarily **************
    public PlayerBuffManager playerBuffManager;
    public CropBuffManager cropBuffManager;
    public SpeedBuff speedBuff;
    public ValueBuff valueBuff;
    *********************************************************/

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {

        // adds keybinds for buff testing
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerBuffManager.AddBuff(speedBuff);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            cropBuffManager.AddBuff(valueBuff);
        }*/

        rb.linearVelocity = moveInput * currentSpeed; 
        
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void ApplySpeedBuff(float modifier)
    {
        currentSpeed *= modifier;
    }

    public void RemoveSpeedBuff(float modifier)
    {
        currentSpeed /= modifier;
    }
}
