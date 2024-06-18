using System;
using System.Collections;
using UnityEngine;

public class NormalAttackTrigger : MonoBehaviour
{
    [SerializeField] private int _normalAttackValue;
    private bool _isNormalAttacking;

    public GameObject _hitprefab;
    public GameObject _hitEffect;

    private void Start()
    {
        _normalAttackValue = 10;
        _isNormalAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isNormalAttacking)
        {
            //Debug.Log("Start Normal Attacking~~~~~~");
            NowNormalAttacking();
            Vector3 hitPos = other.ClosestPoint(transform.position);
            AppearHitEffect(hitPos, other.gameObject);
            //CombatManager.Instance.TakeDamage("Boss", _normalAttackValue);
        }
    }

    /// <summary>
    /// normalAttack을 한 번 실행하는 동안 공격이 중첩되지 않도록 
    /// _isnormalAttacking를 잠시 켰다 꺼는 코루틴 CoNormalAttack을 실행합니다.
    /// </summary>
    public void NowNormalAttacking()
    {
        StartCoroutine(CoNormalAttack());
    }

    public IEnumerator CoNormalAttack()
    {
        _isNormalAttacking = true;
        yield return new WaitForSeconds(2f);
        //Debug.Log("Quit Nomal Attack~~~~~~");
        _isNormalAttacking = false;
    }

    /// <summary>
    /// boss의 normalAttack에 충돌된 위치에 HitEffect를 나타내는 코루틴을 실행합니다.
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
