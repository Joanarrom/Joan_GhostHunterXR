using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CounterUI : MonoBehaviour
{
    public string orbTag = "Orb";
    public string ghostTag = "Ghost";

    public TextMeshProUGUI orbsText;
    public TextMeshProUGUI ghostsText;
    void Update()
    {
        int orbCount = GameObject.FindGameObjectsWithTag(orbTag).Length;
        int ghostCount = GameObject.FindGameObjectsWithTag(ghostTag).Length;

        orbsText.text = $"Orbes: {orbCount}";
        ghostsText.text = $"Fantasmas: {ghostCount}";
    }
}
