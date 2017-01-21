using UnityEngine;

public class Player : MonoBehaviour
{
    public float sensitivityX = 1f;
    public float sensitivityY = 1f;
    public float minimumX = -60f;
    public float maximumX = 60f;
    private float rotationY;


    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Camera.main.transform.position = transform.position;

        float deltaX = Input.GetAxis("Mouse X");
        float deltaY = Input.GetAxis("Mouse Y");

        var cameraAngles = Camera.main.transform.eulerAngles;
        cameraAngles.x = cameraAngles.x - deltaY * sensitivityY;
        cameraAngles.y = cameraAngles.y + deltaX * sensitivityX;
        cameraAngles.z = 0;

        cameraAngles.x = cameraAngles.x >= 180 ? cameraAngles.x - 360 : cameraAngles.x;
        cameraAngles.x = Mathf.Clamp(cameraAngles.x, -60, 60);

        Camera.main.transform.rotation = Quaternion.Euler(cameraAngles);
    }
}