using UnityEngine;

public class MouseAiming : MonoBehaviour
{
    public GameObject crosshair; // The sprite attached to the mouse position
    public GameObject bulletPrefab; // The bullet prefab
    public Transform bulletSpawnPoint; // The point from which the bullet will be instantiated
    public float bulletSpeed = 10f; // Speed of the bullet
    public float zoomSpeed = 2f; // Speed at which the camera zooms
    public float zoomInSize = 3f; // Orthographic size when zoomed in
    public float zoomOutSize = 5f; // Orthographic size when zoomed out
    public float cameraFollowSpeed = 2f; // Speed at which the camera follows the mouse
    public float focusWeight = 0.5f; // Weight for the focus point between the player and the crosshair (0 to 1)
    public float maxCameraOffset = 2f; // Maximum offset for the camera when zooming in

    private Camera mainCamera;
    private bool isZoomingIn = false;
    private Vector3 initialMousePosition;
    private Vector3 initialCameraPosition;

    void Start()
    {
        mainCamera = Camera.main;
        Cursor.visible = false; // Hide the default cursor
        initialCameraPosition = mainCamera.transform.localPosition; // Initial camera position relative to player
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f; // Distance from the camera to the object
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        crosshair.transform.position = worldPosition;

        if (Input.GetMouseButtonDown(1)) // Right mouse button just pressed
        {
            isZoomingIn = true;
            initialMousePosition = worldPosition - transform.position; // Mouse position relative to player
        }

        if (Input.GetMouseButtonUp(1)) // Right mouse button released
        {
            isZoomingIn = false;
        }

        if (Input.GetMouseButton(1)) // Right mouse button held down
        {
            if (Input.GetMouseButtonDown(0)) // Left mouse button clicked
            {
                ShootBullet(worldPosition);
            }
        }

        SmoothZoom();
        UpdateCameraPosition(worldPosition - transform.position);
    }

    void ShootBullet(Vector3 targetPosition)
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        bullet.SetActive(true);

        Vector3 direction = (targetPosition - bulletSpawnPoint.position).normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
    }

    void SmoothZoom()
    {
        float targetSize = isZoomingIn ? zoomInSize : zoomOutSize;
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);
    }

    void UpdateCameraPosition(Vector3 relativeMousePosition)
    {
        if (isZoomingIn) {
            Vector3 mouseOffset = relativeMousePosition - initialMousePosition;
            Vector3 cameraOffset = Vector3.ClampMagnitude(mouseOffset * focusWeight, maxCameraOffset);
            mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, initialCameraPosition + cameraOffset, Time.deltaTime * cameraFollowSpeed);
        }
        else {
        mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, initialCameraPosition, Time.deltaTime * cameraFollowSpeed);
        }
    }
}
