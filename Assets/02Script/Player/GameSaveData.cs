using System;

namespace _02Script.Player
{
    [Serializable]
    public class GameSaveData 
    {
        public Sound sound = new Sound();
        public PlayerStatSC stat = new PlayerStatSC();
    }

    [Serializable]
    public class Sound //소리 (AudioSave)
    {
        public float mainSound;
        public float bgmSound;
        public float effectSound;
    }
}