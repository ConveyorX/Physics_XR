using ConveyorX.XR.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ConveyorX.XR.HybridMechs
{
    public class JetPack : MonoBehaviour
    {
        public float movementSpeed;
        public float MaxFuel = 100;
        public UIManager ui;

        private Rigidbody rb;
        public bool Working { get; set; }

        private float FuelLeft;
        private Vector3 input;
        private Vector3 velocity;
        private PlayerMotor motor;

        private void Start()
        {
            motor = GetComponent<PlayerMotor>();
            rb = GetComponent<Rigidbody>();
            FuelLeft = MaxFuel;
            ui.UpdateUI(FuelLeft, MaxFuel);
        }

        public void FeedInput(Vector3 inp) 
        {
            input = inp;
        }

        private void Update()
        {
            return;

            if (Working)
            {
                FuelLeft -= Time.deltaTime;

                if (FuelLeft <= 0)
                {
                    Working = false;
                }
                ui.UpdateUI(FuelLeft, MaxFuel);
            }

            if (FuelLeft > 0 && Input.GetKeyDown(KeyCode.J))
            {
                Working = !Working;

                if (Working)
                {
                    rb.useGravity = false;
                    motor.enabled = false;
                }
                else
                {
                    motor.enabled = true;
                    rb.useGravity = true;
                }
            }

            if (!Working)
                return;

            //make player maneuver - i.e. move up, down, left, right, forward & back!
            Vector3 velocity = transform.forward * input.z + transform.right * input.x + Vector3.up * input.y;
            rb.linearVelocity = velocity * movementSpeed;
        }
    }
}