using UnityEngine;
using UnityEngine.EventSystems;

public class GameOfLife_2 : MonoBehaviour
{
    public int rows = 20;
    public int columns = 20;
    public float cellSize = 1.0f;
    private bool[,] grid;
    private bool[,] nextGrid;
    private bool isSimulating = false;

    // ����� 'Start' �������������� �����, ���������� ��������� ������ � ����������� ������.
    void Start()
    {
        // ������������� ������ ��� �����
        Camera.main.transform.position = new Vector3((rows - 1) * cellSize / 2, 30, (columns - 1) * cellSize / 2);
        Camera.main.transform.rotation = Quaternion.Euler(90, 0, 0);
        // �������������� ����� ��� �������� �������� � ���������� ��������� ������
        grid = new bool[rows, columns];
        nextGrid = new bool[rows, columns];
        // ���������� ��������� ��������� ����� (��� ������ ������)
        DrawGrid();
    }

    void Update()
{
    // �������� ��������� �� ������� �������
    if (Input.GetKeyDown(KeyCode.Space))
    {
        isSimulating = !isSimulating;
        if (isSimulating)
        {
            InvokeRepeating("UpdateGrid", 1.0f, 0.5f);
        }
        else
        {
            CancelInvoke("UpdateGrid");
        }
    }

    // ��������� ������������ ������� ���� �������� � ��������� ������
    // ��� ����� � ������ ����� ��������� ��� ������� ������ � �����
    if (Input.GetMouseButtonDown(0) && !isSimulating)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            int x = Mathf.FloorToInt(hit.point.x / cellSize);
            int y = Mathf.FloorToInt(hit.point.z / cellSize);
            if (x >= 0 && x < rows && y >= 0 && y < columns)
            {
                grid[x, y] = !grid[x, y];
                DrawGrid();
            }
        }
    }
}

    // ����� 'UpdateGrid' ��������� ��������� ���� ������ � ������������ � ��������� "���� �����".
    void UpdateGrid()
    {
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                // ������������ ���������� ����� ������� ��� ������� ������
                int liveNeighbors = CountLiveNeighbors(x, y);
                // ������� ���������� ��������� ������
                if (grid[x, y])
                {
                    // ����� ������ ��������, ���� � �� 2 ��� 3 ����� ������
                    nextGrid[x, y] = liveNeighbors == 2 || liveNeighbors == 3;
                }
                else
                {
                    // ̸����� ������ �������, ���� � �� ����� 3 ����� ������
                    nextGrid[x, y] = liveNeighbors == 3;
                }
            }
        }

        // ����� �����: ������� ����� ���������� ���������
        bool[,] temp = grid;
        grid = nextGrid;
        nextGrid = temp;

        // ������������� ������� ��������� �����
        DrawGrid();
    }

    // ����� 'CountLiveNeighbors' ������������ ���������� ����� ������� ��� �������� ������.
    int CountLiveNeighbors(int x, int y)
    {
        int count = 0;
        // ���������� ��� �������� ������, ������� ������������
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                // ���������� ���� ������
                if (i == 0 && j == 0) continue;
                int nx = x + i;
                int ny = y + j;
                // ���������, ��� �������� ������ ��������� ������ ������ ����� � ����
                if (nx >= 0 && nx < rows && ny >= 0 && ny < columns && grid[nx, ny])
                {
                    count++;
                }
            }
        }
        return count;
    }

    // ����� 'DrawGrid' �������� �� ������������ �������� ��������� ����� � ���� �������� �� �����.
    void DrawGrid()
    {
        // ������ ��������� ��� ����������� �����
        GameObject plane;
        if (transform.Find("Plane") == null)
        {
            plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.name = "Plane";
            plane.transform.position = new Vector3((rows - 1) * cellSize / 2, -0.1f, (columns - 1) * cellSize / 2);
            plane.transform.localScale = new Vector3(rows / 10.0f, 1, columns / 10.0f);
            plane.GetComponent<Renderer>().material.color = Color.gray;
            plane.transform.parent = transform;
        }
        else
        {
            plane = transform.Find("Plane").gameObject;
        }
        plane.transform.position = new Vector3((rows - 1) * cellSize / 2, -0.1f, (columns - 1) * cellSize / 2);
        plane.transform.localScale = new Vector3(rows / 10.0f, 1, columns / 10.0f);
        plane.GetComponent<Renderer>().material.color = Color.gray;
        plane.transform.parent = transform;
        // ������� ��� ���������� �������-������, ����� �������� �����
        foreach (Transform child in transform)
        {
            if (child.gameObject.name != "Plane")
            {
                Destroy(child.gameObject);
            }
        }

        // ������ ����� �������-������ ��� ����� ������
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                if (grid[x, y])
                {
                    // ������ ��� ��� ������������ ����� ������
                    GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cell.GetComponent<Renderer>().material.color = Color.black;
                    cell.transform.position = new Vector3(x * cellSize, 0, y * cellSize);
                    cell.transform.parent = transform;
                }
            }
        }

    }
}