using UnityEngine;
using TMPro;

public class Dancer_Selection : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    TMP_Dropdown group;
    void Start()
    {
        group = GetComponent<TMP_Dropdown>();
    }

    public void updateDancer()
    {
        Debug.Log(group.value);
        GameManager.Instance.Dancer(group.value.ToString());
    }
}
