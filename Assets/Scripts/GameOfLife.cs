using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    public int rows = 20;
    public int columns = 20;
    public float cellSize = 1.0f;
    private bool[,] grid;
    private bool[,] nextGrid;

    // ����� 'Start' �������������� ����� � ��������� ���������� ��������� ������.
    void Start()
    {
        // �������������� ����� ��� �������� �������� � ���������� ��������� ������
        grid = new bool[rows, columns];
        nextGrid = new bool[rows, columns];

        // ��������� ����� ���������� ������ � ������� ��������
        InitializeGrid();

        // ������������� ������������� ���������� ����� ������ 0.5 �������
        InvokeRepeating("UpdateGrid", 1.0f, 0.5f);
    }

    // ����� 'InitializeGrid' �������� �� ��������� ������������� ������ � �����.
    void InitializeGrid()
    {
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                // � ������������ 70% ������ ����� ������, ����� �����
                grid[x, y] = Random.value > 0.7f;
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
        // ������� ��� ���������� �������-������, ����� �������� �����
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
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
                    cell.transform.position = new Vector3(x * cellSize, 0, y * cellSize);
                    cell.transform.parent = transform;
                }
            }
        }
    }
}