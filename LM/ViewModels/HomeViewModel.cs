using LM.Models.LM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LM.ViewModels
{
    public class HomeViewModel
    {
        public ICollection<Software> Softwares {get; set;}
        public int Entries { get; set; } = 10;

        public string GetFixedString(int size, string input)
        {
            if(input == null) return new string("");
            char[] array = input.Take(size).ToArray();
            if (input.Length-1 < size)
            {
                return new string(array);
            }
            return new string(array) + "...";
        }


    }
}
