using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EatSpawner : MonoBehaviour
{
    [SerializeField] private Tilemap _levelMap; // карта tilemap
    [SerializeField] private Eat _eatTemplate; // шаблон еды
    [SerializeField] private int _gridSize; // размер сетки

    private Vector3Int _gridPosition; // позиция сетки
    private Transform _selfTransform; // позиция самого объекта EatSpawner (0, 0)

    private List<Eat> eats = new List<Eat>(); // список еды
    private int _eatsCount; // количество еды

    public int EatsCount => _eatsCount;

    private void Awake()
    {
        _selfTransform = GetComponent<Transform>();
        _gridPosition = _levelMap.WorldToCell(_selfTransform.position); // преобразуем текущую позицию объекта в координаты сетки (работаем в её координатах Vector3Int)

        for (int x = _gridPosition.x - _gridSize; x <= _gridPosition.x + _gridSize; x++) // проверяем позиции от -_gridSize до _gridSize по оси Х
        {
            for (int y = _gridPosition.y - _gridSize; y <= _gridPosition.y + _gridSize; y++) // проверяем позиции от -_gridSize до _gridSize по оси Y
            {
                if (x == _gridPosition.x && _gridPosition.y == y) continue; // пропускаем, чтобы еда не спавнилась в середине позиции сетки (там где стоит пакман)

                var targetPoint = new Vector3Int(x, y, 0); // запоминаем точку, к которой должен перейти спавнер

                if (_levelMap.GetTile(targetPoint) is WallTile) // если в этой точке есть препятствие
                    continue; // то пропускаем итерацию
                else // иначе
                {
                    Instantiate(_eatTemplate, new Vector3(targetPoint.x + 0.5f, targetPoint.y + 0.5f, 0), Quaternion.identity, this.transform); // спавним еду в выбранной позиции с оффсетом 0.5f в позиции спавнера
                }
            }
        }

        eats.AddRange(GetComponentsInChildren<Eat>()); // добавляем в массив все дочерние элементы еды после спавна
        _eatsCount = eats.Count; // находим общее количество еды
    }
}
