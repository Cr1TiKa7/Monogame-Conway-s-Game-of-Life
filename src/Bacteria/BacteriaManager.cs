using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLive.Bacteria
{
    public class BacteriaManager
    {
        private readonly Random _Randomizer = new Random();
        private int _maxNumber = 2;
        public int MaxNumber{ get { return _maxNumber; } } 


        private int _generation = 0;
        public int Generation => _generation;
        public double IsAliveChance;

        public BacteriaManager()
        {
            IsAliveChance = 1.0 / _maxNumber * 100;
        }

        public BacteriaManager(int maxNumber)
        {
            _maxNumber = maxNumber;
            IsAliveChance = 1.0 / _maxNumber * 100;
        }

        public void SetBirthRate(int maxNumber)
        {
            if (maxNumber > 0)
            {
                _maxNumber = maxNumber;
                IsAliveChance = 1.0 / _maxNumber * 100;
            }
        }

        /// <summary>
        /// Generates a list of bacterias. Amounts depends on the width and height parameter.
        /// </summary>
        /// <param name="width">Cell count on the x axis</param>
        /// <param name="height">Cell count on the y axis</param>
        /// <returns>A list of bacetrias</returns>
        public List<Bacteria> GenerateBacterias(int width, int height)
        {
            _generation = 0;
            var ret = new List<Bacteria>();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var newBacteria = new Bacteria
                    {
                        X = x,
                        Y = y,
                        IsAlive = GenerateBacteriaAliveStatus(),
                        Neighbours = new List<Bacteria>()
                    };
                    SetNeightboursOfBacteria(ret, newBacteria, width, height);
                    ret.Add(newBacteria);
                }
            }
            return ret;
        }

        /// <summary>
        /// Generates the next generation of the bacterias.
        /// </summary>
        /// <param name="bacterias">List of bacterias you want to generate the new generation for</param>
        public void GenerateNextGeneration(List<Bacteria> bacterias)
        {
            _generation++;
            bacterias.ForEach(x => x.WasAlive = x.IsAlive);

            foreach (var bacteria in bacterias)
            {
                var neighbourCount = bacteria.Neighbours.Count(x => x.WasAlive);
                if (bacteria.IsAlive)
                {
                    if (neighbourCount < 2)
                        bacteria.IsAlive = false;
                    if (neighbourCount == 2 || neighbourCount == 3)
                        bacteria.IsAlive = true;
                    if (neighbourCount > 3)
                        bacteria.IsAlive = false;
                }
                else
                {
                    if (neighbourCount == 3)
                        bacteria.IsAlive = true;
                }
            }
        }

        private bool GenerateBacteriaAliveStatus()
        {
            int aliveStatusIndex = _Randomizer.Next(1, _maxNumber + 1);
            return aliveStatusIndex == 1;
        }

        private void SetNeightboursOfBacteria(List<Bacteria> bacterias, Bacteria curBacteria, int width, int height)
        {
            for (int yPos = curBacteria.Y - 1; yPos <= curBacteria.Y + 1; yPos++)
            {
                if (yPos >= 0 && yPos <= height - 1)
                {
                    for (int xPos = curBacteria.X - 1; xPos <= curBacteria.X + 1; xPos++)
                    {
                        if (xPos >= 0 && xPos <= width - 1)
                        {
                            var neighbourBacteria = bacterias.FirstOrDefault(x => x.X == xPos && x.Y == yPos);
                            if (neighbourBacteria != null)
                            {
                                curBacteria.Neighbours.Add(neighbourBacteria);
                                neighbourBacteria.Neighbours.Add(curBacteria);
                            }
                        }
                    }
                }
            }
        }
    }
}