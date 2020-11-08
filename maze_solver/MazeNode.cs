using System;
using System.Collections.Generic;
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

		public MazeNode()
		{
			parent = null;
		}

		public MazeNode(int xC, int yC, MazeNode p)
		{
			this.x = xC;
			this.y = yC;
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
	}
}
