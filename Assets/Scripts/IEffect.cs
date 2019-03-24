namespace Rayark.Hi.Engine
{
    public interface IEffect
    {
        void SpeedChangeCharacterSpeed(float speedUpRatio, float speedUpAmount);
        void ChangeRemainSwipeCount(int amount);
    }
}