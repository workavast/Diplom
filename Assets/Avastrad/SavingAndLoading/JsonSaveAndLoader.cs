using System.IO;
using UnityEngine;

namespace Avastrad.SavingAndLoading
{
    public class JsonSaveAndLoader : ISaveAndLoader
    {
        private readonly string _savePath = Application.persistentDataPath + "/Save.json";
        
        public void Save(object data)
        {
            var save = JsonUtility.ToJson(data);
            using (var writer = new StreamWriter(_savePath)) 
                writer.Write(save);
        }

        public T Load<T>()
            where T : new()
        {
            if (!SaveExist())
                return new T();
                
            var save = "";
            using (var reader = new StreamReader(_savePath)) 
                save += reader.ReadLine();

            if (string.IsNullOrEmpty(save))
                return new T();

            return JsonUtility.FromJson<T>(save);
        }
        
        public bool SaveExist() 
            => File.Exists(_savePath);

        public void DeleteSave()
            => File.Delete(_savePath);
    }
}