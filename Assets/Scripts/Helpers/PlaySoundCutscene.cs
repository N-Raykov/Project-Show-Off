using UnityEngine;

public class PlaySoundCutscene : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

   public void PlaySound() {
        audioSource.Play();
   }
}
