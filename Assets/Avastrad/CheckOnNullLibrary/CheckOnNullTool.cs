namespace Avastrad.CheckOnNullLibrary
{
    public static class CheckOnNullTool
    {
        /// <summary>
        /// Method primarily for checking interfaces for null
        /// </summary>
        /// <returns>
        /// return true if value is c# null or Unity null, else return false
        /// </returns>
        public static bool IsAnyNull<T>(this T value) 
            => value == null || ((value is UnityEngine.Object) && (value as UnityEngine.Object) == null);
    }
}