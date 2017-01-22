using UnityEngine;

public class WaveEmitter : MonoBehaviour
{
    public GameObject wavePrefab;
    public float emissionInterval = 1f;
    public WaveType waveType;
    public bool canAbsorbWaveType;
    public MeshRenderer meshRenderer;
    public Material alternativeMaterial;

    private Material defaultMaterial;
    private MeshFilter meshFilter;
    private float chargeTime = float.MaxValue;
    private Vector3 meshCenter;

    void Awake()
    {
        meshFilter = GetComponentInChildren<MeshFilter>();
        defaultMaterial = meshRenderer.material;
        if (LevelController.INSTANCE != null) LevelController.INSTANCE.waveEmitters.Add(this);

        for (int i = 0; i < meshFilter.mesh.vertexCount; i++)
        {
            meshCenter += meshFilter.mesh.vertices[i];
        }
        meshCenter /= meshFilter.mesh.vertexCount;
    }

    void Update()
    {
        if (emissionInterval > 0)
        {
            chargeTime += Time.deltaTime;
            if (chargeTime >= emissionInterval)
            {
                chargeTime = 0;

                // create new wave
                EmitWave();
            }
        }
    }

    private void EmitWave()
    {
        Mesh mesh = meshFilter.mesh;
        Vector3[] vertices = mesh.vertices;
        Vector3 point1 = vertices[Random.Range(0, vertices.Length)];
        Vector3 point2 = vertices[Random.Range(0, vertices.Length)];
        Vector3 point3 = vertices[Random.Range(0, vertices.Length)];
        Vector3 point4 = vertices[Random.Range(0, vertices.Length)];
        Vector3 pos1 = Vector3.Lerp(point1, point2, Random.Range(0f, 1f));
        Vector3 pos2 = Vector3.Lerp(point3, point4, Random.Range(0f, 1f));
        Vector3 pos = Vector3.Lerp(pos1, pos2, Random.Range(0f, 1f));
        pos = meshFilter.transform.TransformPoint(pos);

        Vector3 transformedCenter = meshFilter.transform.TransformPoint(meshCenter);
        Vector3 dirVec = (pos - transformedCenter).normalized;
        Quaternion dir = Quaternion.LookRotation(dirVec);

        GameObject obj = Instantiate(wavePrefab, pos, dir);

        // update wave type
        Wave wave = obj.GetComponent<Wave>();
        wave.waveType = waveType;
    }

    public void ChangeWaveType(WaveType waveType)
    {
        this.waveType = waveType;
        UpdateMaterial();
    }

    public void UpdateMaterial()
    {
        WaveType playerCurrentWaveType = Player.INSTANCE.FindWaveType();
        Material expectedMaterial = defaultMaterial;
        if (enabled && waveType == playerCurrentWaveType) expectedMaterial = alternativeMaterial;

        if (meshRenderer.material != expectedMaterial)
        {
            meshRenderer.material = expectedMaterial;
        }
    }
}