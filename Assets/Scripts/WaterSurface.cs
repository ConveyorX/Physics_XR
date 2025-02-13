using UnityEngine;

namespace ConveyorX.XR
{
    public class WaterSurface : MonoBehaviour
    {
        public SwimmingController swimmingController;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player") 
            {
                swimmingController.NearWaterSurface = true;
                swimmingController.waterSurfaceY = transform.position.y;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                swimmingController.NearWaterSurface = false;
                swimmingController.waterSurfaceY = 0f;
            }
        }
    }
}