using UnityEngine;

public class DistanceColorChanger : MonoBehaviour
{
    [Tooltip("The target GameObject to measure distance from.")]
    public GameObject target;

    [Tooltip("The distance at which the color becomes fully red.")]
    public float maxDistance = 5f;

    public Material GhostMaterial;

    private Renderer objRenderer;

    public GameObject[] possibleDancers;

    void Start()
    {


        possibleDancers = new GameObject[4];
        int count = 0;
        for (int i = 0; i < GameManager.Instance.tanzParent.transform.childCount; i++)
        {
            GameObject o = GameManager.Instance.tanzParent.transform.GetChild(i).gameObject;
            if (o.name.Contains("Root"))
            {
                Debug.Log(o.name);
                possibleDancers[count] = o.transform.GetChild(0).gameObject;
                count = (count + 1) % 4;
            }
        }

        GameObject[] possibleDancers2 = new GameObject[4];

        possibleDancers2[0] = possibleDancers[3];
        possibleDancers2[1] = possibleDancers[0];
        possibleDancers2[2] = possibleDancers[1];
        possibleDancers2[3] = possibleDancers[2];

        possibleDancers = possibleDancers2;


    }

    void Update()
    {

        Vector3 dancerPosition = possibleDancers[GameManager.Instance.dancer].transform.position;
        if (target == null)
        {
            return;
        }

        float distance = Vector2.Distance(new Vector2(dancerPosition.x, dancerPosition.z), new Vector2(target.transform.position.x, target.transform.position.z));

        // Normalize distance to a 0-1 range
        float t = Mathf.Clamp01(distance / maxDistance);

        // Lerp from green (close) to red (far)
        Color color = Color.Lerp(Color.green, Color.red, t);

        GhostMaterial.SetColor("_BaseColor", new Color(color.r, color.g, color.b, GhostMaterial.color.a));
    }
}
