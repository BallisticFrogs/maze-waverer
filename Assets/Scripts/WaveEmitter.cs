using UnityEngine;

public class WaveEmitter : MonoBehaviour
{
    public GameObject wavePrefab;
    public float emissionInterval = 1f;
    public WaveType waveType;
    public bool canAbsorbWaveType;
    public GameObject transparentParts;
    public Material alternativeMaterial;
    public MeshFilter waveEmissionMesh;

    private Material defaultMaterial;
    private float chargeTime = float.MaxValue;
    private Vector3 meshCenter;
    private MeshRenderer[] meshRenderers;

    void Awake()
    {
        // register
        if (LevelController.INSTANCE != null) LevelController.INSTANCE.waveEmitters.Add(this);

        // find all part that can become transparent
        meshRenderers = transparentParts.GetComponentsInChildren<MeshRenderer>();

        // save default material
        defaultMaterial = meshRenderers[0].material;

        // compute wave emission mesh center
        for (int i = 0; i < waveEmissionMesh.mesh.vertexCount; i++)
        {
            meshCenter += waveEmissionMesh.mesh.vertices[i];
        }
        meshCenter /= waveEmissionMesh.mesh.vertexCount;
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
        Vector3[] vertices = waveEmissionMesh.mesh.vertices;
        Vector3 point1 = vertices[Random.Range(0, vertices.Length)];
        Vector3 point2 = vertices[Random.Range(0, vertices.Length)];
        Vector3 point3 = vertices[Random.Range(0, vertices.Length)];
        Vector3 point4 = vertices[Random.Range(0, vertices.Length)];
        Vector3 pos1 = Vector3.Lerp(point1, point2, Random.Range(0f, 1f));
        Vector3 pos2 = Vector3.Lerp(point3, point4, Random.Range(0f, 1f));
        Vector3 pos = Vector3.Lerp(pos1, pos2, Random.Range(0f, 1f));
        pos = waveEmissionMesh.transform.TransformPoint(pos);

        Vector3 transformedCenter = waveEmissionMesh.transform.TransformPoint(meshCenter);
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

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            MeshRenderer currMeshRenderer = meshRenderers[i];
            if (currMeshRenderer.material != expectedMaterial)
            {
                currMeshRenderer.material = expectedMaterial;
            }
        }
    }
}