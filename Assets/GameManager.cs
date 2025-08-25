using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Dictionary<string, string> settings;

    public GameObject[] DancerMeshes;

    public GameObject tanzParent;

    public GameObject AudioSourceParent;

    public Material[] materials;

    public TMP_Text Scoreboard;
    public bool gameActive = false;
    public bool paused = false;
    public float frameRate = 60f;

    public float volume = 1.0f;

    public int dancer = 0;

    public float speed = 1.0f;

    List<Material>[] AllMaterials;

    Animator animator;

    void Start()
    {
        animator = tanzParent.GetComponent<Animator>();
        Instance = this;
        DontDestroyOnLoad(this.gameObject);


        AllMaterials = new List<Material>[4];
        

        DancerMeshes = new GameObject[4];
        
        for (int i = 0; i < tanzParent.transform.childCount; i++)
        {
            bool hasMesh = tanzParent.transform.GetChild(i).GetComponent<SkinnedMeshRenderer>() != null;
            if (hasMesh)
            {
                DancerMeshes[i] = tanzParent.transform.GetChild(i).gameObject;
            }
        }

        for (int i = 0; i < 4; i++)
        {
            List<Material> m = new List<Material>();
            DancerMeshes[i].GetComponent<SkinnedMeshRenderer>().GetMaterials(m);
            AllMaterials[i] = m;
        }

        switchDancer(dancer);

        startPlayingMusic();
    }



    public void Dance()
    {
        if (!gameActive)
        {
            int dancer_index = dancer;

            gameActive = true;

            animator.SetTrigger("PlayAll");

            StartCoroutine(measure_Average_Distance());
        }
    }

    public void switchDancer(int new_dancer)
    {
        dancer = new_dancer;
        for (int i = 0; i < DancerMeshes.Length; i++)
        {
            List<Material> m;
            if (i == dancer)
            {
                // Create a new list with the same length, filled with materials[2]
                int count = AllMaterials[i].Count;
                m = new List<Material>();
                for (int j = 0; j < count; j++)
                {
                    m.Add(materials[1]);
                }
            }
            else
            {
                // Use the original materials for other dancers
                m = new List<Material>(AllMaterials[i]);
            }
            DancerMeshes[i].GetComponent<SkinnedMeshRenderer>().SetMaterials(m);
        }
    }


    public void end_dance_callback()
    {
        gameActive = false;
    }



    public void Volume(float new_volume)
    {
        volume = new_volume;

        for (int i = 0; i < AudioSourceParent.transform.childCount; i++)
        {
            AudioSourceParent.transform.GetChild(i).GetComponent<AudioSource>().volume = new_volume;
        }
    }


    public void change_Ghost_Transparency(float value)
    {
        Color baseColor = materials[1].GetColor("_BaseColor");
        materials[1].SetColor("_BaseColor", new Color(baseColor.r, baseColor.g, baseColor.b, value));

    }

    void startPlayingMusic()
    {
        for (int i = 0; i < AudioSourceParent.transform.childCount; i++)
        {
            AudioSourceParent.transform.GetChild(i).GetComponent<AudioSource>().Play();
        }
    }


    public void cycleDancer()
    {
        int currentDancer = dancer;

        currentDancer = (currentDancer + 1) % 4;

        switchDancer(currentDancer);

    }

    IEnumerator measure_Average_Distance()
    {
        List<float> distances = new List<float>();


        float averageDistance = 0.0f;


        DistanceColorChanger dcc = GetComponent<DistanceColorChanger>();

        while (gameActive)
        {
            if (!paused)
            {
                //TODO update measure function
                distances.Add(Vector2.Distance(new Vector2(dcc.possibleDancers[dancer].transform.position.x, dcc.possibleDancers[dancer].transform.position.z), new Vector2(dcc.target.transform.position.x, dcc.target.transform.position.z)));
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

        Scoreboard.text = averageDistance.ToString() + "\n" + Scoreboard.text;

        Debug.Log("Average Distance: " + averageDistance);
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
    }

    public void change_Speed(float new_speed)
    {
        speed = new_speed;
        animator.speed = speed;

    }


    public void StepBack1Frame()
    {

        StepFrames(-1);
    }

    public void StepForward1Frame()
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
