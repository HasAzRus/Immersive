using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Oxygen
{
    public interface IWritableStorage
    {
        void Write<T>(string key, T value);
    }

    public interface IReadableStorage
    {
        T Read<T>(string key, T defaultValue);
    }
    
    public class GameStorage : IWritableStorage, IReadableStorage
    {
        private readonly BinaryFormatter _formatter;

        private GameData _gameData;

        private readonly string _path;

        public GameStorage(string fileName)
        {
            var directory = Application.persistentDataPath + "/Saves";

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            _path = directory + $"/{fileName}.dat";

            _formatter = new BinaryFormatter();
            var selector = new SurrogateSelector();

            var vector3SerializationSurrogate = new Vector3SerializationSurrogate();
            var quaternionSerializationSurrogate = new QuaternionSerializationSurrogate();

            selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All),
                vector3SerializationSurrogate);

            selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All),
                quaternionSerializationSurrogate);

            _formatter.SurrogateSelector = selector;

            _gameData = new GameData();
        }

        public void Load()
        {
            if (!File.Exists(_path))
            {
                var data = new GameData();

                _gameData = data;

                Save();
            }

            var file = File.Open(_path, FileMode.Open);

            var deserializeData = (GameData)_formatter.Deserialize(file);

            _gameData = deserializeData;

            file.Close();
        }

        public void Save()
        {
            var file = File.Create(_path);

            _formatter.Serialize(file, _gameData);

            file.Close();
        }

        public void Write<T>(string key, T value)
        {
            _gameData.AddValue(key, value);
        }

        public T Read<T>(string key, T valueByDefault)
        {
            return _gameData.GetValue(key, valueByDefault);
        }
    }
}