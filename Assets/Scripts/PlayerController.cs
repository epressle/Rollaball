using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public float jump_speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    private Rigidbody rb;
    private Transform tf;
    private float movementX;
    private float movementY;
    private float movementZ;
    private bool grounded = true;
    private bool single_jump;
    private bool double_jump;

    private int count;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tf = GetComponent<Transform>();

        count = 0;
        SetCountText();

        winTextObject.SetActive(false);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    bool OutOfBounds() 
    {
        Vector3 position = transform.position;
        if (position.y < -10) 
        {
            return true;
        }
        return false;
    }
    
    void OnJump() 
    {
        // if ball is grounded then jump; no longer grounded so make grounded false and set single_jump to true
        if (grounded) 
        {
            single_jump = true;
            grounded = false;
            Vector3 movement = new Vector3(0.0f, 1.0f, 0.0f);
            rb.AddForce(movement * jump_speed);
        } 
        // if single_jump is true andd double_jump is not true, then we still have another jump to use
        // also making the double jump cover more than the single jump :)
        else if (single_jump && !double_jump) 
        {
            double_jump = true;
            Vector3 movement = new Vector3(0.0f, 1.0f, 0.0f);
            rb.AddForce(movement * jump_speed);
        }
    }

    void SetCountText() 
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 12) 
        {
            winTextObject.SetActive(true);
        }
    }

    // every frame, check to make sure the player is in bounds
    void Update() 
    {
        // if the player is out of bounds (based on Y-axis), reset back to inital position and set velocity to 0
        if (OutOfBounds()) 
        {
            rb.velocity = Vector3.zero;
            transform.position = new Vector3(0.0f, 0.5f, 0.0f);
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    private void OnCollisionEnter(Collision other)
    {
        // if we are in contact with the ground, we are not jumping and are grounded
        if (other.gameObject.name == "Ground")
        {
            single_jump = false;
            double_jump = false;
            grounded = true;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);

            count = count + 1;
            SetCountText();
        } 
    }


}
