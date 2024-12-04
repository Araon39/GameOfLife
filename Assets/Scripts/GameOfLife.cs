using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    public int rows = 20;
    public int columns = 20;
    public float cellSize = 1.0f;
    private bool[,] grid;
    private bool[,] nextGrid;

    // Метод 'Start' инициализирует сетку и запускает обновления состояния клеток.
    void Start()
    {
        // Инициализируем сетки для хранения текущего и следующего состояния клеток
        grid = new bool[rows, columns];
        nextGrid = new bool[rows, columns];

        // Заполняем сетку случайными живыми и мёртвыми клетками
        InitializeGrid();

        // Устанавливаем повторяющееся обновление сетки каждые 0.5 секунды
        InvokeRepeating("UpdateGrid", 1.0f, 0.5f);
    }

    // Метод 'InitializeGrid' отвечает за случайную инициализацию клеток в сетке.
    void InitializeGrid()
    {
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                // С вероятностью 70% клетка будет мёртвой, иначе живой
                grid[x, y] = Random.value > 0.7f;
            }
        }
    }

    // Метод 'UpdateGrid' обновляет состояние всех клеток в соответствии с правилами "Игры жизни".
    void UpdateGrid()
    {
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                // Подсчитываем количество живых соседей для текущей клетки
                int liveNeighbors = CountLiveNeighbors(x, y);

                // Правила обновления состояния клетки
                if (grid[x, y])
                {
                    // Живая клетка выживает, если у неё 2 или 3 живых соседа
                    nextGrid[x, y] = liveNeighbors == 2 || liveNeighbors == 3;
                }
                else
                {
                    // Мёртвая клетка оживает, если у неё ровно 3 живых соседа
                    nextGrid[x, y] = liveNeighbors == 3;
                }
            }
        }

        // Обмен сеток: текущая сетка становится следующей
        bool[,] temp = grid;
        grid = nextGrid;
        nextGrid = temp;

        // Визуализируем текущее состояние сетки
        DrawGrid();
    }

    // Метод 'CountLiveNeighbors' подсчитывает количество живых соседей для заданной клетки.
    int CountLiveNeighbors(int x, int y)
    {
        int count = 0;
        // Перебираем все соседние клетки, включая диагональные
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                // Пропускаем саму клетку
                if (i == 0 && j == 0) continue;
                int nx = x + i;
                int ny = y + j;

                // Проверяем, что соседняя клетка находится внутри границ сетки и жива
                if (nx >= 0 && nx < rows && ny >= 0 && ny < columns && grid[nx, ny])
                {
                    count++;
                }
            }
        }
        return count;
    }

    // Метод 'DrawGrid' отвечает за визуализацию текущего состояния сетки в виде объектов на сцене.
    void DrawGrid()
    {
        // Удаляем все предыдущие объекты-клетки, чтобы обновить сцену
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Создаём новые объекты-клетки для живых клеток
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                if (grid[x, y])
                {
                    // Создаём куб для визуализации живой клетки
                    GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cell.transform.position = new Vector3(x * cellSize, 0, y * cellSize);
                    cell.transform.parent = transform;
                }
            }
        }
    }
}