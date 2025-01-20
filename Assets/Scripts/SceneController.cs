using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{
    public void ReloadScene() {
        StartCoroutine(WaitToReloadScene());
    }

    private IEnumerator WaitToReloadScene() {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
