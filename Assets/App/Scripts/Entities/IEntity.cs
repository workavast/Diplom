namespace App.Entities
{
    public interface IEntity
    {
        EntityIdentifier Identifier { get; }

        int HealthPoints { get; }

        string GetName();
    }
}