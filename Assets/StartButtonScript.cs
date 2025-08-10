using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class StartButtonScript : MonoBehaviour
{
    public GameObject Dropdown;
    public void onPress()
    {
        //Game_Script.Instance.startDance((int)Dropdown.GetComponent<DropdownMenu>().m_Value);
        Debug.Log(Dropdown.GetComponent<DropdownMenu>());
    }
}
