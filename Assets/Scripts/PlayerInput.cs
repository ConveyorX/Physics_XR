using ConveyorX.XR.HybridMechs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ConveyorX.XR
{
    public class PlayerInput : MonoBehaviour
    {
        public CameraRotation cameraRotation; //add to camera and assign here for real FPS mode!
        public PlayerMotor playerMotor;
        public JetPack jetPack;

        [Space]
        public InputActionProperty moveInput;
        public InputActionProperty grappleInput;
        public InputActionProperty jumpInput;
        public InputActionProperty interactInput;
        public InputActionProperty secondaryClickInput; //like right clicking on PC
        public InputActionProperty shootAction;

        public float minMoveStepTime = 0.5f;

        private float inputHeldTime;
        private bool cursorLocked;

        private void Start()
        {
            Application.targetFrameRate = 45;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            cursorLocked = true;
        }

        private void Update()
        {
            //Camera Rotation
            if (cameraRotation != null && cursorLocked && cameraRotation.enabled)
            {
                Vector2 mouseMotion = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                cameraRotation.FeedInput(mouseMotion);
            }

            //Vector3 input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("UpDown"), Input.GetAxis("Vertical"));

            Vector2 i = moveInput.action.ReadValue<Vector2>();
            Vector3 input = new Vector3(i.x, Input.GetAxis("UpDown"), i.y);
            
            //Player Movement
            if (playerMotor.enabled) 
            {
                //disabled for XR
                /*if (input.magnitude != 0f)
                {
                    inputHeldTime = minMoveStepTime;
                }
                else if (inputHeldTime > 0f)
                {
                    inputHeldTime -= Time.deltaTime;
                    if (inputHeldTime <= 0f)
                    {
                        input = Vector3.zero;
                        inputHeldTime = 0f;
                    }
                }*/

                playerMotor.FeedInput(new Vector2(input.x, input.z).normalized);

                if (jumpInput.action.WasPressedThisFrame())
                {
                    playerMotor.Jump();
                }
            }

            if (jetPack.Working)
                jetPack.FeedInput(input.normalized);

            //Locking/Unlocking Cursor (PC only)
            if (Input.GetKeyDown(KeyCode.Escape) && cursorLocked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                cursorLocked = false;
            }

            if (Input.GetMouseButton(0) && !cursorLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                cursorLocked = true;
            }
        }
    }
}