using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Avastrad.SavingAndLoading
{
    public class BinarySaveAndLoader : ISaveAndLoader
    {
        private const string SaveFileName = "Save";
        private static string SavePath => Path.Combine(Application.dataPath, SaveFileName);

        public void Save(object data)
        {
            using (FileStream stream = new FileStream(SavePath, FileMode.OpenOrCreate))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, data);
            }
        }

        public T Load<T>() where T : new()
        {
            if (!SaveExist())
                return new T();

            T loadedData;
            using (FileStream stream = new FileStream(SavePath, FileMode.Open))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                loadedData = (T) binaryFormatter.Deserialize(stream);
            }
            
            return loadedData;
        }

        public bool SaveExist() 
            => File.Exists(SavePath);

        public void DeleteSave() 
            => File.Delete(SavePath);
    }
}