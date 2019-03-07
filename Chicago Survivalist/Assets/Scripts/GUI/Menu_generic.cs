using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Menu_generic : Panel_generic {

    public void changeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void quitApplication()
    {
        Application.Quit();
    }
}
