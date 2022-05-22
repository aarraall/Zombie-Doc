using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour, IFightable<ZombieController>
{
    [SerializeField] EnemyModel model;
    [SerializeField] EnemyView view;

    [field: SerializeField] public bool IsDead { get; private set; }
    List<ZombieController> _targets = new List<ZombieController>();

    WaitForSeconds _fightWait;

    public Action OnDeath;
    private void Awake()
    {
        _fightWait = new WaitForSeconds(model.fightRate);
        view.Initialize(model.animationSet);
    }

    public void SetFightReady(List<ZombieController> targets)
    {
        _targets = targets;
        Fight();
    }


    public void Fight()
    {

        StartCoroutine(FightCoroutine());

        IEnumerator FightCoroutine()
        {
            while (!IsDead)
            {
                ZombieController enemy = ClosestOpponent();
                if (!enemy) yield break;

                if (Vector3.Distance(enemy.transform.position, transform.position) > model.fightingDistance)
                {
                    view.OnChase();
                    transform.position = Vector3.MoveTowards(transform.position, enemy.transform.position, Time.deltaTime * model.chaseSpeed);
                    yield return null;
                }
                else
                {
                    yield return _fightWait;
                    Hit(enemy);
                }

            }
        }
    }

    public void GetHit()
    {
        IsDead = true;
        gameObject.SetActive(false);
        OnDeath?.Invoke();
    }


    public ZombieController ClosestOpponent()
    {
        return _targets.OrderBy(target => Vector3.Distance(target.transform.position, transform.position)).FirstOrDefault(target => !target.IsDead);
    }

    public void Hit(ZombieController opponent)
    {
        if (opponent == null || view == null) return;
        view.OnFight();
        opponent.GetHit();
    }

}

