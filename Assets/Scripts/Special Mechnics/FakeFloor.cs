using UnityEngine;

namespace ConveyorX.XR.HybridMechs
{
    public class FakeFloor : MonoBehaviour
    {
        public GameObject brokenFloorMesh, goodFloorMesh;

        private void Start()
        {
            brokenFloorMesh.SetActive(false);
            goodFloorMesh.SetActive(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player") 
            {
                brokenFloorMesh.SetActive(true);
                goodFloorMesh.SetActive(false);

                CancelInvoke(nameof(EnableGoodFloor));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                CancelInvoke(nameof(EnableGoodFloor));
                Invoke(nameof(EnableGoodFloor), 2f);
            }
        }

        private void EnableGoodFloor() 
        {
            brokenFloorMesh.SetActive(false);
            goodFloorMesh.SetActive(true);
        }
    }
}