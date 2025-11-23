using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

    public AudioClip scream1;
    public AudioClip scream2;

    public AudioClip keyPickupSound;

    public AudioClip paintingSound;

    public AudioClip dingDongClockSound;
    public AudioClip paperSound;

    public AudioClip doorOpenSound;

    public AudioClip stoneButtonPressedSound;
    public AudioClip leverPullSound;

    public AudioClip doorbell;

    public AudioClip pianoKey;

    public AudioSource audioSource;

    public void PlayScream1() => audioSource.PlayOneShot(scream1);
    public void PlayScream2() => audioSource.PlayOneShot(scream2);
    public void PlayKeyPickupSound() => audioSource.PlayOneShot(keyPickupSound);
    public void PlayPaintingSound() => audioSource.PlayOneShot(paintingSound);
    public void PlayDingDongClockSound() => audioSource.PlayOneShot(dingDongClockSound);
    public void PlayPaperSound() => audioSource.PlayOneShot(paperSound);
    public void PlayDoorOpenSound() => audioSource.PlayOneShot(doorOpenSound);
    public void PlayStoneButtonPressedSound() => audioSource.PlayOneShot(stoneButtonPressedSound);
    public void PlayLeverPullSound() => audioSource.PlayOneShot(leverPullSound);
    public void PlayDoorbell() => audioSource.PlayOneShot(doorbell);

    public void PlayPianoKey(int pitch)
    {
        // Clamp pitch to 0-12 range
        pitch = Mathf.Clamp(pitch, 0, 12);
        // Normalize to 0.1-1 range
        float pitchValue = 0.1f + pitch / 12f * 0.9f;
        audioSource.pitch = pitchValue;
        audioSource.PlayOneShot(pianoKey);
        //audioSource.pitch = 1f; // Reset pitch to normal
    }

    private void Start()
    {
        StartCoroutine(RandomDoorbellRoutine());
    }

    private IEnumerator RandomDoorbellRoutine()
    {
        // Wait 2-3 minutes before doorbell
        float initialDelay = Random.Range(120, 180);
        Debug.LogWarning($"Doorbell will ring in {initialDelay} seconds ({initialDelay / 60f:F1} minutes)");
        yield return new WaitForSeconds(initialDelay);
        Debug.LogWarning("Doorbell ringing now!");
        // Play doorbell once if player is not in Yard AND Basement is still locked
        if (GameManager.Instance.playerController.currentNode.NodeName != "Yard"
            && GameManager.Instance.basementNode.IsLocked)
        {
            PlayDoorbell();
        }
    }
}
