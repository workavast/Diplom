namespace App.Entities
{
    public class EntityIdentifier
    {
        private static int _index;

        private int? _id;
        
        public int Id => _id ??= _index++;
        
        // public static bool operator ==(EntityIdentifier left, int right)
        // {
        //     if (left == null)
        //         return false;
        //
        //     return left._id == right;
        // }
        //
        // public static bool operator !=(EntityIdentifier left, int right) 
        //     => !(left == right);
    }
}