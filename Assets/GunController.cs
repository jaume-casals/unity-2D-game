using UnityEngine;

public class GunController : MonoBehaviour
{
    public Transform player; // Reference to the player GameObject
    public Transform bulletSpawnPoint; // Reference to the bullet spawn point
    public float orbitDistance = 1.0f; // Distance from the player to the gun

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(mainCamera.transform.position.z); // Ensure the distance from the camera to the object is correct
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

        Vector3 direction = worldPosition - player.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Calculate the gun position based on the orbit distance
        Vector3 gunPosition = player.position + (direction.normalized * orbitDistance);

        // Set the gun's position and rotation
        transform.position = gunPosition;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Set the bullet spawn point to the gun's position
        bulletSpawnPoint.position = transform.GetChild(0).position; // Assuming the bulletSpawnPoint is the first child of the gun
    }
}
