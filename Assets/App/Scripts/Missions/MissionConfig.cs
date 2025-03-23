using Unity.Collections;
using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif

namespace App.Missions
{
    [CreateAssetMenu(fileName = nameof(MissionConfig), menuName = Consts.AppName + "/Configs/Missions/" + nameof(MissionConfig))]
    public class MissionConfig : ScriptableObject
    {
        [field: SerializeField] public string MissionName { get; private set; } = "None";
#if UNITY_EDITOR
        [field: SerializeField] public SceneAsset Scene { get; private set; }
#endif  
        [field: SerializeField, ReadOnly] public string SceneName { get; private set; }
        
        public int SceneIndex => SceneManagerExt.GetSceneIndexByName(SceneName);
        
#if UNITY_EDITOR
        private void OnValidate() 
            => UpdateSceneName();

        private void OnEnable() 
            => UpdateSceneName();

        private void UpdateSceneName()
        {
            if (Scene != null)
            {
                var newName = Scene.name;
                if (SceneName != newName)
                {
                    SceneName = newName;
                    EditorUtility.SetDirty(this);
                }
            }
        }
#endif
    }
}