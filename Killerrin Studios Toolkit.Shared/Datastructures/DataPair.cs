using System;
using System.Collections.Generic;
using System.Text;

namespace KillerrinStudiosToolkit.Datastructures
{
    public class DataPair <T1, T2>
    {
        public T1 ValueOne { get; set; }
        public T2 ValueTwo { get; set; }

        public DataPair (T1 t1, T2 t2)
        {
            ValueOne = t1;
            ValueTwo = t2;
        }
    }
}
