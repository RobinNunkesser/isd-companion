﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ISDCompanion.Resources.Strings;
using ISDCompanion.Services;
using Italbytz.Adapters.Exam.OperatingSystems;
using Italbytz.Infrastructure.Exam.OperatingSystems.PageReplacement;
using Italbytz.Ports.Exam.OperatingSystems;
using TableGen;

namespace ISDCompanion
{
    public class PageReplacementViewModel : StepCollectionViewModel<PageReplacementStepViewModel>
    {

        private int selectedStrategy = 0;
        public int SelectedStrategy
        {
            get => selectedStrategy;
            set
            {
                if (selectedStrategy != value)
                {
                    selectedStrategy = value;
                    OnPropertyChanged();
                    Steps = ComputeItems();
                }
            }
        }

        protected override int NoOfSteps => 13;

        private List<IPageReplacementStep>? clockSolution;
        private List<IPageReplacementStep>? optimalSolution;
        private List<IPageReplacementStep>? lruSolution;
        private List<IPageReplacementStep>? fifoSolution;
        private PageReplacementParameters? parameters;

        protected override void newExercise()
        {
            parameters = new PageReplacementParameters()
            {
                MemorySize = 4
            };

            optimalSolution = new OptimalSolver().Solve(parameters).Steps;
            optimalSolution.RemoveAt(0);

            fifoSolution = new FIFOSolver().Solve(parameters).Steps;
            fifoSolution.RemoveAt(0);

            lruSolution = new LRUSolver().Solve(parameters).Steps;
            lruSolution.RemoveAt(0);

            clockSolution = new ClockSolver().Solve(parameters).Steps;
            clockSolution.RemoveAt(0);

            Steps = ComputeItems();
        }

        private PageReplacementStepViewModel[] ComputeItems()
        {
            CurrentSolutionStep = 0;

            List<IPageReplacementStep>? solution = null;

            if (selectedStrategy == -1)
            {
                selectedStrategy = 0;
            }

            switch (selectedStrategy)
            {
                case 0:
                    solution = optimalSolution;
                    break;
                case 1:
                    solution = fifoSolution;
                    break;
                case 2:
                    solution = lruSolution;
                    break;
                case 3:
                    solution = clockSolution;
                    break;
            }

            var newSteps = solution.Select((item) =>
            {
                return new PageReplacementStepViewModel()
                {
                    Element = $"{item.Element}",
                    Frames = item.Frames.Select((x) => PageReplacementViewModel.entryToString(x)).ToArray(),
                    FrameInformation = item.FrameInformation.Select((x) => PageReplacementViewModel.entryToString(x)).ToArray(),
                    AdditionalInfo = item.AdditionalInfo
                };
            }).ToArray();

            return newSteps;
        }

        private static String entryToString(int value)
        {
            if (value == int.MaxValue) return "∞";
            return $"{value}";
        }

        protected override void showInfo()
        {
            throw new NotImplementedException();
        }
    }
}