using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Media;

namespace DataGrid.Models
{
    public class Person
    {
        public string Character { get; set; }
        public Brush BgColor { get; set; } = Brushes.Transparent;
        public int Number { get; set; }
        public string Name { get; set; }
        public int? AvarageNumber { get; set; }
        public int? BestResult { get; set; }
        public int? WorsResult { get; set; }
        public Brush BgColorCharacter { get; set; }

        public Person(List<int?> personSteps,string name, int number)
        {
            Character = name[0].ToString().ToUpper();
            BgColorCharacter = ColorGeneration();
            Number = number;
            Name = name;
            BestResult = personSteps.Max();
            WorsResult = personSteps.Min();
            AvarageNumber = personSteps.Sum() / personSteps.Count();
            if(AvarageNumber * 1.2 < BestResult || AvarageNumber * 0.8 > WorsResult)
                BgColor = Brushes.LightBlue;
        }

        private Brush ColorGeneration()
        {
            Brush result = null;
            do
            {
                result = Brushes.Transparent;

                Random rnd = new Random();

                Type brushesType = typeof(Brushes);

                PropertyInfo[] properties = brushesType.GetProperties();

                int random = rnd.Next(properties.Length);
                result = (Brush)properties[random].GetValue(null, null);
            }
            while (result == Brushes.Black);

            return result;
        }
    }
}
