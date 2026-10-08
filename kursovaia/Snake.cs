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
    public class Snake
    {
        private List<Point> segments = new List<Point>(); // Голова змейки - первый элемент списка

        public IReadOnlyList<Point> Segments => segments;
        public Point Head => segments.First(); // Голова змейки

        public Snake(Point startPosition) // Инициализация змейки с начальной позицией
        {
            segments.Add(startPosition);
        }

        public void Move(Vector direction) // Метод для перемещения змейки
        {
            Point newHead = new Point(Head.X + direction.X, Head.Y + direction.Y); // Вычисление новой позиции головы
            segments.Insert(0, newHead); // Добавление новой головы в начало списка
        }

        public void Grow()
        {
            // змея растёт
        }

        public void Shrink()
        {
            if (segments.Count > 1) // Проверка, что змейка имеет больше одного сегмента
                segments.RemoveAt(segments.Count - 1); // Удаление последнего сегмента змейки
        }

        public bool CollidesWithSelf(Point point) => segments.Contains(point); // Проверка столкновения с собой
    }
}
// тут змея если не ест еду , мы удаляем последний сегмент и тк она двигается то голова добавляется в начало списка и получается эффект движения змейки, а если сьедает еду то мы не удаляем последний сегмент и получается что змейка растет