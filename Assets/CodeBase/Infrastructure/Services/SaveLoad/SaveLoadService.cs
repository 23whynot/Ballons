using System;
using System.IO;
using CodeBase.Infrastructure.Services.SaveLoad.Data;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private string filePath = Path.Combine(Application.persistentDataPath, "GAMEDATA.json");

        public GameData GameData { get; private set; }

        public UniTask InitializeAsync()
        {
            Debug.Log(Application.persistentDataPath);
            GameData = Load();
            return UniTask.CompletedTask;
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(GameData, Formatting.Indented);

            File.WriteAllText(filePath, json);
        }

        private GameData Load()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                Debug.Log("Загруженный JSON: " + json); 

                try
                {
                    GameData = JsonConvert.DeserializeObject<GameData>(json) ?? new GameData();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Ошибка загрузки GameData: {e.Message}");
                    GameData = new GameData();
                    Save();
                }

                return GameData;
            }
            else
            {
                GameData = new GameData();
                Debug.Log("Файл сохранения не найден, создаётся новый объект данных.");
                return GameData;
            }
        }



        public void Update(GameData gameData)
        {
            GameData = gameData;
            Save();
        }
    }
}