using UnityEngine;
using UnityEngine.SceneManagement;  // Importante para recargar escena
using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;

public class OrbSpawner : MonoBehaviour
{
    public static OrbSpawner Instance { get; private set; }

    [Header("Orb Settings")]
    public int numberOfOrbsToSpawn = 5;
    public GameObject orbPrefab;
    public float height = 1.0f;

    [Header("Spawn Control")]
    public int maxNumberOfTry = 100;
    private int currentNumberOfTry = 0;

    [Header("Runtime Spawned Orbs")]
    public List<GameObject> spawnedOrbs = new List<GameObject>();

    [Header("Game Over UI")]
    public GameObject gameOverCanvas;  

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        MRUK.Instance.RegisterSceneLoadedCallback(SpawnOrbs); 
        gameOverCanvas.SetActive(false); 
    }

    public void SpawnOrbs()
    {
        if (MRUK.Instance.Rooms == null || MRUK.Instance.Rooms.Count == 0)
        {
            Debug.LogWarning("No rooms found in MRUK.");
            return;
        }

        MRUKRoom room = MRUK.Instance.Rooms[0];

        for (int i = 0; i < numberOfOrbsToSpawn; i++)
        {
            Vector3 randomPosition = Vector3.zero;
            currentNumberOfTry = 0;
            bool positionFound = false;

            while (currentNumberOfTry < maxNumberOfTry && !positionFound)
            {
                bool hasFound = room.GenerateRandomPositionOnSurface(
                    MRUK.SurfaceType.FACING_UP,
                    1,
                    LabelFilter.Included(MRUKAnchor.SceneLabels.FLOOR), 
                    out randomPosition,
                    out Vector3 normal);

                if (hasFound)
                {
                    bool tooClose = false;
                    foreach (var orb in spawnedOrbs)
                    {
                        if (Vector3.Distance(randomPosition, orb.transform.position) < 1.0f) 
                        {
                            tooClose = true;
                            break;
                        }
                    }

                    if (!tooClose)
                    {
                        positionFound = true;
                    }
                }

                currentNumberOfTry++;
            }

            randomPosition.y = height;

            if (positionFound)
            {
                GameObject spawned = Instantiate(orbPrefab, randomPosition, Quaternion.identity);
                spawnedOrbs.Add(spawned);
            }
            else
            {
                Debug.LogWarning("No valid position found for orb.");
            }
        }
    }

    public void DestroyOrb(GameObject orb)
    {
        spawnedOrbs.Remove(orb);
        Destroy(orb);
        CheckGameEnd();
    }

    void CheckGameEnd()
    {
        if (spawnedOrbs.Count == 0)
        {
            Debug.Log("Game Over! All orbs have been destroyed.");
            gameOverCanvas.SetActive(true);
            StartCoroutine(RestartGameAfterDelay(3f));  // Arranca la corutina para reiniciar el juego
        }
    }

    IEnumerator RestartGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Recarga la escena actual
    }
}