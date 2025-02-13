using System.Collections;
using UnityEngine;

namespace ConveyorX.XR
{
    public class BlastForce : MonoBehaviour
    {
        public float force;
        public float blastRadius;
        public Vector3 blastPositionOffset;

        private bool Done = false;

        private IEnumerator Start() 
        {
            yield return new WaitForSeconds(1f);
            Done = true;
        }

        private void Update()
        {
            if (Done) return;

            Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);

            for (int i = 0; i < colliders.Length; i++)
            {
                Rigidbody rb = colliders[i].GetComponent<Rigidbody>();
                if (colliders[i].tag != "Player" && rb != null)
                    rb.AddExplosionForce(force, transform.position + blastPositionOffset, blastRadius);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + blastPositionOffset, blastRadius);
        }
    }
}
