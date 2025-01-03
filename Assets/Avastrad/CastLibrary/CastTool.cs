namespace Avastrad.CastLibrary
{
    public static class CastTool
    {
        /// <returns>
        /// return true if value is T, else return false
        /// </returns>
        public static bool CastPossible<T>(this object value) => value is T;
        
        /// <returns>
        /// return T, after conversion from value
        /// </returns>
        public static T Cast<T>(this object value) => (T)value;
        
        /// <returns>
        /// return true if value is T, else return false
        /// </returns>
        public static bool TryCast<T>(this object value, out T outValue)
        {
            if (value.CastPossible<T>())
            {
                outValue = value.Cast<T>();
                return true;
            }
            
            outValue = default;
            return false;
        }
    }
}