using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    void Update()
    {
        UpdateCamera();
    }

    private void UpdateCamera()
    {
        Player player = Player.INSTANCE;
        Camera.main.transform.position = player.transform.position;

        float deltaX = Input.GetAxis("Mouse X");
        float deltaY = Input.GetAxis("Mouse Y");

        var cameraAngles = Camera.main.transform.eulerAngles;
        cameraAngles.x = cameraAngles.x - deltaY * player.sensitivityY;
        cameraAngles.y = cameraAngles.y + deltaX * player.sensitivityX;
        cameraAngles.z = 0;

        cameraAngles.x = cameraAngles.x >= 180 ? cameraAngles.x - 360 : cameraAngles.x;
        cameraAngles.x = Mathf.Clamp(cameraAngles.x, player.minimumX, player.maximumX);

        Camera.main.transform.rotation = Quaternion.Euler(cameraAngles);
    }
}