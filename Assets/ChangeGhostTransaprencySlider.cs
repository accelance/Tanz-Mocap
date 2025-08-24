using UnityEngine;
using UnityEngine.UI;

public class ChangeGhostTransaprencySlider : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Slider slider;
void Start()
{
    slider = GetComponent<Slider>();
}

// Update is called once per frame
public void Update_Ghost_Transaprency()
{
    GameManager.Instance.change_Ghost_Transparency(slider.value);
}
}
