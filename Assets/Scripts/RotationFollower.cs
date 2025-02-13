using UnityEngine;

namespace ConveyorX.XR
{
    public class RotationFollower : MonoBehaviour
    {
        public Transform toFollow;
        public bool followX = true;
        public bool followY = true;
        public bool followZ = true;

        private void LateUpdate()
        {
            Vector3 eulerAngles = toFollow.eulerAngles;
            if (!followX) eulerAngles.x = 0f;
            if (!followY) eulerAngles.y = 0f;
            if (!followZ) eulerAngles.z = 0f;
            transform.rotation = Quaternion.Euler(eulerAngles);
        }
    }
}