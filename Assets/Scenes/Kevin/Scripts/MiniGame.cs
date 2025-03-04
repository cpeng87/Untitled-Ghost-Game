using System.Collections;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class MiniGame : MonoBehaviour
{
    public static MiniGame Instance;

    [SerializeField] private Transform[] blockPrefabs;
    [SerializeField] private Transform blockHolder;
    [SerializeField] private TMPro.TextMeshProUGUI livesText;
    [SerializeField] private TMPro.TextMeshProUGUI countdownText;
    [SerializeField] private TMPro.TextMeshProUGUI successText;
    [SerializeField] private TMPro.TextMeshProUGUI failText;

    public Transform currentBlock = null;
    private Rigidbody currentRigidbody;
    private Vector2 blockStartPosition = new Vector2(0f, 0.5f);
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
    private HashSet<GameObject> connectedIngredients = new HashSet<GameObject>();
    private HashSet<GameObject> placedIngredients = new HashSet<GameObject>();
    private Coroutine successCheckCoroutine;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        livesRemaining = 3;
        livesText.text = livesRemaining.ToString();
        GenerateSandwichOrder();
        SpawnNewBlock();
    }

    public IEnumerator RemoveLife()
    {
        livesRemaining = Mathf.Max(livesRemaining - 1, 0);
        livesText.text = livesRemaining.ToString();

        if (livesRemaining == 0)
        {
            playing = false;
            failText.gameObject.SetActive(true);
            failText.text = "Failed!";
            yield return new WaitForSeconds(2f);
            UnityEngine.SceneManagement.SceneManager.LoadScene("Kevin");
        }
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
            CheckForSuccess();
            return;
        }

        Transform selectedPrefab = sandwichOrder[currentIngredientIndex];
        currentBlock = Instantiate(selectedPrefab, blockHolder);
        currentBlock.position = blockStartPosition;
        currentRigidbody = currentBlock.GetComponent<Rigidbody>();
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
            currentRigidbody.isKinematic = true;
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
                currentRigidbody.isKinematic = false;
                StartCoroutine(DelayedSpawn());
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("SandwichGame");
        }
    }

    public void RegisterIngredientCollision(GameObject ingredient)
    {
        connectedIngredients.Add(ingredient);
        CheckForSuccess();
    }

    public void UnregisterIngredientCollision(GameObject ingredient)
    {
        connectedIngredients.Remove(ingredient);
        CheckForSuccess();
    }

    private void CheckForSuccess()
    {
        if (allIngredientsPlaced && connectedIngredients.Count == placedIngredients.Count)
        {
            if (successCheckCoroutine == null)
            {
                successCheckCoroutine = StartCoroutine(SuccessCheckTimer());
            }
        }
        else
        {
            if (successCheckCoroutine != null)
            {
                StopCoroutine(successCheckCoroutine);
                successCheckCoroutine = null;
                countdownText.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator SuccessCheckTimer()
    {
        if (connectedIngredients.Count == placedIngredients.Count)
        {
            successText.gameObject.SetActive(true);
            successText.text = "Sandwich Complete!";
            yield return new WaitForSeconds(2f);
            UnityEngine.SceneManagement.SceneManager.LoadScene("Kevin");
        }
    }
}
