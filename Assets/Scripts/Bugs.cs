using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Bugs : MonoBehaviour
{
    public GameObject whole;
    public GameObject sliced;

    private Rigidbody bugRigidbody;
    private Collider bugCollider;
    private ParticleSystem juiceEffects;

    private void Awake()
    {
        bugRigidbody = GetComponent<Rigidbody>();
        bugCollider = GetComponent<Collider>();
        juiceEffects = GetComponentInChildren<ParticleSystem>();
    }

    private void Slice(Vector3 direction, Vector3 position, float force)
    {
        whole.SetActive(false);
        sliced.SetActive(true);

        bugCollider.enabled = false;
        juiceEffects.Play();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        sliced.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Rigidbody[] slices = sliced.GetComponentsInChildren<Rigidbody>(); 

        foreach (Rigidbody slice in slices)
        {
            slice.velocity = bugRigidbody.velocity;
            slice.AddForceAtPosition(direction * force, position, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Blade blade = other.GetComponent<Blade>();
            Slice(blade.direction, blade.transform.position, blade.sliceForce);
        }
    }
}
