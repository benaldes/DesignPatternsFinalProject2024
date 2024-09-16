using UnityEngine;

namespace Interfaces
{
    public interface IPickupConsumer
    {
        public void ConsumePickup(IPickupAble pickupConsumed);
    }
}
