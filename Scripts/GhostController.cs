using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : TileMapMovableObject
{
    private List<Vector3Int> _visited = new List<Vector3Int>(); // список клеток в сетке, которые посетил призрак

    protected override void Start() // переопределяем старт и достаем все данные из родительского класса
    {
        base.Start();
    }

    void FixedUpdate()
    {
        _visited.Add(GridPosition); // после каждого шага заносим нашу текущую позицию сетки в массив
        if (!TryMove()) // если на нашем пути есть препятствия (мы не можем двигаться по текущему направлению)
        {
            var direction = FindDirection(); // меняем направление движения призрака к какой-то непосещенной клетке

            if (direction == Vector3.zero) // если у нас нет направлений для движения
            {
                _visited.Clear(); // очищаем массив с посещенными точками (чтобы можно было заново задать направления для движения)
            }
            else
            {
                SetDirection(direction); // иначе задаем новое направление для движения
            }
        }
    }

    private Vector3Int FindDirection() // перебираем все возможные ходы для призрака (влево-вниз-вверх-вправо)
    {
        for (int x = GridPosition.x - 1; x <= GridPosition.x + 1; x++) // проверяем позиции от -1 до 1 по оси Х
        {
            for (int y = GridPosition.y - 1; y <= GridPosition.y + 1; y++) // проверяем позиции от -1 до 1 по оси Y
            {
                if (x == GridPosition.x && GridPosition.y == y) continue; // если наш x и y совпадает с текущей позицией призрака пропускаем данную итерацию (т.е. это точка в которой мы сейчас стоим)
                if (x != GridPosition.x && y != GridPosition.y) continue; // если наш x и y не совпадает с текущей позицией призрака, то есть пытается двигаться по диагонали, то пропускаем итерацию

                var targetPoint = new Vector3Int(x, y, 0); // запоминаем точку, к которой мы хотим перейти

                if (CanMove(targetPoint) && !_visited.Contains(targetPoint)) // если мы можем перейти к этой точке и мы не были в ней ранее (в массиве отсутствует вектор с данными координатами)
                {
                    return targetPoint - GridPosition; // находим направление к этой точке
                }
            }
        }
        return Vector3Int.zero; // если направление не найдено возвращаем нулевой вектор
    }
}
