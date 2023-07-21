using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Constant parameters.
    private static int maxAlgae = 500;
    private static int maxRivals = 30;
    public static int MaxAlgae
    {
        get { return maxAlgae; }
    }
    private static int currentAlgae = 0;
    public static int CurrentAlgae
    {
        get { return currentAlgae; }
        set { currentAlgae = value; }
    }
    public static bool IsGameOver { get; set; }
    private static int currentRivalCells = 0;
    public static int CurrentRivals
    {
        get { return currentRivalCells; }
        set { currentRivalCells = value; }
    }
    private static float spawnRange = 70.0f;
    public Button restartButton;
    // List of prefabs to keep track of.
    public List<GameObject> cells;
    // Start is called before the first frame update
    void Start()
    {
        IsGameOver = false;
        SpawnAtRandomLocation(cells[0], maxAlgae);
        SpawnAtRandomLocation(cells[1], maxRivals);
        SpawnAtRandomLocation(cells[2], 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGameOver)
        {
            restartButton.gameObject.SetActive(true);
        }
        // Continuously add more rival cells to the arena.
        if (currentRivalCells < maxRivals)
            SpawnAtRandomLocation(cells[1], 1);
    }

    private void SpawnAtRandomLocation(GameObject cell, int total)
    {
        for (int i = 0; i < total; i++)
        {
            float xPos = Random.Range(-spawnRange, spawnRange);
            float zPos = Random.Range(-spawnRange, spawnRange);
            Vector3 position = new Vector3(xPos, 0.0f, zPos);
            Instantiate(cell, position, cell.gameObject.transform.rotation);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
