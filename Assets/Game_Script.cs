using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class Game_Script : MonoBehaviour
{
    public static Game_Script Instance;

    public float speed = 1.0f;

    public GameObject[] Heads;

    public GameObject tanzParent;

    public GameObject indicator;

    public GameObject audioSource;

    public bool gameActive = false;

    public TMP_Text Scoreboard;

    public float frameRate = 60f;


    public static bool paused = false;

    Indicator i;
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = tanzParent.GetComponent<Animator>();
        Instance = this;
        i = indicator.GetComponent<Indicator>();
        //startDance(0);
    }


    public void startDance(int dancer)
    {
        if (!gameActive)
        {
            gameActive = true;

            animator.SetTrigger("Start_Trigger");


            i.setDancer(dancer);

            StartCoroutine(measure_Average_Distance());
        }

        //audioSource.GetComponent<AudioSource>().Play();
    }

    public void end_dance_callback()
    {

        gameActive = false;
    }

    IEnumerator measure_Average_Distance()
    {
        List<float> distances = new List<float>();


        float averageDistance = 0.0f;


        DistanceColorChanger dcc = indicator.GetComponent<DistanceColorChanger>();

        while (gameActive)
        {
            if (!paused)
            {
                distances.Add(Vector2.Distance(new Vector2(indicator.transform.position.x, indicator.transform.position.z), new Vector2(dcc.target.transform.position.x, dcc.target.transform.position.z)));
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }

        int numberOfMeasurements = distances.Count;

        foreach (float d in distances)
        {
            averageDistance += d / numberOfMeasurements;
        }

        Debug.Log("Average Distance: " + averageDistance);

        Scoreboard.text = averageDistance.ToString() + "\n" + Scoreboard.text;

    }

    public void Pause()
    {
        paused = !paused;
        animator.speed = paused ? 0.0f : speed;
        return;
    }

    public void Reset()
    {
        animator.Rebind();
        animator.Update(0f);
        animator.Play("Idle", 0, 0f);
        gameActive = false;
        paused = false;
        animator.speed = speed;
        //GameManager.Instance.Dance();
    }

    public void change_Speed(float speed2)
    {
        speed = speed2;
    }


    public void StepBack5Frames()
    {
        
        StepFrames(-1);
    }

    public void StepForward5Frames()
    {
        StepFrames(1);
    }

    private void StepFrames(int frameCount)
    {
        if (animator == null) return;

        // Pause Animator so manual stepping works
        animator.speed = 0f;

        int layerCount = animator.layerCount;
        float frameTime = 1f / frameRate; // fraction of animation per frame

        for (int i = 0; i < layerCount; i++)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(i);
            int currentStateHash = stateInfo.shortNameHash;

            // Calculate new normalizedTime
            float newTime = stateInfo.normalizedTime + frameTime * frameCount;

            // Clamp between 0 and 1 to avoid going out of bounds
            newTime = Mathf.Clamp01(newTime);

            // Apply the new time to the current layer
            animator.Play(currentStateHash, i, newTime);
        }

        animator.Update(0f); // Apply changes immediately
    }

}
