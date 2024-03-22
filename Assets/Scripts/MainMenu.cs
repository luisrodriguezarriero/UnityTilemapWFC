using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlaySnake()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        StartCoroutine(_PlayGame(audio.clip.length, 1));
    }
    
    public void Demo()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        StartCoroutine(_QuitGame(audio.clip.length));
    }

    private IEnumerator _PlayGame(float timeToWait, int sceneID)
    {
        yield return new WaitForSeconds(timeToWait);
        SceneManager.LoadScene(sceneID);
    }
    
    private IEnumerator _QuitGame(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        Application.Quit();
    } 
}
