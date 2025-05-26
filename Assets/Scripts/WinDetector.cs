using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinDetector : MonoBehaviour
{
    public string ghostTag = "Ghost";
    public GameObject winCanvas;
    public float timeToWin = 4f;
    public float restartDelay = 3f;

    private float timer = 0f;
    private bool winTriggered = false;

    void Start()
    {
        if (winCanvas != null)
            winCanvas.SetActive(false);
    }

    void Update()
    {
        if (winTriggered)
            return;

        GameObject[] ghosts = GameObject.FindGameObjectsWithTag(ghostTag);

        if (ghosts.Length == 0)
        {
            timer += Time.deltaTime;

            if (timer >= timeToWin)
            {
                Debug.Log("¡WIN por ausencia de fantasmas!");
                if (winCanvas != null)
                    winCanvas.SetActive(true);

                winTriggered = true;
                StartCoroutine(RestartSceneAfterDelay());
            }
        }
        else
        {
            timer = 0f; 
        }
    }

    IEnumerator RestartSceneAfterDelay()
    {
        yield return new WaitForSeconds(restartDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}