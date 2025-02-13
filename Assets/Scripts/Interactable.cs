using UnityEngine;

namespace ConveyorX.XR.Interaction
{
    public class Interactable : MonoBehaviour
    {
        public float interactionRadius = 3f;
        public Transform playerBody;

        public PlayerInput playerInput { get; private set; }

        public virtual void Start() 
        {
            playerInput = playerBody.GetComponent<PlayerInput>();
        }

        //called when player interacts with this object, useful for classes that derive from this class
        public virtual void OnInteract() { }

        public virtual void Update()
        {
            float distance = Vector3.Distance(transform.position, playerBody.position);

            if (distance <= interactionRadius) 
            {
                if (playerInput.interactInput.action.WasPressedThisFrame()) 
                {
                    Debug.Log("Interacting with " + transform.name);
                    OnInteract();
                }
            }
        }

        public virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, interactionRadius);
        }
    }
}
