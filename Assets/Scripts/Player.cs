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
        UpdateCamera();

        var fire1 = Input.GetButtonDown("Fire1");
        if (fire1)
        {
            RaycastHit[] hits = Physics.RaycastAll(transform.position, Camera.main.transform.forward, 50);
            float closestHitDist = float.MaxValue;
            RaycastHit? closestHit = null;
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit currHit = hits[i];
                if (currHit.distance < closestHitDist)
                {
                    closestHitDist = currHit.distance;
                    closestHit = currHit;
                }
            }

            if (closestHit.HasValue)
            {
                RaycastHit hit = closestHit.Value;
                Base baseComponent = hit.collider.gameObject.GetComponentInParent<Base>();
                if (baseComponent != null)
                {
                    Vector3 pos = transform.position;
                    pos.x = baseComponent.transform.position.x;
                    pos.z = baseComponent.transform.position.z;
                    transform.position = pos;
                }
            }
        }
    }

    void UpdateCamera()
    {
        Camera.main.transform.position = transform.position;

        float deltaX = Input.GetAxis("Mouse X");
        float deltaY = Input.GetAxis("Mouse Y");

        var cameraAngles = Camera.main.transform.eulerAngles;
        cameraAngles.x = cameraAngles.x - deltaY * sensitivityY;
        cameraAngles.y = cameraAngles.y + deltaX * sensitivityX;
        cameraAngles.z = 0;

        cameraAngles.x = cameraAngles.x >= 180 ? cameraAngles.x - 360 : cameraAngles.x;
        cameraAngles.x = Mathf.Clamp(cameraAngles.x, minimumX, maximumX);

        Camera.main.transform.rotation = Quaternion.Euler(cameraAngles);
    }
}