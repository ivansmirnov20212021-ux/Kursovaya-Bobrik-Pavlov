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
    
    public class RecordManager
    {
        private Dictionary<string, int> records = new Dictionary<string, int>(); // словарь для хранения рекордов
        private readonly string filePath = "records.json";// имя файла для хранения рекордов

        public void Load()
        {
            if (File.Exists(filePath)) // проверка существования файла
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    records = JsonSerializer.Deserialize<Dictionary<string, int>>(json) ?? new Dictionary<string, int>(); //превращение JSON обратно в словарь
                }
                catch { records = new Dictionary<string, int>(); } // в случае ошибки инициализация пустого словаря
            }
        }

        public void Save() => File.WriteAllText(filePath, JsonSerializer.Serialize(records, new JsonSerializerOptions { WriteIndented = true })); // сохранение словаря в файл в формате JSON

        public int GetRecord(string key) => records.GetValueOrDefault(key, 0); // получение рекорда по ключу, если ключ не найден возвращается 0

        public void UpdateRecord(string key, int score) // обновление рекорда
        {
            if (score > GetRecord(key))
                records[key] = score;
        }

        public string GetKey(int interval, bool obstacles) => $"{interval}_{(obstacles ? "Obstacles" : "Classic")}"; // генерация ключа для словаря рекордов (тип название) 
    }
}

// будем грубо говоря разбтрать код по кусочкам
// 1 ) создаем класс RecordManager в котором есть словарь для хранения рекордов (public) этим классом можно будет пользоваться из других частей программы
//1,1) тут словарь и слева название режима и сложности справа рекорд
// 2 ) есть метод Load который загружает рекорды из файла records.json
// 3 ) есть метод Save который сохраняет рекорды в файл records.json
// 4 ) есть метод GetRecord который возвращает рекорд по ключу
// 5 ) есть метод UpdateRecord который обновляет рекорд если новый счет больше старого
// 6 ) есть метод GetKey который генерирует ключ для словаря рекордов на основе интервала и наличия препятствий
//** И это все спрятано внутри класса RecordManager что снаружи никто не может напрямую трогать словарь это ИНКАПСУЛЯЦИЯ - скрывает внутренности , и показываешь только то , чем нужно пользоваться  **//