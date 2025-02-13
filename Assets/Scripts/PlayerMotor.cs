using UnityEngine;

namespace ConveyorX.XR
{
    public class PlayerMotor : MonoBehaviour
    {
        public float movementSpeed, runSpeedMultiplier = 2f;
        public float wallMoveStep;
        public float grappleSpeed;

        public Camera cam;
        public Transform bodyRotationHelper; //this is the object that defines the forward/right of the player body controller.
        public GameObject hookObject, lineRenderer;
        public LayerMask grappleLayerMask;

        [Space]
        public float downForce = 10f;
        public float jumpForce = 15f;
        public Transform groundCheckPoint;

        public bool disableMovement = false;
        public bool disableJump = false;
        public bool EnableSuctionCups = false;

        [Space]
        public AudioClip grappleClip;

        public Rigidbody rb { get; private set; }
        private AudioSource source;
        private Vector2 moveInput;
        private Vector3 moveDirection;
        private bool IsGrounded;
        private float lastMoveTime;
        private bool IsRunning;

        private PlayerInput playerInput;

        private bool Grappling;
        private GameObject hook;
        private LineRenderer render;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            source = GetComponent<AudioSource>();
            playerInput = GetComponent<PlayerInput>();
        }

        public void FeedInput(Vector2 input)
        {
            if (!disableMovement)
                moveInput = input;
            else
                moveInput = Vector2.zero;
        }

        private void Update()
        {
            //pc only
            IsRunning = Input.GetKey(KeyCode.LeftShift);
            GroundCheck();

            //suction cups or grappling hook?
            if (playerInput.grappleInput.action.WasPressedThisFrame()) 
            {
                //if it's a wall - use suction cups
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 2f) && hit.collider.tag == "ClimbableWall")
                {
                    disableJump = true;
                    EnableSuctionCups = true;
                    rb.useGravity = false;
                }
                else if(Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit1, 300f, grappleLayerMask, QueryTriggerInteraction.Ignore)) //use grappling hook
                {
                    source.PlayOneShot(grappleClip);

                    Grappling = true;
                    hook = Instantiate(hookObject, hit1.point, Quaternion.identity);
                    render = Instantiate(lineRenderer).GetComponent<LineRenderer>();
                    render.SetPositions(new Vector3[] 
                    {
                        cam.transform.position, hit1.point
                    });
                    rb.useGravity = false;
                }
            }

            //disable grappling
            if (playerInput.grappleInput.action.WasReleasedThisFrame() && Grappling) 
            {
                Destroy(hook);
                Destroy(render.gameObject, 0.1f);
                rb.useGravity = true;
                Grappling = false;
            }

            //space to let go of wall scaling
            if (playerInput.jumpInput.action.WasPressedThisFrame() && EnableSuctionCups) 
            {
                EnableSuctionCups = false;
                rb.useGravity = true;
                disableJump = false;
            }

            //Suction Cups movement on walls
            if (EnableSuctionCups)
            {
                Physics.Raycast(transform.position, transform.forward, out RaycastHit hitWall, 2f);

                if (hitWall.collider == null) 
                {
                    EnableSuctionCups = false;
                    rb.useGravity = true;
                    return;
                }

                moveDirection = Vector3.zero;

                //move by pressing up/down
                if (Time.time >= lastMoveTime) 
                {
                    if (moveInput.y == 1)
                    {
                        Vector3 pos = rb.position;
                        pos.y += wallMoveStep;
                        rb.position = pos;
                        lastMoveTime = Time.time + 0.25f;
                    }

                    if (moveInput.y == -1)
                    {
                        Vector3 pos = rb.position;
                        pos.y -= wallMoveStep;
                        rb.position = pos;
                        lastMoveTime = Time.time + 0.25f;
                    }

                    if (moveInput.x == 1)
                    {
                        Vector3 pos = rb.position;
                        pos.x += wallMoveStep;
                        rb.position = pos;
                        lastMoveTime = Time.time + 0.25f;
                    }

                    if (moveInput.x == -1)
                    {
                        Vector3 pos = rb.position;
                        pos.x -= wallMoveStep;
                        rb.position = pos;
                        lastMoveTime = Time.time + 0.25f;
                    }
                }
            }
            else 
            {
                //handle moving forward/backward and strafe!
                moveDirection = bodyRotationHelper.forward * moveInput.y + bodyRotationHelper.right * moveInput.x;
                moveDirection.Normalize();
            }
        }

        private void FixedUpdate()
        {
            if (EnableSuctionCups)
                return;

            if (Grappling) //grappling movement
            {
                Vector3 dir = (hook.transform.position - transform.position).normalized;
                rb.linearVelocity = dir * grappleSpeed;
                return;
            }

            //apply velocity to rigid-body for player movement
            if (IsGrounded)
            {
                Vector3 velo = movementSpeed * (IsRunning ? runSpeedMultiplier : 1f) * moveDirection;
                velo.y = rb.linearVelocity.y;
                rb.linearVelocity = velo;

            }

            //add a small down force to make player 'stick' to ground
            if (rb.useGravity)
            {
                rb.AddForce(Vector3.down * downForce);
            }
        }

        public void Jump() 
        {
            if (disableJump || !IsGrounded)
                return;

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        private void GroundCheck() 
        {
            IsGrounded = Physics.Raycast(groundCheckPoint.position, Vector2.down, 1f);
        }
    }
}