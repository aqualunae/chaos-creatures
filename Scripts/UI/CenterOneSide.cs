using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class CenterOneSide : MonoBehaviour
{
    public enum XAxis
    {
        Left,
        Right
    }

    [SerializeField]
    private XAxis anchoredSide;

    [SerializeField]
    private int xOffset;

    private void OnEnable()
    {
        // find the transform and canvas size
        Vector2 canvasSize = GetComponentInParent<Canvas>().renderingDisplaySize;
        RectTransform transform = GetComponent<RectTransform>();

        // calculate the margin
        Debug.Log(canvasSize.x / 2);
        int sideMargin = (int)(canvasSize.x / 2) + xOffset;
        Debug.Log(sideMargin);

        // apply the margin to the transform
        if (anchoredSide == XAxis.Left)
        {
            transform.offsetMin = new Vector2(sideMargin, transform.offsetMin.y);
            Debug.Log(transform.offsetMin);
        }
        else
        {
            transform.offsetMax = new Vector2(sideMargin * -1, transform.offsetMax.y);
            Debug.Log(transform.offsetMax);
        }
    }
}
