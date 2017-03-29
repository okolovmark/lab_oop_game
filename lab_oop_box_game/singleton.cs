using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_oop_box_game
{
    public class Singleton
    {
        /// защищённый конструктор нужен, чтобы предотвратить создание экземпляра класса Singleton
        protected Singleton() { }

        private sealed class SingletonCreator
        {
            private static readonly Singleton instance = new Singleton();
            public static Singleton Instance { get { return instance; } }
        }

        public static Singleton Instance
        {
            get { return SingletonCreator.Instance; }
        }

    }
}
