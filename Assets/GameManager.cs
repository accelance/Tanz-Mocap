using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Dictionary<string, string> settings;

    public GameObject[] DancerMeshes;

    public GameObject AudioSourceParent;

    public Material[] materials;
    public static GameManager Instance;


    void Start()
    {
        settings = new Dictionary<string, string>();
        settings.Add("Volume", "1.0");
        settings.Add("Dancer", "0");
        settings.Add("Speed", "1");

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        startPlayingMusic();
    }


    public void Dance()
    {
        Game_Script.Instance.startDance(int.Parse(settings["Dancer"]));

    }

    public void Dancer(string dancer)
    {
        Debug.Log(dancer);
        settings["Dancer"] = dancer;


        for (int i = 0; i < DancerMeshes.Length; i++)
        {
            DancerMeshes[i].GetComponent<SkinnedMeshRenderer>().SetMaterials(new List<Material> { ((i == int.Parse(dancer)) ? materials[1] : materials[0]) });
        }
    }

    public void Volume(string volume)
    {
        Debug.Log(volume);

        settings["Volume"] = volume;

        updateVolume(float.Parse(volume));
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

    void updateVolume(float vol)
    {
        for (int i = 0; i < AudioSourceParent.transform.childCount; i++)
        {
            AudioSourceParent.transform.GetChild(i).GetComponent<AudioSource>().volume = vol;
        }
    }






}
