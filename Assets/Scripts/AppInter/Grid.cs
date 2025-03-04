using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;

public class GridRenderer : MonoBehaviour
{
    public int gridSize = 10;
    public float gridStep = 1f;
    public Color gridColor = Color.gray;
    public Color textColor = Color.white;

    public TMP_FontAsset fontAsset;
    private Camera Camera;

    public bool Texst;
    public bool Grid;

    public Vector3 gridOffset;

    private void Start()
    {
        DrawAxes();
        if (Grid) DrawGrid();   
        if (Texst) DrawLabels();

        Camera = Camera.main;
    }

    private void Update()
    {
        foreach (TextMeshPro tmp in FindObjectsOfType<TextMeshPro>())
        {
            tmp.transform.LookAt(Camera.transform);
            tmp.transform.Rotate(0, 180, 0);
        }
        gridOffset = gameObject.transform.position;
    }

    void DrawGrid()
    {
        for (int i = 0; i <= gridSize; i++)
        {
            DrawLine(gridOffset + new Vector3(i * gridStep, 0, 0), gridOffset + new Vector3(i * gridStep, 0, gridSize * gridStep), gridColor);
            DrawLine(gridOffset + new Vector3(0, 0, i * gridStep), gridOffset + new Vector3(gridSize * gridStep, 0, i * gridStep), gridColor);
        }
    }

    void DrawAxes()
    {
        DrawLine(gridOffset, gridOffset + Vector3.right * gridSize * gridStep, Color.red);
        DrawLine(gridOffset, gridOffset + Vector3.up * gridSize * gridStep, Color.blue);
        DrawLine(gridOffset, gridOffset + Vector3.forward * gridSize * gridStep, Color.green);
    }

    void DrawLabels()
    {
        for (int i = 0; i <= gridSize; i++)
        {
            CreateText(i.ToString(), gridOffset + new Vector3(i * gridStep, 0, -0.5f), textColor, 4);
            CreateText(i.ToString(), gridOffset + new Vector3(-0.5f, 0, i * gridStep), textColor, 4);
            CreateText(i.ToString(), gridOffset + new Vector3(-0.5f, i * gridStep, 0), textColor, 4);
        }

        float xyzPos = (gridSize * gridStep) + (gridSize * 0.1f);

        CreateText("X", gridOffset + new Vector3(xyzPos, 0, 0), Color.red, 6);
        CreateText("Z", gridOffset + new Vector3(0, xyzPos, 0), Color.blue, 6);
        CreateText("Y", gridOffset + new Vector3(0, 0, xyzPos), Color.green, 6);
    }

    void CreateText(string text, Vector3 position, Color color, float fontSize)
    {
        GameObject textObj = new GameObject("Label_" + text);
        textObj.transform.position = position;
        TextMeshPro tmp = textObj.AddComponent<TextMeshPro>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.color = color;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.font = fontAsset;
    }

    void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        GameObject line = new GameObject("GridLine");
        LineRenderer lr = line.AddComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
}
