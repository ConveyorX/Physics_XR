using UnityEngine;
using UnityEngine.InputSystem;

namespace ConveyorX.XR
{
    //Simple logic: just swim in direction of player camera!
    //Mermaid Type!
    public class SwimmingController : MonoBehaviour
    {
        public float swimSpeed = 220f;
        public InputActionProperty MoveInput;
        public Transform cam; //camera object
        public Transform groundCheck;

        public bool NearWaterSurface { get; set; }
        public float waterSurfaceY { get; set; }
        private float myHeight;

        private Rigidbody rb;
        private Vector3 velocity;
        private bool IsGrounded; //touching the sand beneath sea?

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            NearWaterSurface = false;
            waterSurfaceY = 0f;
            myHeight = GetComponent<CapsuleCollider>().height;
        }

        private void Update()
        {
            Vector2 input = MoveInput.action.ReadValue<Vector2>().normalized;
            velocity = cam.forward * input.y * swimSpeed + cam.right * input.x * swimSpeed;

            //make sure to let player half-out on water surface before stopping movement!
            if (NearWaterSurface && velocity.y > 0 && transform.position.y >= waterSurfaceY + myHeight / 2f) 
            {
                //do not allow player to move *up* anymore!
                velocity.y = 0f;
            }

            GroundCheck();
        }

        private void GroundCheck()
        {
            IsGrounded = Physics.Raycast(groundCheck.position, Vector2.down, 1f);
        }

        private void FixedUpdate()
        {
            rb.linearVelocity = velocity;
        }
    }
}