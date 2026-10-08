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
    public class Food
    {
        public Point Position { get; private set; } // позиция еды
        private Random random = new Random();
        private const int FieldSize = 20; // Размер поля (20x20)

        public void Generate(IEnumerable<Point> occupiedPositions) // генерация позиции еды, избегая занятых позиций
        {
            Point newPos;
            do
            {
                newPos = new Point(random.Next(FieldSize), random.Next(FieldSize)); // случайная позиция в пределах поля
            } while (occupiedPositions.Contains(newPos)); // проверка на занятость позиции
            Position = newPos; // установка новой позиции еды
        }
    }
}
