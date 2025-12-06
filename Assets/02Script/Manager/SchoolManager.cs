using _02Script.Obj.Character;
using _02Script.Etc;


namespace _02Script.Manager
{
    public class SchoolManager : Singleton<SchoolManager>
    {
        //public SaveDictionary<int, PlayerJob> todayClass; //오늘 수업
        public bool setTodayClass; // true : 오늘 수업 세팅 함/ false : 오늘의 수업 세팅 안 함.

        private void Awake()
        {
            GameManager.OnNextDay += ClearToday;
        }

        public void ClearToday() //학교 일정 싹 비우기
        {
            //todayClass.Clear();
        }

        public void SettingTodayClass() //수업 세팅 한 것을 알려주기.
        {
            setTodayClass = true;
            //추후 옆 UI 띄울 생각.
        }
    }
}
