using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maze_solver
{
	// This class represents a node inside the maze that has been visited
	// This is mostly to enable a way to keep track of the path back to the start
	// once the end pixel is found

	// The parent field is to be used to travel back from the finish node to the start node
	class MazeNode
    {
		private int x; // represents x coordinate inside the image
		private int y; // represents y coordinate inside the image
		private MazeNode parent; // points to the node which added this node to the queue
		public int costh = 0;
		public int costg = 0;
		public int costf = 0;

		public MazeNode()
		{
			parent = null;
		}

		public MazeNode(int xC, int yC, MazeNode p)
		{
			this.x = xC;
			this.y = yC;
			this.costg = 0;
			parent = p;
		}

		// Accessors 
		public int GetX()
		{
			return x;
		}

		public int GetY()
		{
			return y;
		}

		public MazeNode GetParent()
		{
			return parent;
		}

		// Modifiers
		public void SetX(int xCoordinate)
		{
			x = xCoordinate;
		}

		public void SetY(int yCoordinate)
		{
			y = yCoordinate;
		}

		public void SetParent(MazeNode p)
		{
			parent = p;
		}

		//This Herus Function is used only for GBFS Algorithm so it only calculate the Manhattam distance
		public void Heurs(MazeNode Goal)
		{
			
			int Heuristic = 0;
			Console.WriteLine("Calculating for: " + this.x + " , " + this.y);
			Console.WriteLine(Goal.x.ToString() + " , " + Goal.y.ToString());
			Heuristic += Math.Abs(this.x - Goal.x) + Math.Abs(this.y - Goal.y);
			// setting the state heurisitc cost.
			this.costh = Heuristic;
		}

		//This Herus method is used for A* Algorithm it will calculate the Manhattam distance to goal and start
		public void Heurs(MazeNode Goal, MazeNode Start)
		{
			Console.WriteLine("Calculating for: " + this.x + " , " + this.y);
			Console.WriteLine(Goal.x.ToString() + " , " + Goal.y.ToString());
			Console.WriteLine(Start.x.ToString() + " , " + Start.y.ToString());
			this.costh = Math.Abs(this.x - Goal.x) + Math.Abs(this.y - Goal.y);
			this.costg += this.parent.costg +1;
			// setting the state heurisitc cost.
			this.costf = this.costh + this.costg;
			Console.WriteLine("F: " + this.costf.ToString());
		}

		public MazeNode Goal(Bitmap maze)
        {
			MazeNode goal = null;
			System.Drawing.Color end = System.Drawing.Color.Blue;

			for (int i = 0; i < maze.Width; i++)
			{
				for (int j = 0; j < maze.Height; j++)
				{
					// found a start pixel. Return a new MazeNode with no parent
					// This will the be end of all linked list paths and 
					// lets ColorPathFromFinishToStart end
					if (maze.GetPixel(i, j).ToArgb().Equals(end.ToArgb()))
						goal = new MazeNode(i, j, null);
				}
			}

			return goal;

		}

		private int getRowIndexPositionOfValue(Bitmap maze, int x , int y, MazeNode Goal)
		{
			int position = 0;
			for (int indx = 0; indx < 3; indx++)
			{
				for (int jndx = 0; jndx < 3; jndx++)
				{
					if (maze.GetPixel(x, y).ToArgb().Equals(maze.GetPixel(Goal.x, Goal.y).ToArgb()))
					{
						position = indx;
					}
				}
			}
			return position;
		}

		private int getColumnIndexPositionOfValue(Bitmap maze, int x, int y, MazeNode Goal)
		{
			int position = 0;
			for (int indx = 0; indx < 3; indx++)
			{
				for (int jndx = 0; jndx < 3; jndx++)
				{
					if (maze.GetPixel(x, y).ToArgb().Equals(maze.GetPixel(Goal.x, Goal.y).ToArgb()))
					{
						position = jndx;
					}
				}
			}
			return position;
		}
	}
	class GBFSC : IComparer<MazeNode>
	{
		public int Compare(MazeNode x, MazeNode y)
		{
			if (x.costh == 0 || y.costh == 0)
			{
				return 0;
			}
			return y.costh.CompareTo(x.costh);
		}
	}

	class AStarSC : IComparer<MazeNode>
	{
		public int Compare(MazeNode x, MazeNode y)
		{
			if (x.costf == 0 || y.costf == 0)
			{
				return 0;
			}
			return y.costf.CompareTo(x.costf);
		}
	}
}
