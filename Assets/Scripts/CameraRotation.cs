using UnityEngine;

namespace ConveyorX.XR
{
    public class CameraRotation : MonoBehaviour
    {
        public float xSpeed, ySpeed;
        public Transform playerBody;

        private float xRot, yRot;

        public void FeedInput(Vector2 input)
        {
            xRot -= input.y * ySpeed * Time.deltaTime;
            xRot = Mathf.Clamp(xRot, -90, 90);
            transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);

            yRot += input.x * xSpeed * Time.deltaTime;
            playerBody.localRotation = Quaternion.Euler(0f, yRot, 0f);
        }
    }
}