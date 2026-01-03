using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAgainLvl1 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void RestartLevel()
    {
        SceneManager.LoadScene(2); 
    }
}
