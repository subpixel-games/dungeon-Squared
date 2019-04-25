using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public float speed = 6f;

    Vector3 movement;
    Animator anim;
    int floorMask;
    float camRayLength = 100f;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        floorMask = LayerMask.GetMask("Floor");

       
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Move(moveHorizontal, moveVertical);
        Turning();
        Animating(moveHorizontal, moveVertical);

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
    }
    void Move (float moveHorizontal, float moveVertical)
    {
        movement.Set(moveHorizontal, 0f, moveVertical);

        movement = movement.normalized * speed * Time.deltaTime;

        rb.MovePosition(transform.position + movement);
    }
    void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;

        if(Physics.Raycast (camRay, out floorHit, camRayLength, floorMask))
        {
            Vector3 playertoMouse = floorHit.point - transform.position;
            playertoMouse.y = 0f;
            Quaternion newRotation = Quaternion.LookRotation(playertoMouse);
            rb.MoveRotation(newRotation);
        }
    }
    void Animating(float moveHorizontal, float moveVertical)
    {
        bool walking = moveHorizontal != 0f || moveVertical != 0f;
        anim.SetBool("IsWalking", walking);
    }
}