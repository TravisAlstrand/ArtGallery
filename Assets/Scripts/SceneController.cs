using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{
    public void ReloadScene() {
        int index = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(WaitToLoadScene(index, 2f));
    }

    public void LoadMainMenu() {
        StartCoroutine(WaitToLoadScene(0, 1f));
    }

    public void LoadScene(int index) {
        StartCoroutine(WaitToLoadScene(index, 1f));
    }
 
    public void LoadNextScene() {
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        if (index >= SceneManager.loadedSceneCount) {
            index = 0;
        }
        StartCoroutine(WaitToLoadScene(index, 1f));
    }

    private IEnumerator WaitToLoadScene(int sceneIndex, float timeToWait) {
        yield return new WaitForSeconds(timeToWait);
        SceneManager.LoadScene(sceneIndex);
    }
}
