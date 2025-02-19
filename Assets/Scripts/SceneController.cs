using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.VisualScripting;

public class SceneController : MonoBehaviour
{
    [SerializeField] private int sceneToLoad;
    [SerializeField] private Animator fadeAnimator;

    private void Start()
    {
        StartCoroutine(StartLevelFade());
    }

    public void ReloadScene() {
        int index = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(WaitToLoadScene(index, 2f));
    }

    public void LoadMainMenu() {
        StartCoroutine(WaitToLoadScene(0, 2f));
    }

    public void LoadScene() {
        StartCoroutine(WaitToLoadScene(sceneToLoad, 2f));
    }
 
    public void LoadNextScene() {
        StartCoroutine(WaitToLoadScene(sceneToLoad, 2f));
    }

    private IEnumerator WaitToLoadScene(int sceneIndex, float timeToWait) {
        fadeAnimator.gameObject.SetActive(true);
        fadeAnimator.Play("ExitLevel");
        yield return new WaitForSeconds(timeToWait);
        SceneManager.LoadScene(sceneIndex);
    }

    private IEnumerator StartLevelFade() {
        fadeAnimator.Play("EnterLevel");
        yield return new WaitForSeconds(1f);
        fadeAnimator.gameObject.SetActive(false);
    }
}
