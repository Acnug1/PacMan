using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Animator))]

public class PacManController : TileMapMovableObject
{
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private EatSpawner _eatSpawner;
    private Animator _animator;
    private int _score;

    public event UnityAction GameOver;
    public event UnityAction<int> ScoreChanged;

    protected override void Start() // переопределяем старт и достаем все данные из родительского класса
    {
        base.Start();
        _animator = GetComponent<Animator>();
    }

    public void MoveUp() // задаем направление для перемещения с помощью кнопок на экране (вызываем событие при нажатии)
    {
        SetDirection(0, 1);
    }

    public void MoveDown()
    {
        SetDirection(0, -1);
    }

    public void MoveRight()
    {
        SetDirection(1, 0);
    }

    public void MoveLeft()
    {
        SetDirection(-1, 0);
    }

    private void ApplyDirection() // применяем направление для пакмана
    {
        if (Direction.x >= 0) // если его позиция перемещения больше или равна нулю
        {
            _sprite.flipX = true; // отражаем его спрайт по оси Х
        }
        else
        {
            _sprite.flipX = false; // иначе оставляем все как есть
        }

        //UP - 0, 1, 0
        //1) 0, 1, 0 ==  90
        //2) 0, -1, 0 == -90
        //3) 0, 0, 0 == 0
        _selfTransform.rotation = Quaternion.Euler(0, 0, 90f * Direction.y); // если пакман движется вверх или вниз задаем угол поворота пакмана по оси Z на -90, 0 или 90
    }

    private void FixedUpdate()
    {
        ApplyDirection(); // применяем выбранное направление
        TryMove(); // пытаемся сделать движение по выбранному направлению
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out GhostController ghostController)) // если пакман сталкивается с призраком
        {
            SetDirection(0, 0); // он останавливается
            _animator.SetTrigger("Death");
            StartCoroutine(DeathPacman());
        }

        if (collision.TryGetComponent(out Eat eat)) // если пакман сталкивается с едой
        {
            Destroy(eat.gameObject); // еда уничтожается
            _score++; // увеличиваем количество очков
            ScoreChanged?.Invoke(_score); // вызываем событие для отображения очков

            if (_score >= _eatSpawner.EatsCount)
                GameOver?.Invoke();
        }
    }

    private IEnumerator DeathPacman()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1.5f);

        yield return waitForSeconds;

        GameOver?.Invoke(); // выходит окно окончания игры
    }
}
