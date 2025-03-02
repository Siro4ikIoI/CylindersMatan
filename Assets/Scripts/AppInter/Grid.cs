using UnityEngine;
using TMPro;

public class GridRenderer : MonoBehaviour
{
    public int gridSize = 10;
    public float gridStep = 1f;
    public Color gridColor = Color.gray;
    public Color textColor = Color.white;

    public TMP_FontAsset fontAsset;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        DrawGrid();
        DrawAxes();
        DrawLabels();
    }

    private void Update()
    {
        foreach (TextMeshPro tmp in FindObjectsOfType<TextMeshPro>())
        {
            tmp.transform.LookAt(mainCamera.transform);
            tmp.transform.Rotate(0, 180, 0);
        }
    }

    void DrawGrid()
    {
        for (int i = 0; i <= gridSize; i++)
        {
            DrawLine(new Vector3(i * gridStep, 0, 0), new Vector3(i * gridStep, 0, gridSize * gridStep), gridColor);
            DrawLine(new Vector3(0, 0, i * gridStep), new Vector3(gridSize * gridStep, 0, i * gridStep), gridColor);
        }
    }

    void DrawAxes()
    {
        DrawLine(Vector3.zero, Vector3.right * gridSize * gridStep, Color.red);
        DrawLine(Vector3.zero, Vector3.forward * gridSize * gridStep, Color.blue);
        DrawLine(Vector3.zero, Vector3.up * gridSize * gridStep, Color.green);
    }

    void DrawLabels()
    {
        for (int i = 0; i <= gridSize; i++)
        {
            CreateText(i.ToString(), new Vector3(i * gridStep, 0, -0.5f));
            CreateText(i.ToString(), new Vector3(-0.5f, 0, i * gridStep));
            CreateText(i.ToString(), new Vector3(-0.5f, i * gridStep, 0));
        }
    }

    void CreateText(string text, Vector3 position)
    {
        GameObject textObj = new GameObject("Label_" + text);
        textObj.transform.position = position;
        TextMeshPro tmp = textObj.AddComponent<TextMeshPro>();
        tmp.text = text;
        tmp.fontSize = 4;
        tmp.color = textColor;
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
