using System.Collections;
using Meta.XR.MRUtilityKit;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    public static GhostSpawner Instance;

    public float spawnInterval = 1f;
    public GameObject ghostPrefab;
    public int ghostCount = 20;

    public float minEdgeDistance = 0.3f;
    public MRUKAnchor.SceneLabels spawnLabels;
    public float normalOffset = -1.5f;

    public GameObject winCanvas;  

    void Awake()
    {
        Instance = this;
        if (winCanvas != null)
            winCanvas.SetActive(false); 
    }

    void Start()
    {
        StartCoroutine(SpawnGhostsCoroutine());
    }

    private IEnumerator SpawnGhostsCoroutine()
    {
        while (ghostCount > 0)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnGhost();
            ghostCount--;
        }
    }

    private void SpawnGhost()
    {
        MRUKRoom room = MRUK.Instance.GetCurrentRoom();

        int currentTry = 0;
        while (currentTry < 100)
        {
            bool hasFoundPosition = room.GenerateRandomPositionOnSurface(MRUK.SurfaceType.VERTICAL, minEdgeDistance,
                LabelFilter.Included(spawnLabels), out Vector3 pos, out Vector3 norm);
            if (hasFoundPosition)
            {
                Vector3 randomPosition = pos + norm * normalOffset;
                randomPosition.y = 0f;
                Instantiate(ghostPrefab, randomPosition, Quaternion.identity);
                return;
            }
            else
            {
                currentTry++;
            }
        }
    }

    
    /*public void CheckForWin()
    {
        GhostMove[] ghosts = FindObjectsOfType<GhostMove>();
        Debug.Log($"Fantasmas: {ghosts.Length}, ghostCount: {ghostCount}");
        
        // Fuerza ghostCount para test:
        if (ghosts.Length == 0 && ghostCount <= 0)
        {
            winCanvas.SetActive(true);
            Debug.Log("¡WIN ACTIVADO!");
        }
    }*/
}
