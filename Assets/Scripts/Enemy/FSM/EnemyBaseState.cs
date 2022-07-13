
public abstract class EnemyBaseState
{
    /// <summary>
    /// 刚进入状态执行的功能
    /// </summary>
    /// <param name="enemy"></param>
    public abstract void EnterState(Enemy enemy);

    /// <summary>
    /// 在当前状态下持续执行的功能
    /// </summary>
    /// <param name="enemy"></param>
    public abstract void OnUpdate(Enemy enemy);

}
