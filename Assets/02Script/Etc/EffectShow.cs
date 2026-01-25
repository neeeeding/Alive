using UnityEngine;

namespace _02Script.Etc
{
    public class EffectShow : MonoBehaviour
    {
        [SerializeField] private GameObject effect;
        [SerializeField] private ParticleSystem[] particles;

        public void ShowEffect()
        {
            effect.SetActive(true);
            foreach (ParticleSystem particle in particles)
            {
                particle.Play();
            }
        }

        public void HideEffect()
        {
            effect.SetActive(false);
            foreach (ParticleSystem particle in particles)
            {
                particle.Stop();
            }
        }
    }
}