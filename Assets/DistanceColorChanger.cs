using UnityEngine;

public class DistanceColorChanger : MonoBehaviour
{
    [Tooltip("The target GameObject to measure distance from.")]
    public GameObject target;

    [Tooltip("The distance at which the color becomes fully red.")]
    public float maxDistance = 5f;

    public Material GhostMaterial;

    private Renderer objRenderer;
    private Material objMaterial;

    public GameObject[] possibleDancers;

    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        if (objRenderer != null)
        {
            objMaterial = objRenderer.material;
        }     
    }

    void Update()
    {
        Vector3 dancerPosition = possibleDancers[GameManager.Instance.dancer].transform.position;
        if (target == null || objMaterial == null)
        {
            return;
        }

        float distance = Vector2.Distance(new Vector2(dancerPosition.x, dancerPosition.z), new Vector2(target.transform.position.x, target.transform.position.z));

        // Normalize distance to a 0-1 range
        float t = Mathf.Clamp01(distance / maxDistance);

        // Lerp from green (close) to red (far)
        Color color = Color.Lerp(Color.green, Color.red, t);

        objMaterial.color = color;
        GhostMaterial.SetColor("_BaseColor", new Color(color.r, color.g, color.b, GhostMaterial.color.a));
    }
}
