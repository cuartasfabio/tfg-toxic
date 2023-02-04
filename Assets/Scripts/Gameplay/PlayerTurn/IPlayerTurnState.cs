namespace Gameplay.PlayerControllers
{
    public interface IPlayerTurnState
    {
        void Enter();
        void UpdateState();
        void Exit();
    }
}