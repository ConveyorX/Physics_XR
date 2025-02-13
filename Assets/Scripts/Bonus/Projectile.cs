using ConveyorX.XR;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int Damage = 5;
    public float moveForce = 20f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 10f); //destroy no matter what after 10 seconds
    }

    private void Update()
    {
        rb.AddForce(transform.forward * moveForce);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") 
        {
            //do damage and destroy
            CharacterStats stats = other.GetComponent<CharacterStats>();
            if (stats!=null) 
            {
                stats.TakeDamage(Damage);
            }

            Destroy(gameObject);
        }
    }
}
