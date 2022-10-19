using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private GameObject columnIconPrefab;

    [SerializeField] private int gridWidth = 5;
    [SerializeField] private int gridHeight = 5;
    [SerializeField] private float nodeSpacing;

    [SerializeField] private NodeSO[] columnColors = new NodeSO[3];
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject congratulationsText;

    [SerializeField] private NodeSO[] startMatrix;
    private GridNode[,] gameMatrix;
    [SerializeField] private Transform nodesFolder;

    // Start is called before the first frame update
    void Start()
    {
        gameMatrix = new GridNode[gridWidth, gridHeight];
        GenerateColumnGoals();
        GenerateGameGrid();
        SetCameraOnGrid();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForSolution();
    }

    public void GenerateGameGrid()
    {
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = gridHeight; j > 0; j--)
            {
                GameObject createdNode = Instantiate(nodePrefab, new Vector2(nodeSpacing * i, nodeSpacing * j), Quaternion.identity, nodesFolder);
                if (startMatrix[gridHeight * i + gridHeight - j] != null)
                {
                    createdNode.GetComponent<GridNode>().nodeSO = startMatrix[gridHeight * i + gridHeight - j];
                    gameMatrix[i, gridHeight - j] = createdNode.GetComponent<GridNode>();
                    createdNode.GetComponent<GridNode>().gridPosition[0] = i;
                    createdNode.GetComponent<GridNode>().gridPosition[1] = gridHeight - j;
                }
            }
        }
    }

    private void SetCameraOnGrid()
    {
        Vector3 centerNodePosition = new Vector3(nodeSpacing * 2, nodeSpacing * 3, -1);
        mainCamera.transform.position = centerNodePosition;
    }

    public void ClearGameGrid()
    {
        foreach (Transform child in nodesFolder.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        congratulationsText.SetActive(false);
    }

    public void GenerateRandomStartMatrix()
    {
        int[] colorsLeftToGenerate = new int[columnColors.Length];
        int randomChoice = Random.Range(0, 3);
        for (int i = 0; i < colorsLeftToGenerate.Length; i++)
        {
            colorsLeftToGenerate[i] = 5;
        }

        for (int i = 0; i < gridWidth; i = i + 2)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                do
                {
                    randomChoice = Random.Range(0, 3);
                } while (colorsLeftToGenerate[randomChoice] == 0);

                startMatrix[(gridHeight * i) + j] = columnColors[randomChoice];
                colorsLeftToGenerate[randomChoice]--;
            }
        }
    }

    private void GenerateColumnGoals()
    {
        for (int i = 0; i < columnColors.Length; i++)
        {
            GameObject createdIcon = Instantiate(columnIconPrefab,
                new Vector2(nodeSpacing * 2 * i, nodeSpacing * (gridHeight + 1)), Quaternion.identity);
            createdIcon.GetComponent<SpriteRenderer>().sprite = columnColors[i].nodeSprite;
        }
    }

    public void SwapWithFreeNode(GridNode nodeToMove, GridNode freeNode)
    {
        gameMatrix[nodeToMove.gridPosition[0], nodeToMove.gridPosition[1]] = freeNode;
        gameMatrix[freeNode.gridPosition[0], freeNode.gridPosition[1]] = nodeToMove;

        int[] temp = nodeToMove.gridPosition;
        nodeToMove.gridPosition = freeNode.gridPosition;
        freeNode.gridPosition = temp;

        Vector2 tempPosition = nodeToMove.transform.position;
        nodeToMove.transform.position = freeNode.transform.position;
        freeNode.transform.position = tempPosition;

    }

    private bool CheckForSolution()
    {
        for (int i = 0; i < gridWidth; i = i + 2)
        {
            NodeType columnTypeGoal = columnColors[i / 2].nodeType;
            for (int j = gridHeight; j > 0; j--)
            {
                if (gameMatrix[i, gridHeight - j].nodeSO.nodeType != columnTypeGoal)
                {
                    return false;
                }
            }
        }
        congratulationsText.SetActive(true);
        return true;
    }

}
