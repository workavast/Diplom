using UnityEngine;

namespace Avastrad.SavingAndLoading
{
    public class PlayerPrefsSaveAndLoader : ISaveAndLoader
    {
        private const string SaveKey = "Save";

        public void Save(object data)
        {
            var save = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(SaveKey, save);
            PlayerPrefs.Save();
        }

        public T Load<T>() 
            where T : new()
        {
            if (!SaveExist())
                return new T();

            var save = PlayerPrefs.GetString(SaveKey);
            
            if (string.IsNullOrEmpty(save))
                return new T();

            return JsonUtility.FromJson<T>(save);
        }

        public bool SaveExist() 
            => PlayerPrefs.HasKey(SaveKey);

        public void DeleteSave()
            => PlayerPrefs.DeleteKey(SaveKey);
    }
}