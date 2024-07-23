public abstract class ChestBossBaseState
{
    public ChestBossEnemy boss;
    public ChestBossStateMachine bossStateMachine;
    public abstract void Enter();
    public abstract void Perform();
    public abstract void Exit();
}