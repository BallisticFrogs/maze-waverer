using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player INSTANCE;

    public float sensitivityX = 1f;
    public float sensitivityY = 1f;
    public float minimumX = -60f;
    public float maximumX = 60f;
    public GameObject wavePrefab;

    [HideInInspector] public Base currentBase;

    private readonly List<RaycastResult> uiRaycastHits = new List<RaycastResult>();
    private Button lastHoveredButton;

    void Awake()
    {
        INSTANCE = this;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDestroy()
    {
        INSTANCE = null;
    }

    void Update()
    {
        // update player rotation
        Vector3 look = Camera.main.transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, look.y, 0);

        HandleMenuInteractions();

        HandleTeleportAction();
    }

    public WaveType FindWaveType()
    {
        return currentBase.waveType;
    }

    private void HandleTeleportAction()
    {
        if (uiRaycastHits.Count > 0) return;
        if (currentBase == null) return;

        var fire1 = Input.GetButtonDown("Fire1");
        if (fire1)
        {
            Vector3 rotation = transform.rotation.eulerAngles;
            Quaternion quaternion = Quaternion.Euler(rotation.x, rotation.y, 90);
            EmitWave(quaternion, 0.8f);
        }
    }

    private void HandleMenuInteractions()
    {
        uiRaycastHits.Clear();

        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        pointerEventData.eligibleForClick = true;

        EventSystem.current.RaycastAll(pointerEventData, uiRaycastHits);

        if (uiRaycastHits.Count > 0)
        {
            Button button = uiRaycastHits[0].gameObject.GetComponentInParent<Button>();
            UpdateHoveredButton(button, pointerEventData);

            if (button != null && Input.GetButtonDown("Fire1"))
            {
                // handle click
                button.onClick.Invoke();
            }
        }
        else
        {
            UpdateHoveredButton(null, pointerEventData);
        }
    }

    private void UpdateHoveredButton(Button button, PointerEventData pointerEventData)
    {
        if (lastHoveredButton != null && lastHoveredButton != button)
        {
            lastHoveredButton.OnPointerExit(pointerEventData);
        }
        if (button != null && lastHoveredButton != button)
        {
            button.OnPointerEnter(pointerEventData);
        }
        lastHoveredButton = button;
    }

    public void EmitWave(Quaternion dir, float y)
    {
        Vector3 pos = transform.position;
        pos.y = y;
        GameObject obj = Instantiate(wavePrefab, pos, dir);

        // update wave type
        Wave wave = obj.GetComponent<Wave>();
        wave.waveType = currentBase.waveType;
    }

    public void TeleportToBase(Base baseComponent)
    {
        currentBase = baseComponent;

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
}