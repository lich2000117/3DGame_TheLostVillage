using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YujinSoundManage : MonoBehaviour
{
    public static YujinSoundManage instance;
    public AudioSource audioSource;
    [SerializeField]
    private AudioClip attackAudio, jumpAudio, walkAudio, runAudio,
                      hurtAudio, healAudio, boxAudio, boxQAudio, boxEAudio,
                      moonBladeAudio, mudaAudio, swordRAudio, swordTAudio,
                      swordNAudio;



    public void Awake() {
        instance = this;
        // var pitchBendGroup = Resources.Load("Pitch Bend Mixer");
        // audioSource.outputAudioMixerGroup = pitchBendGroup;
    }

    public void JumpAudio() {
        audioSource.PlayOneShot(jumpAudio, 1f);
    }

    public void HurtAudio() {
        audioSource.clip = hurtAudio;
        audioSource.PlayOneShot(hurtAudio, 1f);
    }

    public void HealAudio() {
        //audioSource.clip = healAudio;
        audioSource.PlayOneShot(healAudio, 1f);
        //audioSource.Play();
        Debug.Log("Heal Audio Played");
    }

    public void RunAudio() {
        audioSource.clip = runAudio;
        audioSource.Play();
    }

    public void EndRunAudio() {
        audioSource.clip = runAudio;
        audioSource.Stop();
    }

    public void AttackAudio() {
        // audioSource.clip = attackAudio;
        audioSource.PlayOneShot(attackAudio, 1f);
    }

    public void BoxAudio() {
        // audioSource.clip = boxAudio;
        // audioSource.Play();
        audioSource.PlayOneShot(boxAudio, 1f);
    }

    public void BoxQAudio() {
        // audioSource.clip = boxAudio;
        // audioSource.Play();
        audioSource.PlayOneShot(boxQAudio, 1f);
    }

    public void BoxEAudio() {
        // audioSource.clip = boxAudio;
        // audioSource.Play();
        audioSource.PlayOneShot(boxEAudio, 1f);
    }

    public void SwordNAudio() {
        audioSource.PlayOneShot(swordNAudio, 1f);
    }

    public void SwordRAudio() {
        audioSource.PlayOneShot(swordRAudio, 1f);
    }

    public void SwordTAudio() {
        audioSource.PlayOneShot(swordTAudio, 1f);
    }

    public void MudaAudio() {
        audioSource.PlayOneShot(mudaAudio, 1f);
    }

    public void MoonBladeAudio() {
        audioSource.PlayOneShot(moonBladeAudio, 1f);
    }

    public void WalkAudio() {
        audioSource.clip = walkAudio;
        // audioSource.pitch = 2f;
        // pitchBendGroup.audioMixer.SetFloat("pitchBend", 1f / 2f);
        audioSource.Play();
    }

    public void EndWalkAudio() {
        audioSource.clip = walkAudio;
        audioSource.Stop();
    }

    public void EndAudio() {
        audioSource.Stop();
    }

}
