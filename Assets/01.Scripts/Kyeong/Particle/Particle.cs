using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : PoolableMono
{
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    public override void Init()
    {
        _particleSystem.Play();
        StartCoroutine(ReturnParticle());
    }

    private IEnumerator ReturnParticle()
    {
        yield return new WaitForSeconds(_particleSystem.startLifetime);
        PoolManager.Instance.Push(this);
    }
}
