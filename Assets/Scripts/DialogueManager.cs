using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject playerTextBox;
    [SerializeField] private TextMeshProUGUI playerText;
    [SerializeField] private GameObject gfTextBox;
    [SerializeField] private TextMeshProUGUI gfText;
    [SerializeField] private TextMeshProUGUI nextLineText;
    [SerializeField] private string[] dialogueLines;

    private int currentLineIndex = 0;
    private float timeToWait = 2f;

    private PlayerInput playerInput;
    private SceneController sceneController;

    private void Awake()
    {
        playerInput = FindFirstObjectByType<PlayerInput>();
        sceneController = FindFirstObjectByType<SceneController>();
    }

    private void Start()
    {
        playerInput.SwitchToUIMap();
        StartCoroutine(WaitToStartDialogue());
    }

    private void Update()
    {
        TextMeshProUGUI currentText = null;
        GameObject currentTextBox = null;
        if (playerTextBox.activeInHierarchy) {
            currentText = playerText;
            currentTextBox = playerTextBox;
        } else if (gfTextBox.activeInHierarchy) {
            currentText = gfText;
            currentTextBox = gfTextBox;
        }

        if (currentTextBox) {
            if (playerInput.FrameInput.SubmitRelease)
            {
                currentLineIndex++;
            }

            if (currentLineIndex >= dialogueLines.Length)
            {
                playerTextBox.SetActive(false);
                gfTextBox.SetActive(false);
                nextLineText.gameObject.SetActive(false);
                playerInput.SwitchToGameplayMap();
                StartCoroutine(WaitToStartNextScene());
            }
            else
            {
                CheckIfName();
                currentText.text = dialogueLines[currentLineIndex];
            }
        }
    }


    private void ActivateDialogueBox()
    {
        playerText.text = "";
        gfText.text = "";
        CheckIfName();
        nextLineText.gameObject.SetActive(true);
    }

    private void CheckIfName()
    {
        string line = dialogueLines[currentLineIndex];
        if (line.StartsWith("n-"))
        {
            // If line is a "n-[name]" line activate correct arrow
            if (line == "n-GF")
            {
                gfTextBox.gameObject.SetActive(true);
                playerTextBox.gameObject.SetActive(false);
            }
            else if (line == "n-Player")
            {
                gfTextBox.gameObject.SetActive(false);
                playerTextBox.gameObject.SetActive(true);
            }
            else
            {
                gfTextBox.gameObject.SetActive(false);
                playerTextBox.gameObject.SetActive(false);
            }
            currentLineIndex++;
        }
    }

    private IEnumerator WaitToStartNextScene() {
        yield return new WaitForSeconds(timeToWait);
        sceneController.LoadScene();
    }

    private IEnumerator WaitToStartDialogue()
    {
        yield return new WaitForSeconds(timeToWait);
        ActivateDialogueBox();
    }
}
