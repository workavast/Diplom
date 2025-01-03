using UnityEngine;

namespace Avastrad.Storages
{
    [System.Serializable]
    public abstract class StorageBase<TDataType> : IReadOnlyStorage<TDataType> where TDataType : struct
    {
        [SerializeField] protected TDataType maxValue;
        [SerializeField] protected TDataType currentValue;
        [SerializeField] protected TDataType minValue;

        public TDataType MaxValue => maxValue;
        public TDataType CurrentValue => currentValue;
        public TDataType MinValue => minValue;

        public abstract float FillingPercentage { get; }
        public abstract bool IsFull { get; }
        public abstract bool IsEmpty { get; }

        public abstract event System.Action OnChange;
        public abstract event System.Action<TDataType> OnMaxValueChange;
        public abstract event System.Action<TDataType> OnCurrentValueChange;
        public abstract event System.Action<TDataType> OnMinValueChange;
        
        public abstract void SetMaxValue(TDataType newMaxValue);

        public abstract void SetCurrentValue(TDataType newCurrentValue);

        public abstract void SetMinValue(TDataType newMinValue);

        public abstract void ChangeCurrentValue(TDataType value);
    }
}