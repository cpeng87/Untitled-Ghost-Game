using System.Collections;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class MiniGame : MonoBehaviour
{
    [SerializeField] private Transform[] blockPrefabs;
    [SerializeField] private Transform blockHolder;
    [SerializeField] private TMPro.TextMeshProUGUI livesText;

    public Transform currentBlock = null;
    private Rigidbody2D currentRigidbody;
    private Vector2 blockStartPosition = new Vector2(0f, 4f);
    private float blockSpeed = 8f;
    private float blockSpeedIncrement = 0.5f;
    private int blockDirection = 1;
    private float xLimit = 5;
    private float timeBetweenRounds = 1f;
    private int livesRemaining;
    private bool playing = true;
    private Transform[] sandwichOrder;
    private int currentIngredientIndex = 0;
    private bool isCheckingSuccess = false;
    private float successTimer = 3f;
    private bool allIngredientsPlaced = false;
    public static List<bool> check = new List<bool>();


    void Start()
    {
        livesRemaining = 3;
        livesText.text = livesRemaining.ToString();
        GenerateSandwichOrder();
        SpawnNewBlock();
    }

    private void GenerateSandwichOrder()
    {
        sandwichOrder = blockPrefabs.ToArray();
    }

    private void SpawnNewBlock()
    {
        if (currentIngredientIndex >= sandwichOrder.Length)
        {
            allIngredientsPlaced = true;
            return;
        }

        Transform selectedPrefab = sandwichOrder[currentIngredientIndex];
        currentBlock = Instantiate(selectedPrefab, blockHolder);
        currentBlock.position = blockStartPosition;
        currentRigidbody = currentBlock.GetComponent<Rigidbody2D>();
        blockSpeed += blockSpeedIncrement;
    }

    private IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(timeBetweenRounds);
        currentIngredientIndex++;
        SpawnNewBlock();
    }

    void Update()
    {
        if (currentBlock && playing)
        {
            float moveAmount = Time.deltaTime * blockSpeed * blockDirection;
            currentBlock.position += new Vector3(moveAmount, 0, 0);

            if (Mathf.Abs(currentBlock.position.x) > xLimit)
            {
                currentBlock.position = new Vector3(blockDirection * xLimit, currentBlock.position.y, 0);
                blockDirection = -blockDirection;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentBlock = null;
                currentRigidbody.simulated = true;
                StartCoroutine(DelayedSpawn());
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("SandwichGame");
        }
    }

    public void RemoveLife()
    {
        livesRemaining = Mathf.Max(livesRemaining - 1, 0);
        livesText.text = livesRemaining.ToString();
        if (livesRemaining == 0)
        {
            playing = false;
        }
    }

    private void CheckForSuccess()
    {
        bool containsFalse = false;
        for (int j = 0; j < check.Count; j++)
        {
            if (check[j] == false)
            {
                containsFalse = true;
                break;
            }
        }

        if (containsFalse)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("SandwichGame");
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Kevin");
        }
    }

    public void CheckAllIngredientsConnectedTrue()
    {
        check.Add(true);
        CheckForSuccess();
    }
    
    public void CheckAllIngredientsConnectedFalse()
    {
        check.Add(false);
        CheckForSuccess();
    }
}
