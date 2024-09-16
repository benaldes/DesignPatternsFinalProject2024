namespace Interfaces
{
    public interface IEventListener
    {
        void OnEventReceived(EventMessage eventMessage);
    }
}