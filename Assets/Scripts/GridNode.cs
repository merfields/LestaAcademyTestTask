using UnityEngine;

public class GridNode : MonoBehaviour
{
    public NodeSO nodeSO;

    public int[] gridPosition = new int[2];

    private void Start()
    {
        Transform picture = transform.Find("Picture");
        picture.GetComponent<SpriteRenderer>().sprite = nodeSO.nodeSprite;
    }
}
