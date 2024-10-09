namespace Website.Client.Services
{
    public class NotificationService
    {
        public event Action<string>? OnSuccess;
        public event Action<string>? OnError;

        public void SetSuccess(string message)
        {
            OnSuccess?.Invoke(message);
        }

        public void SetError(string message)
        {
            OnError?.Invoke(message);
        }
    }

}
