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
    public class GameEngine
    {
        public Snake Snake { get; }
        public Food Food { get; }
        public List<Point> Obstacles { get; } = new List<Point>();
        public Vector Direction { get; set; } = new Vector(1, 0);
        public bool IsGameOver { get; private set; }

        private int baseInterval;
        private DispatcherTimer timer;

        public event Action OnGameUpdated;
        public event Action<int> OnGameEnded;

        public GameEngine(int interval, bool obstacleMode)
        {
            baseInterval = interval;
            Snake = new Snake(new Point(5, 10));
            Food = new Food();
            Food.Generate(Snake.Segments);

            if (obstacleMode)
                GenerateObstacles();

            SetupTimer();
        }

        private void GenerateObstacles()
        {
            Random rnd = new Random();
            for (int i = 0; i < 15; i++)
            {
                Point p;
                do { p = new Point(rnd.Next(20), rnd.Next(20)); }
                while (Snake.Segments.Contains(p) || (Math.Abs(p.X - 5) <= 2 && Math.Abs(p.Y - 10) <= 2));
                Obstacles.Add(p);
            }
        }

        private void SetupTimer()
        {
            timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(baseInterval) };
            timer.Tick += (s, e) => Update();
            timer.Start();
        }

        private void Update()
        {
            if (IsGameOver) return;

            // === НОВОЕ: проверка клавиш WASD ===
            Vector newDirection = Direction;

            if (Keyboard.IsKeyDown(Key.W)) newDirection = new Vector(0, -1);
            else if (Keyboard.IsKeyDown(Key.S)) newDirection = new Vector(0, 1);
            else if (Keyboard.IsKeyDown(Key.A)) newDirection = new Vector(-1, 0);
            else if (Keyboard.IsKeyDown(Key.D)) newDirection = new Vector(1, 0);

            // Запрет разворота на 180 градусов
            if (newDirection.X + Direction.X != 0 || newDirection.Y + Direction.Y != 0)
                Direction = newDirection;
            // === КОНЕЦ НОВОГО ===

            Point newHead = new Point(Snake.Head.X + Direction.X, Snake.Head.Y + Direction.Y);

            if (newHead.X < 0 || newHead.X >= 20 || newHead.Y < 0 || newHead.Y >= 20 ||
                Snake.CollidesWithSelf(newHead) || Obstacles.Contains(newHead))
            {
                IsGameOver = true;
                timer.Stop();
                OnGameEnded?.Invoke(Snake.Segments.Count - 1);
                return;
            }

            Snake.Move(Direction);

            if (newHead == Food.Position)
            {
                Snake.Grow();
                Food.Generate(Snake.Segments.Concat(Obstacles));
                UpdateSpeed();
            }
            else
            {
                Snake.Shrink();
            }

            OnGameUpdated?.Invoke();
        }

        private void UpdateSpeed()
        {
            int length = Snake.Segments.Count;
            int newInterval = Math.Max(40, baseInterval - (length / 5) * 20);
            timer.Interval = TimeSpan.FromMilliseconds(newInterval);
        }
    }
}