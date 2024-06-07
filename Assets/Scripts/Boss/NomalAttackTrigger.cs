using System;
using System.Collections;
using UnityEngine;

public class NomalAttackTrigger : MonoBehaviour
{
    [SerializeField] private int _nomalAttackValue;
    private bool _isNomalAttacking;

    public GameObject _hitprefab;
    public GameObject _hitEffect;

    private void Start()
    {
        _nomalAttackValue = 10;
        _isNomalAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isNomalAttacking)
        {
            Debug.Log("Start Nomal Attacking~~~~~~");
            NowNomalAttacking();
            Vector3 hitPos = other.ClosestPoint(transform.position);
            AppearHitEffect(hitPos, other.gameObject);
            CombatManager.Instance.TakeDamage("Boss", _nomalAttackValue);
        }
    }

    /// <summary>
    /// NomalAttack을 한 번 실행하는 동안 공격이 중첩되지 않도록 
    /// _isNomalAttacking를 잠시 켰다 꺼는 코루틴 CoNomalAttack을 실행합니다.
    /// </summary>
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

    /// <summary>
    /// boss의 nomalAttack에 충돌된 위치에 HitEffect를 나타내는 코루틴을 실행합니다.
    /// 해당 코루틴은 CoHitEffect입니다.
    /// </summary>
    /// <param name="hitPos">충돌된 개체가 맞은 곳</param>
    /// <param name="player">충돌된 개체 = Player</param>
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
