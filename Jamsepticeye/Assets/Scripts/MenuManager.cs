using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Image Rules;
    public Image Credits;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HideRules();
            HideOptions();
        }
    }

    public void Start()
    {
        Rules.gameObject.SetActive(false);
        Credits.gameObject.SetActive(false);
    }

    public void ShowRules()
    {
        Debug.Log("Showing rules");
        Rules.gameObject.SetActive(true);
    }

    public void HideRules()
    {
        Rules.gameObject.SetActive(false);
    }


    public void ShowOptions()
    {
        Credits.gameObject.SetActive(true);
    }

    public void HideOptions()
    {
        Credits.gameObject.SetActive(false);
    }
}
