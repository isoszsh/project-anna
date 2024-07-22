public abstract class ChestAiBaseState
{
    public ChestAiEnemy enemy;
    public ChestAiStateMachine stateMachine;
    public abstract void Enter();
    public abstract void Perform();
    public abstract void Exit();
}