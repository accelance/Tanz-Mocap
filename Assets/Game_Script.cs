using UnityEngine;


public class Game_Script : MonoBehaviour
{
    public static Game_Script Instance;
    public GameObject tanzParent;

    public GameObject indicator;

    public GameObject audioSource;

    public bool gameActive = false; 
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = tanzParent.GetComponent<Animator>();
        Instance = this;
        startDance(0);
    }


    public void startDance(int dancer)
    {
        gameActive = true;

        animator.SetTrigger("Start_Trigger");

        Indicator i = indicator.GetComponent<Indicator>();

        i.setDancer(dancer);

        audioSource.GetComponent<AudioSource>().Play();


    }
}
