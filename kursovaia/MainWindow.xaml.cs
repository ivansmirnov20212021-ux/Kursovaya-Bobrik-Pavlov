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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameEngine gameEngine;// ссылка на обьект игрового движка
        private RecordManager recordManager = new RecordManager();//  работа с рекордами    ***инкапсуляция - к ним доступ только внутри класса***
        private GameSettings currentSettings;// текущие настройки игры

        public MainWindow()
        {
            InitializeComponent();
            recordManager.Load();
        }

        private void StartGame(GameSettings settings)
        {
            currentSettings = settings;
            gameEngine = new GameEngine(settings.BaseInterval, settings.ObstacleMode); // инициализация игрового движка с выбранными настройками

            gameEngine.OnGameUpdated += Draw; // обработка обновления игры
            gameEngine.OnGameEnded += score => EndGame(score);   // подписка на событие окончания игры (AddRect) ***Принцип делегатов и событий - подписка на события игрового движка***

            StartPanel.Visibility = Visibility.Collapsed;
            SelectedInfo.Text = $"Уровень: {settings.DifficultyName} | Режим: {settings.ModeName}"; // отображение выбранных настроек
            Draw();
        }

        private void Draw()
        {
            GameCanvas.Children.Clear(); // очистка канваса перед перерисовкой

            // препятствия
            foreach (var obs in gameEngine.Obstacles)
            {
                AddRect(obs, Brushes.Gray);
            }

            // змейка
            for (int i = 0; i < gameEngine.Snake.Segments.Count; i++)
            {
                AddRect(gameEngine.Snake.Segments[i], i == 0 ? Brushes.LimeGreen : Brushes.GreenYellow);// голова и тело змейки разных цветов 
            }

            // еда
            AddRect(gameEngine.Food.Position, Brushes.Red);
        }

        private void AddRect(Point pos, Brush color)// добавление прямоугольника на канвас
        {
            var rect = new Rectangle { Width = 20, Height = 20, Fill = color };   // размер и цвет прямоугольника   ***принцип абстракции - скрытие деталей создания прямоугольника в отдельном методе****
            Canvas.SetLeft(rect, pos.X * 20);
            Canvas.SetTop(rect, pos.Y * 20);      // позиционирование на канвасе
            GameCanvas.Children.Add(rect); // добавление на канвас
        }

        private void EndGame(int score)
        {
            string key = recordManager.GetKey(currentSettings.BaseInterval, currentSettings.ObstacleMode);// ключ для записи рекорда
            int best = recordManager.GetRecord(key);

            if (score > best)
            {
                recordManager.UpdateRecord(key, score);
                recordManager.Save();
                MessageBox.Show($"Новый рекорд: {score}!", "Победа!", MessageBoxButton.OK, MessageBoxImage.Asterisk); // сообщение о новом рекорде
            }
            else
            {
                MessageBox.Show($"Игра окончена. Счёт: {score}\nРекорд: {best}", "Конец", MessageBoxButton.OK, MessageBoxImage.Information); // сообщение о конце игры
            }

            StartPanel.Visibility = Visibility.Visible; // возврат к стартовому экрану
        }

        // Кнопки остаются те же
        private void EasyButton_Click(object sender, RoutedEventArgs e) => WaitForMode(200);
        private void MediumButton_Click(object sender, RoutedEventArgs e) => WaitForMode(120);
        private void HardButton_Click(object sender, RoutedEventArgs e) => WaitForMode(70);
        private void WaitForMode(int interval) { currentSettings = new GameSettings { BaseInterval = interval }; SelectedInfo.Text = $"Выбран: {new GameSettings { BaseInterval = interval }.DifficultyName}\nВыберите режим"; } // сохранение сложности и ожидание выбора режима
        private void ClassicMode_Click(object sender, RoutedEventArgs e) => StartGame(new GameSettings { BaseInterval = currentSettings.BaseInterval, ObstacleMode = false });
        private void HardMode_Click(object sender, RoutedEventArgs e) => StartGame(new GameSettings { BaseInterval = currentSettings.BaseInterval, ObstacleMode = true });

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameEngine == null || gameEngine.IsGameOver || StartPanel.Visibility == Visibility.Visible) return; // игнорировать ввод, если игра не активна

            switch (e.Key)
            {
                case Key.Up when gameEngine.Direction.Y != 1: gameEngine.Direction = new Vector(0, -1); break;
                case Key.Down when gameEngine.Direction.Y != -1: gameEngine.Direction = new Vector(0, 1); break; 
                case Key.Left when gameEngine.Direction.X != 1: gameEngine.Direction = new Vector(-1, 0); break; // предотвращение разворота на 180 градусов
                case Key.Right when gameEngine.Direction.X != -1: gameEngine.Direction = new Vector(1, 0); break;
            }
        }
    }
}