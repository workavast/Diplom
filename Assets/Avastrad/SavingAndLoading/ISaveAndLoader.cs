namespace Avastrad.SavingAndLoading
{
    public interface ISaveAndLoader
    {
        public void Save(object data);

        public T Load<T>() 
            where T : new();

        public bool SaveExist();

        public void DeleteSave();
    }
}