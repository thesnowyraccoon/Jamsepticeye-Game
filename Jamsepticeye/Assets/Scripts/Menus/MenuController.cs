using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
   [Header("Main Menu")]
    public GameObject menuCanvas;

    [Header("Menus")]
    public GameObject[] menus;
    public Button[] buttons;

    void Start()
    {
        menuCanvas.SetActive(true);
        //ActivateMenu(0);
    }

    public void StartGame()
    {
        menuCanvas.SetActive(false);
        //play fade animation
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!menuCanvas.activeSelf && PauseController.isPaused)
            {
                return;
            }

            menuCanvas.SetActive(!menuCanvas.activeSelf);
            PauseController.SetPause(menuCanvas.activeSelf);

            ActivateMenu(0);
        }
    }

    public void ActivateMenu(int menuNo)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            menus[i].SetActive(false);
        }

        //buttons[menuNo].Select();
        menus[menuNo].SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        if (!menuCanvas.activeSelf && PauseController.isPaused)
        {
            return;
        }

        menuCanvas.SetActive(!menuCanvas.activeSelf);
        PauseController.SetPause(menuCanvas.activeSelf);
    }

    public void Back()
    {
        ActivateMenu(0);
    }


}
