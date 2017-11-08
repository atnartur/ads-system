using System;
using System.Collections.Generic;

namespace AdsSystem
{
    public class RouterDictionary : Dictionary<string, string>
    {
        public static RouterDictionary operator +(RouterDictionary n1, RouterDictionary n2)
        {
            foreach (var keyValuePair in n2)
                n1[keyValuePair.Key] = keyValuePair.Value;
            foreach (var VARIABLE in n1)
            {
                Console.WriteLine(VARIABLE.Key + " " + VARIABLE.Value);
            }
            return n1;
        }     
    }
}