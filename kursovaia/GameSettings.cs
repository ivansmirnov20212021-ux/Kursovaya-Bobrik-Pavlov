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
    public class GameSettings
    {
        public int BaseInterval { get; set; } // 200, 120, 70 (начальная скорость игры )
        public bool ObstacleMode { get; set; } // режим с препятствиями или классический режим

        public string DifficultyName => BaseInterval switch // выражение switch для получения названия сложности
        {
            200 => "Легко",
            120 => "Средне",
            70 => "Сложно",
            _ => "Неизвестно"
        };

        public string ModeName => ObstacleMode ? "С препятствиями" : "Классический"; // тернарный оператор для получения названия режима
    }
}
