using UnityEngine;

namespace ConveyorX.XR.HybridMechs
{
    public class BreakableWall : CharacterStats
    {
        public GameObject brokenWallPrefab;

        public override void Die()
        {
            base.Die();

            //spawn a broken wall object!
            Instantiate(brokenWallPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}