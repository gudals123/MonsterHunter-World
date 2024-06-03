using System;
using System.Collections;
using UnityEngine;

public class NomalAttackTrigger : MonoBehaviour
{
    [SerializeField] private int _nomalAttack;
    private bool _isNomalAttacking;

    public GameObject _hitprefab;
    public GameObject _hitEffect;

    private void Start()
    {
        _nomalAttack = 10;
        _isNomalAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isNomalAttacking)
        {
            Debug.Log("Start Nomal Attacking~~~~~~");
            Vector3 hitPos = other.ClosestPoint(transform.position);
            AppearHitEffect(hitPos, other.gameObject);
            CombatManager.TakeDamage("Boss", _nomalAttack);
        }
    }

    public void NowNomalAttacking()
    {
        StartCoroutine(CoNomalAttack());
    }

    public IEnumerator CoNomalAttack()
    {
        _isNomalAttacking = true;
        yield return new WaitForSeconds(2f);
        Debug.Log("Quit Nomal Attack~~~~~~");
        _isNomalAttacking = false;
    }

    public void AppearHitEffect(Vector3 hitPos, GameObject player)
    {
        StartCoroutine(CoHitEffect(hitPos, player));
    }

    public IEnumerator CoHitEffect(Vector3 hitPos, GameObject player)
    {
        if(_hitEffect == null)
        {
            _hitprefab = Resources.Load<GameObject>("Prefabs/HitEffectSample");
            _hitEffect = Instantiate(_hitprefab);
            _hitEffect.transform.parent = player.transform;
        }
        _hitEffect.transform.position = hitPos;
        _hitEffect.SetActive(true);
        yield return new WaitForSeconds(1f);
        _hitEffect.SetActive(false);
    }




}
