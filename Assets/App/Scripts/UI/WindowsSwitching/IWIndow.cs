namespace App.UI.WindowsSwitching
{
    public interface IWindow
    {
        public string Id { get; }

        public void Toggle(bool isVisible);
    }
}