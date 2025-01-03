using App.Players;
using TMPro;
using UnityEngine;

namespace App.UI
{
    [RequireComponent(typeof(TMP_InputField))]
    public class NickNameUpdater : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<TMP_InputField>().onValueChanged.AddListener(UpdateNickName);
        }

        private static void UpdateNickName(string newNickName)
        {
            PlayerData.NickName = newNickName;
        }
    }
}