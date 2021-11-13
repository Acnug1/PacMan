using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapMovableObject : MonoBehaviour {

    [SerializeField] private Tilemap _levelMap;
    [SerializeField] private Vector3Int _direction;
    [SerializeField] private Vector3Int _gridPosition;
    protected Transform _selfTransform;

    public Vector3Int Direction
    {
        get
        {
            return _direction;
        }
    }

    public Vector3Int GridPosition
    {
        get
        {
            return _gridPosition;
        }
    }

    protected virtual void Start()
    {
        _selfTransform = GetComponent<Transform>();
        SetDirection(1, 0); // зададим начальное направление
        _gridPosition = _levelMap.WorldToCell(_selfTransform.position); // позиция сетки для перемещения будет в позиции объекта, который мы перемещаем (работаем с координатами сетки)
    }

    protected void SetDirection(Vector3Int direction)
    {
        _direction = direction; // устанавливаем новое направление для перемещения
    }

    protected void SetDirection(int x, int y)
    {
        SetDirection(new Vector3Int(x, y, 0)); // задаем направление
    }

    protected bool TryMove()
    {
        Vector3Int newGridPosition = _gridPosition + _direction; // находим позицию сетки при следующем перемещении в данном направлении

        if (!CanMove(newGridPosition)) // если позиция сетки на следующем шаге совпадает с позицией стены
        {
            return false; // то отменяем перемещение в данном направлении
        }

        _gridPosition = newGridPosition; // если препятствий нет задаем новую позицию для сетки перемещения объекта
        _selfTransform.position = (Vector3)_gridPosition + new Vector3(0.5f, 0.5f); // перемещаем сам объект на позицию сетки tilemap и делаем оффсет на 0.5
        return true;
    }

    protected bool CanMove(Vector3Int gridPos)
    {
        if (_levelMap.GetTile(gridPos) is WallTile) // если позиция сетки на следующем шаге совпадает с позицией стены
        {
            return false; // то отменяем перемещение в данном направлении
        }

        return true; // иначе продолжаем перемещение
    }
}
