using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace kursovaia
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    namespace SnakeGame
    {

        public class Obstacle
        {
            private const int FieldSize = 20;         // Размер поля (20x20)
            private const int ObstacleCount = 15;     // Количество препятствий
            private const int SafeZoneRadius = 4;     // Радиус безопасной зоны вокруг стартовой позиции змейки ** Ничего из этого низя менять это константы**

            private readonly List<Point> _obstacles = new List<Point>(); // Список позиций препятствий

            public IReadOnlyList<Point> Positions => _obstacles;// Возвращает список позиций препятствий для чтения

            public void Generate(Point snakeStartPosition) // Генерирует препятствия, избегая безопасной зоны вокруг змейки
            {
                _obstacles.Clear(); // Очищаем предыдущие препятствия

                Random rnd = new Random(); // Генератор случайных чисел

                for (int i = 0; i < ObstacleCount; i++)
                {
                    Point newObstacle; // Новая позиция препятствия
                    do
                    {
                        newObstacle = new Point(rnd.Next(FieldSize), rnd.Next(FieldSize)); // Случайная позиция в пределах поля
                    }
                    while (
                        _obstacles.Contains(newObstacle) ||                                   // Не дублируем
                        IsInSafeZone(newObstacle, snakeStartPosition) ||                       // Не рядом со змейкой  **делаем пока выполняются эти условия**
                        newObstacle == snakeStartPosition                                      // Не на самой змейке
                    );

                    _obstacles.Add(newObstacle);
                }
            }

            // Проверяет, попадает ли точка в безопасную зону вокруг стартовой позиции змейки

            private bool IsInSafeZone(Point obstaclePos, Point snakeStartPos)
            {
                return Math.Abs(obstaclePos.X - snakeStartPos.X) <= SafeZoneRadius &&
                       Math.Abs(obstaclePos.Y - snakeStartPos.Y) <= SafeZoneRadius;
            }


            // Проверяет, находится ли указанная точка на препятствии
            public bool Contains(Point point)
            {
                return _obstacles.Contains(point);
            }


            // Очищает все препятствия (например, при новой игре без препятствий)

            public void Clear()
            {
                _obstacles.Clear();
            }
        }
    }
}
