using _02Script.MiniGame.Produce;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _02Script.MiniGame.Food.Pot
{
    public class PotBubble : MiniGameObj
    {
        [SerializeField] private Image bubbleImage;
        [SerializeField] private AudioSource bubbleSound;

        private readonly float _minTime = 1;
        private readonly float _maxTime = 3;

        private void OnEnable()
        {
            Show();
        }

        private void Show()
        {
            bubbleImage.color = new Color(bubbleImage.color.r, bubbleImage.color.g, bubbleImage.color.b, 1);
            //bubbleSound.Play();
            
            bubbleImage.DOFade(0, Random.Range(_minTime, _maxTime)).OnComplete(() =>
            {
                spawn.ObjListAdd(this);
            });
        }
    }
}