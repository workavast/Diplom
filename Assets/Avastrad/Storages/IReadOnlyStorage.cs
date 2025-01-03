namespace Avastrad.Storages
{
    public interface IReadOnlyStorage<TDataType> 
        where TDataType : struct
    {
        public TDataType MaxValue { get; }
        public TDataType CurrentValue { get; }
        public TDataType MinValue { get; }

        public float FillingPercentage { get; }
        public bool IsFull { get; }
        public bool IsEmpty { get; }

        public event System.Action OnChange;
        public event System.Action<TDataType> OnMaxValueChange;
        public event System.Action<TDataType> OnCurrentValueChange;
        public event System.Action<TDataType> OnMinValueChange;
    }
}