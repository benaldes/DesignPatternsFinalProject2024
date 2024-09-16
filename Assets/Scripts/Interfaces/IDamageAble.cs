namespace Interfaces
{
    public interface IDamageAble : IKillAble
    {
        public int Health { get; set; }

        public void TakeDamage(int damage);
    }
}