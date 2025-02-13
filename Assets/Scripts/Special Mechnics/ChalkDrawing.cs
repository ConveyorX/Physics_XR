using UnityEngine;

namespace ConveyorX.XR.HybridMechs
{
    public class ChalkDrawing : MonoBehaviour
    {
        public GameObject chalkDecalPrefab;
        public Camera cam;
        public LayerMask drawingSurfaceMask;
        public PlayerInput playerInput;

        private float lastTime;
        private Transform chalkDecalParent;

        private void Start()
        {
            chalkDecalParent = new GameObject("Chalk Decals").transform;
        }

        private void Update()
        {
            //hold middle mouse button to draw with chalk
            if (playerInput.secondaryClickInput.action.IsPressed() && Time.time >= lastTime)
            {
                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, 10f, drawingSurfaceMask, QueryTriggerInteraction.Ignore))
                {
                    GameObject decal = Instantiate(chalkDecalPrefab, hit.point + hit.normal / 2f, Quaternion.LookRotation(hit.normal), chalkDecalParent);
                    Destroy(decal, 60f); //remove the chalk decal after 1 minute for performance
                }

                lastTime = Time.time + 0.035f;
            }
        }
    }
}