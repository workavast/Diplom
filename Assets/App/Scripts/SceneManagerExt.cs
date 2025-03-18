using UnityEngine.SceneManagement;

namespace App
{
    public static class SceneManagerExt
    {
        public static int GetSceneIndexByName(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                var scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                var sceneNameFromPath = System.IO.Path.GetFileNameWithoutExtension(scenePath);

                if (sceneNameFromPath == sceneName)
                    return i;
            }
            
            return -1;
        }
    }
}
