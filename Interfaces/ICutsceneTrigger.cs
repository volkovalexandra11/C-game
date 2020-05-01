namespace The_Game.Interfaces
{
    public interface ITrigger
    {
        bool Active { get; set; }

        void Trigger();
    }
}
