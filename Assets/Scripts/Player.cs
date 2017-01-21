using UnityEngine;

public class Player : MonoBehaviour
{
    public float sensitivityX = 1f;
    public float sensitivityY = 1f;
    public float minimumX = -60f;
    public float maximumX = 60f;
    private float rotationY;

    private WaveEmitter waveEmitter;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        waveEmitter = GetComponent<WaveEmitter>();
    }

    void Update()
    {
        // move and rotate camera
        UpdateCamera();

        // update player rotation
        Vector3 look = Camera.main.transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, look.y, 0);

        HandleTeleport();
    }

    private void HandleTeleport()
    {
        var fire1 = Input.GetButtonDown("Fire1");
        if (fire1)
        {
            Vector3 rotation = transform.rotation.eulerAngles;
            Quaternion quaternion = Quaternion.Euler(rotation.x, rotation.y, 90);
            waveEmitter.EmitWave(quaternion, 0.2f);

            GameObject hit = FindClosestHit(transform.position, Camera.main.transform.forward);
            if (hit != null)
            {
                Base baseComponent = hit.GetComponentInParent<Base>();
                if (baseComponent != null)
                {
//                    TeleportToBase(baseComponent);
                }
            }
        }
    }

    private void TeleportToBase(Base baseComponent)
    {
        Vector3 pos = transform.position;
        pos.x = baseComponent.transform.position.x;
        pos.z = baseComponent.transform.position.z;
        transform.position = pos;
    }

    private GameObject FindClosestHit(Vector3 pos, Vector3 dir)
    {
        RaycastHit[] hits = Physics.RaycastAll(pos, dir, 50);
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
            return hit.collider.gameObject;
        }
        return null;
    }

    private void UpdateCamera()
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