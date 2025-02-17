using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{
    [SerializeField] private int sceneToLoad;

    public void ReloadScene() {
        int index = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(WaitToLoadScene(index, 2f));
    }

    public void LoadMainMenu() {
        StartCoroutine(WaitToLoadScene(0, 1f));
    }

    public void LoadScene() {
        StartCoroutine(WaitToLoadScene(sceneToLoad, 1f));
    }
 
    public void LoadNextScene() {
        StartCoroutine(WaitToLoadScene(sceneToLoad, 1f));
    }

    private IEnumerator WaitToLoadScene(int sceneIndex, float timeToWait) {
        yield return new WaitForSeconds(timeToWait);
        SceneManager.LoadScene(sceneIndex);
    }
}
