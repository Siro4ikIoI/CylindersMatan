using UnityEngine;
using TMPro;

public class GridRenderer : MonoBehaviour
{
    public int gridSize = 10;
    public float gridStep = 1f;
    public Color gridColor = Color.gray;
    public Color textColor = Color.white;
    public TMP_FontAsset fontAsset;

    public bool showText = true;
    public bool showGrid = true;

    public Vector3 gridOffset;

    private Camera mainCamera;
    private GameObject gridLabelsParent;
    private TextMeshPro[] gridLabels;
    private LineRenderer[] gridLines;

    private void Start()
    {
        mainCamera = Camera.main;

        gridLabelsParent = new GameObject("GridLabels");
        gridLabelsParent.transform.SetParent(transform);

        CreateLine(gridOffset, gridOffset + Vector3.right * gridSize * gridStep, Color.red);
        CreateLine(gridOffset, gridOffset + Vector3.up * gridSize * gridStep, Color.blue);
        CreateLine(gridOffset, gridOffset + Vector3.forward * gridSize * gridStep, Color.green);

        if (showGrid) DrawGrid();
        if (showText) DrawLabels();
    }

    private void Update()
    {
        if (showText && gridLabels != null)
        {
            foreach (var tmp in gridLabels)
            {
                tmp.transform.LookAt(mainCamera.transform);
                tmp.transform.Rotate(0, 180, 0);
            }
        }
    }

    void DrawGrid()
    {
        int totalLines = (gridSize + 1) * 2;
        gridLines = new LineRenderer[totalLines];

        for (int i = 0; i <= gridSize; i++)
        {
            gridLines[i] = CreateLine(gridOffset + new Vector3(i * gridStep, 0, 0),
                                      gridOffset + new Vector3(i * gridStep, 0, gridSize * gridStep), gridColor);

            gridLines[i + gridSize + 1] = CreateLine(gridOffset + new Vector3(0, 0, i * gridStep),
                                                     gridOffset + new Vector3(gridSize * gridStep, 0, i * gridStep), gridColor);
        }
    }

    void DrawLabels()
    {
        gridLabels = new TextMeshPro[(gridSize + 1) * 3 + 3]; // x, y, z + оси
        int index = 0;

        for (int i = 0; i <= gridSize; i++)
        {
            gridLabels[index++] = CreateText(i.ToString(), gridOffset + new Vector3(i * gridStep, 0, -0.5f), textColor);
            gridLabels[index++] = CreateText(i.ToString(), gridOffset + new Vector3(-0.5f, 0, i * gridStep), textColor);
            gridLabels[index++] = CreateText(i.ToString(), gridOffset + new Vector3(-0.5f, i * gridStep, 0), textColor);
        }

        float xyzPos = (gridSize * gridStep) + (gridSize * 0.1f);

        gridLabels[index++] = CreateText("X", gridOffset + new Vector3(xyzPos, 0, 0), Color.red);
        gridLabels[index++] = CreateText("Z", gridOffset + new Vector3(0, xyzPos, 0), Color.blue);
        gridLabels[index++] = CreateText("Y", gridOffset + new Vector3(0, 0, xyzPos), Color.green);
    }

    TextMeshPro CreateText(string text, Vector3 position, Color color)
    {
        GameObject textObj = new GameObject("Label_" + text);
        textObj.transform.position = position;
        textObj.transform.SetParent(gridLabelsParent.transform);

        TextMeshPro tmp = textObj.AddComponent<TextMeshPro>();
        tmp.text = text;
        tmp.fontSize = 4;
        tmp.color = color;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.font = fontAsset;

        return tmp;
    }

    LineRenderer CreateLine(Vector3 start, Vector3 end, Color color)
    {
        GameObject lineObj = new GameObject("GridLine");
        lineObj.transform.SetParent(transform);

        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        return lr;
    }
}
