using UnityEngine;


public class Controller : MonoBehaviour
{
    [SerializeField] private Grid grid;
    private GridNode highlightedNode = null;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Transform clickedObject = GetClickedObject();

            if (clickedObject?.GetComponent<GridNode>() != null)
            {
                GridNode clickedNode = clickedObject.GetComponent<GridNode>();

                if (highlightedNode == null)
                {
                    SelectNodeToMove(clickedNode);
                }
                else
                {
                    MoveNode(clickedNode);
                }
            }
            else
            {
                DehighlightNode();
            }
        }
    }

    private static Transform GetClickedObject()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePosition2D = new Vector2(mousePosition.x, mousePosition.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePosition2D, Vector2.zero);
        Transform clickedObject = hit.transform;
        return clickedObject;
    }

    private void MoveNode(GridNode clickedNode)
    {
        if (clickedNode.nodeSO.nodeType != NodeType.Free)
        {
            Debug.Log("This node is taken");
            DehighlightNode();
            return;
        }
        else
        {
            //if (Mathf.Abs(highlightedNode.gridPosition[0] - clickedNode.gridPosition[0]) <= 1 && Mathf.Abs(highlightedNode.gridPosition[1] - clickedNode.gridPosition[1]) <= 1 &&
            // !(Mathf.Abs(highlightedNode.gridPosition[1] - clickedNode.gridPosition[1]) == 1 && Mathf.Abs(highlightedNode.gridPosition[0] - clickedNode.gridPosition[0]) == 1))
            float distanceBetweenNodes = CalculateNodesDistance(clickedNode);

            //Сheck for free node to be adjecent to the selected node
            if (distanceBetweenNodes == 1)
            {
                grid.SwapWithFreeNode(highlightedNode, clickedNode);
                DehighlightNode();
            }
            else
            {
                DehighlightNode();
            }
        }
    }

    private float CalculateNodesDistance(GridNode clickedNode)
    {
        int distanceBetweenNodesX = highlightedNode.gridPosition[0] - clickedNode.gridPosition[0];
        int distanceBetweenNodesY = highlightedNode.gridPosition[1] - clickedNode.gridPosition[1];
        float distanceBetweenNodes = Mathf.Sqrt(Mathf.Pow(distanceBetweenNodesX, 2) + Mathf.Pow(distanceBetweenNodesY, 2));
        return distanceBetweenNodes;
    }

    private void SelectNodeToMove(GridNode clickedNode)
    {
        NodeType selectedNodeType = clickedNode.nodeSO.nodeType;
        if (selectedNodeType == NodeType.Free || selectedNodeType == NodeType.Blocked)
        {
            return;
        }
        else
        {
            HighlightNode(clickedNode);
        }
    }

    private void HighlightNode(GridNode clickedNode)
    {
        Color highlightColor = new Color(0.384167f, 0.9150943f, 0.5681103f, 1);
        clickedNode.gameObject.GetComponent<SpriteRenderer>().color = highlightColor;
        highlightedNode = clickedNode;
    }

    private void DehighlightNode()
    {
        if (highlightedNode != null)
        {
            highlightedNode.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            highlightedNode = null;
        }
    }

    public void OnGenereteLevelButtonClicked()
    {
        grid.ClearGameGrid();
        grid.GenerateRandomStartMatrix();
        grid.GenerateGameGrid();
    }
}
