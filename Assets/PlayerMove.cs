using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float moveSpeed = 5f;

    float xRotation = 0f;

    bool firstActiveFrame = true;

    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {


        if (Game_Script.Instance.gameActive)
        {
            if (firstActiveFrame)
            {
                firstActiveFrame = false;
                transform.position = new Vector3(0, 1.8f, 0);

            }
            RotateView();
            MovePlayer();
        }
    }

    void RotateView()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // Prevent flipping over

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.parent.Rotate(Vector3.up * mouseX);  // Rotate the parent (player body)
    }

    void MovePlayer()
    {
        Transform playerBody = transform.parent;  // Camera should be child of player body
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = playerBody.right * moveX + playerBody.forward * moveZ;
        playerBody.position += move * moveSpeed * Time.deltaTime;
    }
}
