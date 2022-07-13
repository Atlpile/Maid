
public abstract class BossState
{
    //开始时的状态
    public abstract void EnterState(Enemy enemy);
    //50%~100%血量时的状态
    public abstract void OnUpdate_HundredState(Enemy enemy);
    //0%~50%血量时的状态
    public abstract void OnUpdate_HalfState(Enemy enemy);

    //处于近程攻击范围时的状态
    public abstract void OnUpdate_NearState(Enemy enemy);
    //处于远程攻击范围时的状态
    public abstract void OnUpdate_FarState(Enemy enemy);
}
