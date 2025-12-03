using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _02Script.Item
{
    [CreateAssetMenu(fileName = "ItemSO", menuName = "SO/ItmeSO")]
    public class ItemSO : ScriptableObject
    {
        [SerializeField] private string itemName; //아이템 이름

        public ItemCategory category; //카테고리
        public ItemType itemType; //아이템 종류

        public int sellCoin; //파는 가격

        public Sprite itemImage; //생긴거
    }

    public enum ItemCategory //카테고리
    {
        gift = 8000,
        potion = 1000,
        book = 2000,
        special = 3000,
        work = 4000,
        chat = 5000,
        hate = 6000,
        notbad = 7000,
        coin = 9000,
        mouse = 10000,
        none = 0
    }

    public enum ItemType //종류
    {
        none = 0, //없다.

        lovePotion = 1001, //포션
        staminaPotion = 1002,
        painPotion = 1003,

        blackbook = 2001, //책
        healbook = 2002,
        firebook = 2003,
        waterbook = 2004,
        copybook = 2005,
        potionbook = 2006,

        umbrella = 3001, //우산
        broomstick = 3002, //빗자루 (순간이동 아이템)
        fan = 3003, //부채
        hotPack = 3004, //핫팩
        fryingPan = 3005, //후라이팬
        flower = 3006, //꽃다발 (연애)
        foragingBin = 3007, //곤충 채집통
        emptyGlass = 3008, //빈 유리병

        drug = 4001, //마약
        box = 4002, //상자

        glasses = 5001, //속마음을 보는 안경 (올바른 선택지)
        readingGlasses = 5002, //돋보기 (선호하는 아이템 2개)

        worm = 6001, //벌레 들
        trashGlass = 6002, //실패한 유리병

        perfectGlass = 7001, //생태계 유리병

        coin = 9001, //코인

        restyMouse = 10001, //커서
        chrisMouse = 10002,
        pioMouse = 10003,
        theoMouse = 10004,
        noahMouse = 10005,
        niaMouse = 10006,
        villainMouse = 10007,
        harryMouse = 10008,
        danielMouse = 10009,

        gift = 8000 //아직임
    }
}