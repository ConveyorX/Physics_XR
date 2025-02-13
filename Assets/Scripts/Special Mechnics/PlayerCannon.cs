using ConveyorX.XR.Interaction;
using UnityEngine;

namespace ConveyorX.XR.HybridMechs
{
    public class PlayerCannon : Interactable
    {
        public float BlastForce = 100.0f;
        public Transform moveOverPosition;

        [Space]
        public AudioClip mountClip;
        public AudioClip launchClip;

        private PlayerMotor playerMotor;
        private Rigidbody playerRb;
        private bool mounted;
        private AudioSource source;

        public override void Start()
        {
            base.Start();

            source = GetComponent<AudioSource>();
            mounted = false;
            playerMotor = playerBody.GetComponent<PlayerMotor>();
            playerRb = playerBody.GetComponent<Rigidbody>();

            moveOverPosition.transform.rotation = Quaternion.Euler(-transform.eulerAngles.x, 0f, 0f);
        }

        public override void OnInteract()
        {
            base.OnInteract();
            MountPlayer();
        }

        public override void Update()
        {
            base.Update();

            if (playerInput.jumpInput.action.WasPressedThisFrame() && mounted) 
            {
                LaunchPlayer();
            }

            if (mounted) 
            {
                playerRb.position = moveOverPosition.position;
                playerRb.linearVelocity = Vector3.zero;
            }
        }

        private void MountPlayer() 
        {
            playerMotor.disableMovement = true;
            playerMotor.disableJump = true;
            playerRb.useGravity = false;
            mounted = true;

            source.PlayOneShot(mountClip);

            playerRb.position = moveOverPosition.position;
            playerRb.linearVelocity = Vector3.zero;

            Quaternion rot = playerBody.transform.rotation;
            rot.x = transform.rotation.x;
            playerBody.rotation = rot;
        }

        //player can launch themselves in direction of cannon!
        private void LaunchPlayer() 
        {
            Quaternion rot = playerBody.transform.rotation;
            rot.x = 0f;
            playerBody.rotation = rot;
            mounted = false;

            source.PlayOneShot(launchClip);

            playerRb.useGravity = true;
            playerMotor.disableMovement = false;
            playerMotor.disableJump = false;

            //here the *Forward* to add force to the player is their *Up* axis
            playerRb.AddForce(playerRb.transform.up * BlastForce, ForceMode.Impulse);
            playerRb.AddForce(moveOverPosition.forward * BlastForce, ForceMode.Impulse);
        }
    }
}