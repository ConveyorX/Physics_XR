using ConveyorX.XR;
using UnityEngine;

namespace ConveyorX.XR.HybridMechs
{
    public class SpringBoots : MonoBehaviour
    {
        public float jumpMultiplier;
        public Collider playerCollider;
        public PhysicsMaterial bouncyMaterial;

        private PlayerMotor motor;
        private float originalJumpForce;

        private void Awake()
        {
            motor = GetComponent<PlayerMotor>();
            originalJumpForce = motor.jumpForce;
        }

        private void OnEnable()
        {
            playerCollider.material = bouncyMaterial;
            motor.jumpForce = originalJumpForce * jumpMultiplier;
        }

        private void OnDisable()
        {
            playerCollider.material = null;
            motor.jumpForce = originalJumpForce;
        }
    }
}