using UnityEngine;

public class DistanceColorChanger : MonoBehaviour
{
    [Tooltip("The target GameObject to measure distance from.")]
    public GameObject target;

    [Tooltip("The distance at which the color becomes fully red.")]
    public float maxDistance = 10f;

    private Renderer objRenderer;
    private Material objMaterial;

    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        if (objRenderer != null)
        {
            // Use a copy of the material so we don't change the shared material
            objMaterial = objRenderer.material;
        }
        else
        {
            Debug.LogError("No Renderer found on the GameObject.");
        }

        if (target == null)
        {
            Debug.LogError("Target GameObject not assigned.");
        }
    }

    void Update()
    {
        if (target == null || objMaterial == null)
            return;

        float distance = Vector3.Distance(transform.position, target.transform.position);

        // Normalize distance to a 0-1 range
        float t = Mathf.Clamp01(distance / maxDistance);

        // Lerp from green (close) to red (far)
        Color color = Color.Lerp(Color.green, Color.red, t);

        objMaterial.color = color;
    }
}
