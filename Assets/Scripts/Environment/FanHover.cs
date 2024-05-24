using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanHover : Activateable
{
    [SerializeField] private float power = 10;
    [SerializeField] private float soundRange = 100;

    private Rigidbody rb;
    private string playerTag = "Player";

    public void SetState(bool state)
    {
        activated = state;

        //A bit messy but only meant for debugging purposes
        gameObject.GetComponent<MeshRenderer>().enabled = state;
    }

    private void OnEnable()
    {
        EventBus<PositionBroadcasted>.OnEvent += OnPositionBroadcasted;
    }

    private void OnDisable()
    {
        EventBus<PositionBroadcasted>.OnEvent -= OnPositionBroadcasted;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == playerTag) {
            rb = other.GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == playerTag) {
            rb = null;
        }
    }

    private void FixedUpdate() {
        if (rb != null && activated) {
            rb.velocity += gameObject.transform.up * power;
        }
    }

    private void Start()
    {
        if(activated)
            EventBus<SoundEffectPlayed>.Publish(new SoundEffectPlayed(SoundEffectType.Wind));
    }

    private void OnPositionBroadcasted(PositionBroadcasted pPositionBroadcasted)
    {
        if (!activated)
            return;

        //float reverseDist = MathF.Abs((pPositionBroadcasted.position - transform.position).magnitude - soundRange);
        float dist = (pPositionBroadcasted.position - transform.position).magnitude;
        float volume = map(dist, 0, soundRange, 1, 0);

        EventBus<SoundEffectVolumeChanged>.Publish(new SoundEffectVolumeChanged(SoundEffectType.Wind, volume));
        //Debug.Log("dist: " + dist + " vol: "+ volume);
    }

    // Maps a value from ome arbitrary range to another arbitrary range
    private float map(float value, float leftMin, float leftMax, float rightMin, float rightMax)
    {
        return rightMin + (value - leftMin) * (rightMax - rightMin) / (leftMax - leftMin);
    }
}