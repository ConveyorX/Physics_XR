using ConveyorX.XR.Interaction;
using UnityEngine;

namespace ConveyorX.XR.HybridMechs
{
    //simple glider type mech (i.e. fall slowly while allowing motion for player)
    public class LeafGlider : Interactable
    {
        public float glideSpeedX = 5f, glideSpeedY = 10f;
        public float downForce;
        public Vector3 gliderObjectOffset;

        private Rigidbody myRb;
        private Rigidbody playerRb; 
        private PlayerMotor playerMotor;
        private Camera cam;
        private Collider col;
        private bool isGliding = false;

        private Vector3 originalRotation;

        private bool playerAttached;

        public override void Start()
        {
            base.Start();
            playerMotor = playerBody.GetComponent<PlayerMotor>();
            playerRb = playerBody.GetComponent<Rigidbody>();
            col = GetComponent<Collider>();
            myRb = GetComponent<Rigidbody>();
            cam = playerMotor.cam;
            originalRotation = transform.eulerAngles;
        }

        public override void OnInteract()
        {
            base.OnInteract();

            col.isTrigger = true;
            playerAttached = true;
            playerMotor.disableJump = true;
            playerMotor.enabled = false;
            myRb.isKinematic = true;
            StartGliding();
        }

        public void DetachPlayer() 
        {
            col.isTrigger = false;
            myRb.isKinematic = false;
            playerAttached = false;
            playerMotor.disableJump = false;
            playerMotor.enabled = true;
        }

        public override void Update()
        {
            base.Update();

            if (!playerAttached)
            {
                return;
            }

            if (playerInput.jumpInput.action.WasPressedThisFrame() && isGliding) 
            {
                StopGliding();
                DetachPlayer();
            }

            if (isGliding)
            {
                transform.position = playerBody.position + gliderObjectOffset;
                HandleGlideMovement();
            }
        }

        private void StartGliding()
        {
            isGliding = true;
            playerRb.linearDamping = 2f; // Add air resistance
            playerRb.useGravity = false;
        }

        private void StopGliding()
        {
            isGliding = false;
            playerRb.linearDamping = 0f; // Reset air resistance
            playerRb.useGravity = true;
        }

        private void HandleGlideMovement()
        {
            // Allow movement while gliding
            Vector2 i = playerInput.moveInput.action.ReadValue<Vector2>();

            Vector3 moveDirection = new Vector3(i.x, 0, i.y).normalized;
            
            if (moveDirection.z < 0)
            {
                playerRb.linearVelocity += Vector3.up * 0.25f; // Adds slight lift when moving back (lifting face of glider)
                moveDirection.z = 0f;
            }
            else
            {
                //add a slight quickness to decent if player is pressing forward!
                playerRb.linearVelocity -= Vector3.up * downForce * Time.deltaTime * (moveDirection.z > 0 ? 2f : 1);
            }

            // Apply gliding movement - make sure to move in direction of player facing via camera
            Vector3 velocity = playerMotor.bodyRotationHelper.forward * moveDirection.z * glideSpeedY * Time.deltaTime +
                playerMotor.bodyRotationHelper.transform.right * moveDirection.x * glideSpeedX * Time.deltaTime;

            velocity.y = playerRb.linearVelocity.y;
            playerRb.linearVelocity = velocity;

            //rotate the Glider depending on input given
            if (i.y < 0) transform.rotation = Quaternion.Euler(originalRotation + new Vector3(-10, 0, 0));
            else if (i.x > 0) transform.rotation = Quaternion.Euler(originalRotation + new Vector3(0, 0, 10));
            else if (i.x < 0) transform.rotation = Quaternion.Euler(originalRotation + new Vector3(0, 0, -10));
            else if (i.y > 0) transform.rotation = Quaternion.Euler(originalRotation + new Vector3(10, 0, 0));
            else transform.rotation = Quaternion.Euler(originalRotation);
        }
    }
}
