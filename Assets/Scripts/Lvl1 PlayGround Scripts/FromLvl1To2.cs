using UnityEngine;
using UnityEngine.SceneManagement;

public class FromLvl1To2 : MonoBehaviour
{
    // You can remove "string sceneName" if you don't need it
    public void GoToLevel2()
    {
        // Fix: Use the Index number 3 directly
        SceneManager.LoadScene(3);
    }
}