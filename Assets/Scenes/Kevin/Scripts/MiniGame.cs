using System.Collections;
using UnityEngine;
using System.Linq;

public class MiniGame : MonoBehaviour
{
    [SerializeField] private Transform[] blockPrefabs; // Array to store the 9 prefabs
    [SerializeField] private Transform blockHolder;
    [SerializeField] private TMPro.TextMeshProUGUI livesText;
    [SerializeField] private TMPro.TextMeshProUGUI countdownText;

    private Transform currentBlock = null;
    private Rigidbody2D currentRigidbody;

    private Vector2 blockStartPosition = new Vector2(0f, 4f);

    private float blockSpeed = 8f;
    private float blockSpeedIncrement = 0.5f;
    private int blockDirection = 1;
    private float xLimit = 5;

    private float timeBetweenRounds = 1f;

    private int startingLives = 3;
    private int livesRemaining;
    private bool playing = true;
    private Transform[] sandwichOrder;
    private bool isCheckingSuccess = false;
    private float successTimer = 3f;

    void Start()
    {
        livesRemaining = startingLives;
        livesText.text = $"{livesRemaining}";
        GenerateSandwichOrder();
        SpawnNewBlock();
    }

    private void GenerateSandwichOrder()
    {
        int numIngredients = blockPrefabs.Length - 2; // Excluding top and bottom bun
        Transform[] ingredients = blockPrefabs.Skip(1).Take(numIngredients).OrderBy(x => Random.value).ToArray();

        sandwichOrder = new Transform[startingLives];

        // Ensure the bottom bun is always first
        sandwichOrder[0] = blockPrefabs[0]; // Bottom bun

        // Fill the middle ingredients
        for (int i = 1; i < sandwichOrder.Length - 1; i++)
        {
            sandwichOrder[i] = ingredients[i - 1]; // Ensure no repeats of top and bottom buns
        }

        // Ensure the top bun is always last
        sandwichOrder[sandwichOrder.Length - 1] = blockPrefabs[blockPrefabs.Length - 1]; // Top bun
    }


    private void SpawnNewBlock()
    {
        int index = Mathf.Clamp(sandwichOrder.Length - livesRemaining, 0, sandwichOrder.Length - 1);
        Transform selectedPrefab = sandwichOrder[index];

        currentBlock = Instantiate(selectedPrefab, blockHolder);
        currentBlock.position = blockStartPosition;
        currentRigidbody = currentBlock.GetComponent<Rigidbody2D>();

        blockSpeed += blockSpeedIncrement;
    }

    private IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(timeBetweenRounds);
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
        livesText.text = $"{livesRemaining}";
        if (livesRemaining == 0)
        {
            playing = false;
        }
    }

    private IEnumerator CheckForSuccess()
    {
        isCheckingSuccess = true;
        float timer = successTimer;
        while (timer > 0)
        {
            countdownText.text = "Success in: " + timer.ToString("F1");
            yield return new WaitForSeconds(1f);
            timer -= 1f;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("Kevin");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isCheckingSuccess && other.transform.CompareTag("TopBun"))
        {
            StartCoroutine(CheckForSuccess());
        }
    }
}