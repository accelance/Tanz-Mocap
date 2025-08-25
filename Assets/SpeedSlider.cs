using UnityEngine;
using UnityEngine.UI;

public class SpeedSlider : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    public void Update_Speed()
    {
        GameManager.Instance.change_Speed(slider.value);
    }
}
