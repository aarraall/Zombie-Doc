using System.Collections.Generic;

public interface IFightable<TOpponent>
{
    bool IsDead { get; }
    void SetFightReady(List<TOpponent> targets);
    void Fight();
    TOpponent ClosestOpponent();
    void Hit(TOpponent opponent);
    void GetHit();
}