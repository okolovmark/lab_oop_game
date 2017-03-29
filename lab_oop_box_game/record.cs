using System;

namespace lab_oop_box_game
{
    [Serializable]
    class Record
    {
        public int record { get; set; }
       
        public Record(int score)
        {
            record = score;
        }
    }
}
