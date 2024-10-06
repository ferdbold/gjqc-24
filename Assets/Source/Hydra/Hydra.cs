using System;
using System.Collections;
using System.Collections.Generic;
using QFSW.QC;
using UnityEngine;

public class Hydra : MonoBehaviour
{
    public static Action OnHydraDefeated;

    [Header("Components")]
    [SerializeField] private Transform _neckAnchor;

    [Header("References")]
    [SerializeField] private HydraData _hydraData;
    [SerializeField] private HydraHead _headPrefab;

    [Header("Values")]
    [SerializeField] private float _maxHeads = 8;

    private List<HydraHead> _heads = new();
    private bool _dontSpawnHeads = false;

    private void OnEnable()
    {
        _heads.Clear();
        SpawnHead();

        Game.OnGameReset += CB_OnGameReset;
        HydraHead.OnDeath += CB_OnHeadDied;
    }

    private void OnDisable()
    {
        Game.OnGameReset -= CB_OnGameReset;
        HydraHead.OnDeath -= CB_OnHeadDied;
    }

    private void CB_OnGameReset()
    {
        _hydraData.Reset();
    }

    private void CB_OnHeadDied(HydraHead head)
    {
        if (!_heads.Remove(head))
            return;

        Debug.Log("Head died");

        _hydraData.Heads.Remove(head.HydraHeadData);
        _hydraData.HeadsDefeated++;

        if (_dontSpawnHeads)
            return;

        StartCoroutine(SpawnHead_Coroutine(1f));
        StartCoroutine(SpawnHead_Coroutine(2f));
    }

    private IEnumerator SpawnHead_Coroutine(float wait)
    {
        yield return new WaitForSeconds(wait);
        SpawnHead();
    }

    public void SpawnHead()
    {
        if (_heads.Count >= _maxHeads)
            return;

        Debug.Log("Spawning head");
        var newHead = Instantiate(_headPrefab, _neckAnchor);
        _heads.Add(newHead);
        _hydraData.Heads.Add(newHead.HydraHeadData);
    }

    [Command("SpawnHead")]
    public static void CMD_SpawnHead()
    {
        FindFirstObjectByType<Hydra>().SpawnHead();
    }

    [Command("KillAllHeads")]
    public static void CMD_KillAllHeads()
    {
        var hydra = FindFirstObjectByType<Hydra>();
        hydra._dontSpawnHeads = true;

        foreach (var head in hydra._heads)
            head.Die();

        hydra._dontSpawnHeads = true;
    }
}
