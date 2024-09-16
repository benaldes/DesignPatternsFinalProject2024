namespace Interfaces
{
    public interface IPickupAble
    {
        public int pickupID { get; set; }
        public PickupType pickupType { get; set; }
        public void OnPickedUp(PickupType type);
    }

    public enum PickupType
    {
        SpeedBoost,
        ScoreBoost100
    }
}
