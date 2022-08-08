using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Media;
using DataGrid.Models;
using DataGrid.Models.Deserializer;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Linq;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace DataGrid.ViewModels;

    [ObservableObject]
    public partial class MainVM
    {
        private Dictionary<string, List<int?>> PersonSteps { get; set; } = new Dictionary<string, List<int?>>();
        public ObservableCollection<Person> Persons { get; set; } = new ObservableCollection<Person>();

        public ISeries[] SeriesCollection { get; set; }

        private Person selectedPerson;
        public Person SelectedPerson
        {
            get { return selectedPerson; }
            set
            {
                selectedPerson = value;
                var index = SeriesCollection.Length - 1; 
                SeriesCollection[index].Values = PersonSteps[selectedPerson.Name];
                OnPropertyChanged("SelectedPerson");
            }
        }

        public MainVM()
        {
            SeriesCollection = new ISeries[] { new LineSeries<int?> {
                //DataLabelsSize = 20,
                //DataLabelsPaint = new SolidColorPaint(SKColors.Blue),
                //DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top,
                //TooltipLabelFormatter = (point) => point.PrimaryValue.ToString(),
                DataLabelsFormatter = (point) => point.PrimaryValue.ToString("C2"),
                Values = null }

            };
            GetDataAboutPerson();
        }
       
        private async void GetDataAboutPerson()
        {

            PersonSteps = await JsonFileReader.GetStepsOfPersons();
            var number = 1;
            foreach (var personStep in PersonSteps)
            {
                var steps = from p in personStep.Value
                        where p != null
                        select p;
                        
                Persons.Add(new Person(steps.ToList(), personStep.Key, number));
                number++;
            }
            var index = SeriesCollection.Length - 1;
            SeriesCollection[index].Values = PersonSteps[Persons[0].Name];
        }
    }