using System.Collections.Generic;

namespace GameOfLive.Models
{
    public class Bacteria
    {
        /// <summary>
        /// If the Bacteria is alive
        /// </summary>
        public bool IsAlive { get; set; }
        /// <summary>
        /// Used to check if the bacteria was alive before the new generation started
        /// </summary>
        public bool WasAlive { get; set; }
        /// <summary>
        /// X Position
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// Y Position
        /// </summary>
        public int Y { get; set; }
        /// <summary>
        /// List of neighbour cells.
        /// </summary>
        public List<Bacteria> Neighbours { get; set; }
    }
}
