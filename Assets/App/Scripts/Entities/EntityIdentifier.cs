namespace App.Entities
{
    public class EntityIdentifier
    {
        private static int _index;

        private int? _id;
        
        public int Id => _id ??= _index++;
    }
}