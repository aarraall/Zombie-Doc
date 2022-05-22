using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZombieController : MonoBehaviour, IFightable<EnemyController>
{
    [SerializeField] ZombieModel model;
    [SerializeField] ZombieView view;

    public bool IsHit { get; set; }
    private bool _isDead; public bool IsDead => _isDead;

    private List<EnemyController> _targets = new List<EnemyController>();

    WaitForSeconds _waitNextTarget;
    public Action OnDeath;


    private void Awake()
    {
        view.Initialize(model.animationSet);
        _waitNextTarget = new WaitForSeconds(model.nextTargetFindRate);
        view.OnIdle();
        view.OnTriggerEnterEvent += OnTriggerEnterEvent;
    }

    private void FollowTarget(Transform target)
    {
        StartCoroutine(FollowCoroutine());

        IEnumerator FollowCoroutine()
        {
            Vector3 posXModifier = transform.position.x > 0 ? Vector3.right * UnityEngine.Random.Range(0f, 3f) : Vector3.left * UnityEngine.Random.Range(0f, 3f);
            Vector3 posZModifier = Vector3.forward * UnityEngine.Random.Range(-1.5f, 1.5f);
            while (true)
            {
                yield return null;
                transform.LookAt(new Vector3(target.transform.position.x, 0, target.transform.position.z));
                Vector3 targetPos = Vector3.back * model.followDistance + posXModifier + posZModifier;
                transform.position = Vector3.MoveTowards(transform.position, target.position + targetPos, Time.deltaTime * model.followSpeed);
                if (transform.position == target.position + targetPos)
                {
                    transform.SetParent(target);
                    transform.forward = Vector3.back;
                    yield break;
                }

            }
        }
    }

    private void OnTriggerEnterEvent(Collider other)
    {
        switch (other.tag)
        {
            case "Syringe":
                Syringe syringe = other.GetComponentInParent<Syringe>();
                view.SetMaterial(model.zombieMat);
                view.OnFollow();
                FollowTarget(syringe.SenderTransform);
                ObjectPoolProxy.Dismiss(syringe);
                break;
        }
    }
    public void SetFightReady(List<EnemyController> targets)
    {
        _targets = targets;
        Fight();
    }

    public void Fight()
    {
        StartCoroutine(FightCoroutine());

        IEnumerator FightCoroutine()
        {
            while (true)
            {
                EnemyController enemy = ClosestOpponent();

                if (!enemy || IsDead) yield break;

                if (Vector3.Distance(enemy.transform.position, transform.position) > model.fightingDistance)
                {
                    view.OnChase();
                    transform.position = Vector3.MoveTowards(transform.position, enemy.transform.position, Time.deltaTime * model.chaseSpeed);
                    yield return null;
                }
                else
                {
                    yield return _waitNextTarget;
                    Hit(enemy);
                }

            }
        }
    }

    public EnemyController ClosestOpponent()
    {
        return _targets.OrderBy(target => Vector3.Distance(target.transform.position, transform.position)).FirstOrDefault(target => !target.IsDead);
    }

    public void Hit(EnemyController opponent)
    {
        view.OnFight();
        StartCoroutine(HitCoroutine());
        IEnumerator HitCoroutine()
        {
            yield return new WaitForSeconds(1);
            opponent.GetHit();
        }
    }

    public void GetHit()
    {
        _isDead = true;
        gameObject.SetActive(false);
        OnDeath?.Invoke();
    }
}
