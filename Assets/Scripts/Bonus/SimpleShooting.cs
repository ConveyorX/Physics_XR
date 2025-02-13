using ConveyorX.XR;
using UnityEngine;

public class SimpleShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Camera cam;
    public PlayerInput playerInput;

    private float lastShootTime;

    private void Update()
    {
        //right click to shoot (breaking walls)
        if (playerInput.shootAction.action.WasPressedThisFrame() && Time.time >= lastShootTime) 
        {
            ShootWeapon();
            lastShootTime = Time.time + 0.3f;
        }
    }

    private void ShootWeapon() 
    {
        Instantiate(projectilePrefab, cam.transform.position + (cam.transform.forward * 2f), Quaternion.LookRotation(cam.transform.forward));
    }
}
