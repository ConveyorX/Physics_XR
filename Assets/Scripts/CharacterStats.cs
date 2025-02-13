using UnityEngine;

namespace ConveyorX.XR
{
    public class CharacterStats : MonoBehaviour
    {
        public int MaxHealth;

        private int currentHealth;
        private bool IsDead;

        private void Start()
        {
            currentHealth = MaxHealth;
            IsDead = false;
        }

        public virtual void TakeDamage(int damage)
        {
            //Already Dead?
            if (IsDead)
                return;

            currentHealth -= damage;

            if (currentHealth <= 0) 
            {
                Die();
            }
        }

        public virtual void Die() 
        {
            IsDead = true;
            currentHealth = 0;
        }
    }
}