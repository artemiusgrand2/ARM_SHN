using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrafficTrain
{
    public class Macro
    {

        IList<string> functionStrings = new List<string>();
        /// <summary>
        /// Функциональные строки
        /// </summary>
        public IList<string> FunctionStrings
        {
            get
            {
                return functionStrings;
            }
            set
            {
                functionStrings = value;
            }
        }

        string name = string.Empty;

        public string Name
        {
            get
            {
                return name;
            }
        }

        IList<string> parametrs = new List<string>();

        public int CountParametrs
        {
            get
            {
                return parametrs.Count;
            }
        }


        public Macro(IList<string> parametrs)
        {
            this.parametrs = parametrs;
        }

    }
}
