using System;

namespace lab_oop_box_game
{
    [Serializable]
    public class savegame
    {
        public gameobjser _mycannon;
        public gameobjser[] _myenemies;
        public gameobjser[] _mycannonballs;
        public int _myscore;
        public int _myhealth;


        public savegame(gameobjser mycannon, gameobjser[] myenemies, gameobjser[] mycannonballs, int myscore, int myhealth)
        {
            _mycannon = mycannon;
            _myenemies = myenemies;
            _mycannonballs = mycannonballs;
            _myscore = myscore;
            _myhealth = myhealth;
        }
    }
}