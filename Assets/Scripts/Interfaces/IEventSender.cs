namespace Interfaces
{
    public interface IEventSender
    {
        public int SenderID { get; set; }
        void SendEvent(EventMessage eventMessage);
    }
}