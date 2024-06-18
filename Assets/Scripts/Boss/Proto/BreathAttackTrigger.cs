using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BreathAttackTrigger : MonoBehaviour
{
    [SerializeField] private int _breathAttackValue;
    public Transform _breathObject;
    public GameObject _hitprefab;
    public GameObject _hitBreathEffect;
    public float countTime = 0;

    private void Awake()
    {
        _breathAttackValue = 5;
        _breathObject = transform.parent;
        _breathObject.gameObject.SetActive(false);
    }
    
    private void OnEnable()
    {
        StartCoroutine(CoStartBreath());
    }

    private void OnDisable()
    {
        StopCoroutine(CoStartBreath());
        countTime = 0;
        _breathObject.localScale = new Vector3(0.5f, 0.5f, 0);

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Breath Attacking~~~~~~");
            AppearHitEffect(other.gameObject);
            CombatManager.Instance.TakeDamage("Boss", _breathAttackValue);
        }
    }

    IEnumerator CoStartBreath()
    {
        while (countTime <= 3f)
        {
            _breathObject.localScale += new Vector3(0, 0, Time.deltaTime * 3);
            yield return null;
            countTime += Time.deltaTime;
        }
    }
    
    /// <summary>
    /// boss의 BreathAttack에 충돌된 대상의 주변에 불꽃 이펙트를 띄우는 코루틴을 실행합니다.
    /// 해당 코루틴은 CoHitEffect입니다.
    /// </summary>
    /// <param name="hitPos">충돌된 개체가 맞은 곳</param>
    /// <param name="player">충돌된 개체 = Player</param>
    public void AppearHitEffect(GameObject player)
    {
        StartCoroutine(CoHitEffect(player));
    }

    public IEnumerator CoHitEffect(GameObject player)
    {
        if(_hitBreathEffect == null)
        {
            _hitprefab = Resources.Load<GameObject>("Prefabs/HitEffectSample");
            _hitBreathEffect = Instantiate(_hitprefab, player.transform);
            _hitBreathEffect.transform.parent = player.transform;
        }
        yield return new WaitForSeconds(1.5f);
        _hitBreathEffect.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        _hitBreathEffect.SetActive(false);
    }

}
