﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using NetworksExam.CRC;
using Xamarin.Forms;

namespace ISDCompanion
{
    public class CRCViewModel : INotifyPropertyChanged
    {
        public ICommand NewParams { get; set; }

        private string bits = "";
        public string Bits
        {
            get => bits;
            set
            {
                if (value != bits)
                {
                    bits = value;
                    OnPropertyChanged();
                }
            }
        }

        private string calculation = "";
        public string Calculation
        {
            get => calculation;
            set
            {
                if (value != calculation)
                {
                    calculation = value;
                    OnPropertyChanged();
                }
            }
        }

        private string check = "";
        public string Check
        {
            get => check;
            set
            {
                if (value != check)
                {
                    check = value;
                    OnPropertyChanged();
                }
            }
        }


        public CRCViewModel()
        {
            Initialize();
            NewParams = new Command(Initialize);
        }

        private void Initialize()
        {
            var parameters = new CRCParameters();
            var solver = new CRCSolver();
            var solution = solver.Solve(parameters);

            Bits = Convert.ToString(parameters.Term, 2);
            Calculation = solution.Calculation;
            Check = solution.Check;
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "") =>
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        #endregion

    }

}