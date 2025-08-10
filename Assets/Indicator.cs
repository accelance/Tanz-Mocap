using UnityEngine;

public class Indicator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject attachedDancer;

    public GameObject[] possibleDancers;

    public GameObject Player;

    public Material material;  // Assign in inspector or via script



    public void setDancer(int dancer)
    {
        attachedDancer = possibleDancers[dancer];
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(attachedDancer.transform.position.x, 0, attachedDancer.transform.position.z);
        //material.SetVector("_playerPosition", Player.transform.position);

    }


}
