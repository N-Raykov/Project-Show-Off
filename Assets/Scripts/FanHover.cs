using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanHover : MonoBehaviour
{
    [SerializeField] private float power = 10;
    [SerializeField] private bool activated = true;
    [SerializeField] private float soundRange;

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

    private void Start() {
        SetState(activated);
        // EventBus<SoundEffectPlayed>.Publish(new SoundEffectPlayed(SoundEffectType.Wind));
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

    private void OnPositionBroadcasted(PositionBroadcasted pPositionBroadcasted)
    {
        float reverseDist = MathF.Abs((pPositionBroadcasted.position - transform.position).magnitude - soundRange);
        float volume = map(reverseDist, 0, soundRange, 0, 1);

        EventBus<SoundEffectVolumeChanged>.Publish(new SoundEffectVolumeChanged(SoundEffectType.Wind, volume));
        //Debug.Log("reverseDist: " + reverseDist + " vol: "+ volume);
    }

    // Maps a value from ome arbitrary range to another arbitrary range
    private float map(float value, float leftMin, float leftMax, float rightMin, float rightMax)
    {
        return rightMin + (value - leftMin) * (rightMax - rightMin) / (leftMax - leftMin);
    }
}
