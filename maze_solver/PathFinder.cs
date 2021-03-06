﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace maze_solver
{
	// Can find and color a path from the start and finish pixels in the maze
	class PathFinder
    {
		// Maze color requirements
		private Color startColor;
		private Color finishColor;
		private Color wallColor;
		private Color pathColor;
		private int MaxIterations = 10000;

		// the maze image to be worked on
		private Bitmap maze;

		public Bitmap Maze
		{
			get
			{
				return maze;
			}
		}

		// keeps track of which pixels have been visited
		private bool[,] visited;

		public PathFinder(Bitmap mazeImage, Color start, Color finish, Color wall, Color path)
		{
			maze = mazeImage;
			startColor = start;
			finishColor = finish;
			wallColor = wall;
			pathColor = path;
		}

		// Method does all the steps to solve the entire maze
		// if null is returned the maze cannot be solved
		// Returns Bitmap of the maze image with a solution path drawn
		public MazeNode SolveMaze(string algorithmType)
		{
			MazeNode finishNode = null;
			MazeNode startNode = FindStartFromColor();
			// If no start pixel could be found, cannot solve maze return null
			if (startNode == null)
				return finishNode;

			
			switch (algorithmType)
			{
				case "Breadth First Search":
					// Try to find the finish node from the start doing bfs
					finishNode = DoBreadthFirstSearch(startNode);
					break;
				case "Depth First Search":
					// Try to find the finish node from the start doing dfs
					finishNode = DoDepthFirstSearch(startNode);
					break;
				case "Iterative Depth First Search":
					// Try to find the finish node from the start doing idfs
					finishNode = DoIterativeDepthFirstSearch(startNode);
					break;
				case "Greedy Best First Search":
					// Try to find the finish node from the start doing gbfs
					finishNode = DoGreedyFirstSearch(startNode);
					break;
				case "A* Search":
					//Try to find the finish node from the start doing A*
					finishNode = DoAStarSearch(startNode);
					break;
				default:
					break;
			}
			
			if (finishNode == null)
				return finishNode;

			// with the finish node having a linked list back to start
			// color the path back to the start node in the bitmap image
			ColorPathFromFinishToStart(finishNode);
			return finishNode;
		}

		public void clearVisited(int nWidth, int nHeight)
		{
			for (int w = 0; w<nWidth; w++){
				for(int h=0; h < nHeight; h++)
				{
					visited[w, h] = false;
				}
			}
		}

		public MazeNode DoIterativeDepthFirstSearch(MazeNode start)
		{
			Stack<MazeNode> nodeStack = new Stack<MazeNode>();
			visited = new bool[maze.Width, maze.Height];
			int maxLevel = 0;
			while (true)
			{
				nodeStack.Clear();
				clearVisited(maze.Width, maze.Height);
				int nCurrentLevel;
				nodeStack.Push(start);
				maxLevel += 1;

				// keep looking until there are no more nodes in the queue
				while (nodeStack.Count > 0)
				{
					MazeNode current = nodeStack.Pop();
					visited[current.GetX(), current.GetY()] = true;
					// Skip any walls
					if (IsWallNode(current))
						continue;
					// If its a finish node, we are done return this node
					if (IsFinishNode(current))
					{
						return current;
					}
					AddAllUnvisitedChildren(current, nodeStack);
					nCurrentLevel = getCurrentLevel(current);
					if (nCurrentLevel >= maxLevel)
                    {
						//Console.WriteLine(maxLevel.ToString());
						break;
					}
						
				}
			}
		}

		public MazeNode DoDepthFirstSearch(MazeNode start)
		{
			// Initialize all visited pixels to false
			visited = new bool[maze.Width, maze.Height];
			// Queue will be used to do bfs. Initialize it with the start node
			Stack<MazeNode> nodeStack = new Stack<MazeNode>();
			nodeStack.Push(start);

			MazeNode current;

			//int nCurrentLevel=0;

			// keep looking until there are no more nodes in the queue
			while (nodeStack.Count != 0)
			{
				current = nodeStack.Pop();
				// Skip any walls
				if (IsWallNode(current))
					continue;
				// If its a finish node, we are done return this node
				if (IsFinishNode(current)) { 
					//Console.WriteLine(nCurrentLevel.ToString());
					return current; }
				AddAllUnvisitedChildren(current, nodeStack);
				//nCurrentLevel = getCurrentLevel(current);
			}
			// if no finish node was found, maze cannot be solved with given
			// start or maze parameters were not set correctly.
			return null;
		}

		// Beforms a Breadth First Search from the start node until it finds
		// a finish node (pixel with finish color).
		// Returns a finish node which has a path back to the start
		// returns null if no finish could be found from the start node
		public MazeNode DoBreadthFirstSearch(MazeNode start)
		{
			// Initialize all visited pixels to false
			visited = new bool[maze.Width, maze.Height];
			// Queue will be used to do bfs. Initialize it with the start node
			Queue<MazeNode> nodeQueue = new Queue<MazeNode>();
			nodeQueue.Enqueue(start);

			MazeNode current;

			// keep looking until there are no more nodes in the queue
			while (nodeQueue.Count != 0)
			{
				current = nodeQueue.Dequeue();
				// Skip any walls
				if (IsWallNode(current))
					continue;
				// If its a finish node, we are done return this node
				if (IsFinishNode(current))
					return current;
				AddAllUnvisitedChildren(current, nodeQueue);
			}
			// if no finish node was found, maze cannot be solved with given
			// start or maze parameters were not set correctly.
			return null;
		}

		public MazeNode DoGreedyFirstSearch(MazeNode start)
		{
			// Initialize all visited pixels to false
			visited = new bool[maze.Width, maze.Height];
			// Queue will be used to do bfs. Initialize it with the start node
			List<MazeNode> nodeStack = new List<MazeNode>();
			nodeStack.Add(start);

			MazeNode current;
			MazeNode Goal = start.Goal(maze);

			//int nCurrentLevel=0;

			// keep looking until there are no more nodes in the queue
			while (nodeStack.Count != 0)
			{
				int count = nodeStack.Count();
				current = nodeStack[count - 1];
				nodeStack.RemoveAt(count - 1);
				// Skip any walls
				if (IsWallNode(current))
					continue;
				// If its a finish node, we are done return this node
				if (IsFinishNode(current))
				{
					//Console.WriteLine(nCurrentLevel.ToString());
					return current;
				}
				
				AddAllUnvisitedChildren(current, nodeStack, Goal);
				
				//nCurrentLevel = getCurrentLevel(current);
			}
			// if no finish node was found, maze cannot be solved with given
			// start or maze parameters were not set correctly.
			return null;
		}

		public MazeNode DoAStarSearch(MazeNode start)
		{
			// Initialize all visited pixels to false
			visited = new bool[maze.Width, maze.Height];
			// Queue will be used to do bfs. Initialize it with the start node
			List<MazeNode> nodeStack = new List<MazeNode>();
			nodeStack.Add(start);

			MazeNode current;
			MazeNode Goal = start.Goal(maze);

			//int nCurrentLevel=0;

			// keep looking until there are no more nodes in the queue
			while (nodeStack.Count != 0)
			{
				int count = nodeStack.Count();
				current = nodeStack[count - 1];
				nodeStack.RemoveAt(count - 1);
				// Skip any walls
				if (IsWallNode(current))
					continue;
				// If its a finish node, we are done return this node
				if (IsFinishNode(current))
				{
					//Console.WriteLine(nCurrentLevel.ToString());
					return current;
				}

				AddAllUnvisitedChildren(current, nodeStack, Goal,start);

				//nCurrentLevel = getCurrentLevel(current);
			}
			// if no finish node was found, maze cannot be solved with given
			// start or maze parameters were not set correctly.
			return null;
		}

		// Adds each child of current node that is not visited to q
		// can only be called from inside DoBreadthFirstSearch since visited
		// needs to be initialized
		private void AddAllUnvisitedChildren(MazeNode current, Queue<MazeNode> q)
		{
			int x = current.GetX();
			int y = current.GetY();

			// Left pixel. If it is not out of bounds and previously not visited
			if (x - 1 >= 0 && !visited[x - 1, y])
			{
				visited[x - 1, y] = true;
				q.Enqueue(new MazeNode(x - 1, y, current));
			}

			// Right pixel. If it is not out of bounds and previously not visited
			if (x + 1 < maze.Width && !visited[x + 1, y])
			{
				visited[x + 1, y] = true;
				q.Enqueue(new MazeNode(x + 1, y, current));
			}

			// top pixel. If it is not out of bounds and previously not visited
			if (y - 1 >= 0 && !visited[x, y - 1])
			{
				visited[x, y - 1] = true;
				q.Enqueue(new MazeNode(x, y - 1, current));
			}

			// bottom pixel. If it is not out of bounds and previously not visited
			if (y + 1 < maze.Height && !visited[x, y + 1])
			{
				visited[x, y + 1] = true;
				q.Enqueue(new MazeNode(x, y + 1, current));
			}
		}

		private void AddAllUnvisitedChildren(MazeNode current, Stack<MazeNode> q)
		{
			int x = current.GetX();
			int y = current.GetY();

			// Left pixel. If it is not out of bounds and previously not visited
			if (x - 1 >= 0 && !visited[x - 1, y])
			{
				visited[x - 1, y] = true;
				q.Push(new MazeNode(x - 1, y, current));
			}

			// Right pixel. If it is not out of bounds and previously not visited
			if (x + 1 < maze.Width && !visited[x + 1, y])
			{
				visited[x + 1, y] = true;
				q.Push(new MazeNode(x + 1, y, current));
			}

			// top pixel. If it is not out of bounds and previously not visited
			if (y - 1 >= 0 && !visited[x, y - 1])
			{
				visited[x, y - 1] = true;
				q.Push(new MazeNode(x, y - 1, current));
			}

			// bottom pixel. If it is not out of bounds and previously not visited
			if (y + 1 < maze.Height && !visited[x, y + 1])
			{
				visited[x, y + 1] = true;
				q.Push(new MazeNode(x, y + 1, current));
			}
		}

		//This method is used only for Greedy Best First Search
		private void AddAllUnvisitedChildren(MazeNode current, List<MazeNode> q, MazeNode Goal)
		{
			int x = current.GetX();
			int y = current.GetY();
			List<MazeNode> temp = new List<MazeNode>();

			// Left pixel. If it is not out of bounds and previously not visited
			if (x - 1 >= 0 && !visited[x - 1, y])
			{
				visited[x - 1, y] = true;
				temp.Add(new MazeNode(x - 1, y, current));
			}

			// Right pixel. If it is not out of bounds and previously not visited
			if (x + 1 < maze.Width && !visited[x + 1, y])
			{
				visited[x + 1, y] = true;
				temp.Add(new MazeNode(x + 1, y, current));
			}

			// top pixel. If it is not out of bounds and previously not visited
			if (y - 1 >= 0 && !visited[x, y - 1])
			{
				visited[x, y - 1] = true;
				temp.Add(new MazeNode(x, y - 1, current));
			}

			// bottom pixel. If it is not out of bounds and previously not visited
			if (y + 1 < maze.Height && !visited[x, y + 1])
			{
				visited[x, y + 1] = true;
				temp.Add(new MazeNode(x, y + 1, current));
			}

			GBFSC MyComparer = new GBFSC();
			foreach (MazeNode child in temp)
			{
				child.Heurs(Goal);
			}
			temp.Sort(MyComparer);

			foreach (MazeNode child in temp)
            {
				q.Add(child);
            }
		}

		//This method is used for A* algorithm
		private void AddAllUnvisitedChildren(MazeNode current, List<MazeNode> q, MazeNode Goal, MazeNode Start)
		{
			int x = current.GetX();
			int y = current.GetY();
			List<MazeNode> temp = new List<MazeNode>();

			// Left pixel. If it is not out of bounds and previously not visited
			if (x - 1 >= 0 && !visited[x - 1, y])
			{
				visited[x - 1, y] = true;
				temp.Add(new MazeNode(x - 1, y, current));
			}

			// Right pixel. If it is not out of bounds and previously not visited
			if (x + 1 < maze.Width && !visited[x + 1, y])
			{
				visited[x + 1, y] = true;
				temp.Add(new MazeNode(x + 1, y, current));
			}

			// top pixel. If it is not out of bounds and previously not visited
			if (y - 1 >= 0 && !visited[x, y - 1])
			{
				visited[x, y - 1] = true;
				temp.Add(new MazeNode(x, y - 1, current));
			}

			// bottom pixel. If it is not out of bounds and previously not visited
			if (y + 1 < maze.Height && !visited[x, y + 1])
			{
				visited[x, y + 1] = true;
				temp.Add(new MazeNode(x, y + 1, current));
			}

			AStarSC MyComparer = new AStarSC();
			foreach (MazeNode child in temp)
			{
				child.Heurs(Goal,Start);
			}
			temp.Sort(MyComparer);

			foreach (MazeNode child in temp)
			{
				q.Add(child);
			}
		}

		private void AddAllUnvisitedChildren(MazeNode current, Stack<MazeNode> q, int nCurrentLevel, int nMaxLimit)
		{
			int x = current.GetX();
			int y = current.GetY();

			// Left pixel. If it is not out of bounds and previously not visited
			if (x - 1 >= 0 && !visited[x - 1, y])
			{
				visited[x - 1, y] = true;
				q.Push(new MazeNode(x - 1, y, current));
			}

			// Right pixel. If it is not out of bounds and previously not visited
			if (x + 1 < maze.Width && !visited[x + 1, y])
			{
				visited[x + 1, y] = true;
				q.Push(new MazeNode(x + 1, y, current));
			}

			// top pixel. If it is not out of bounds and previously not visited
			if (y - 1 >= 0 && !visited[x, y - 1])
			{
				visited[x, y - 1] = true;
				q.Push(new MazeNode(x, y - 1, current));
			}

			// bottom pixel. If it is not out of bounds and previously not visited
			if (y + 1 < maze.Height && !visited[x, y + 1])
			{
				visited[x, y + 1] = true;
				q.Push(new MazeNode(x, y + 1, current));
			}
		}

		// Given a valid finish node with a linked list back to the start node
		// will draw a path on the bitmap image from finish to start
		public void ColorPathFromFinishToStart(MazeNode finishNode)
		{
			MazeNode current = finishNode;
			while (current != null)
			{
				maze.SetPixel(current.GetX(), current.GetY(), pathColor);
				current = current.GetParent();
			}
		}

		// Since the start coordinates must be found, search through the image
		// until a pixel with a color the same as startColor is found
		public MazeNode FindStartFromColor()
		{
			for (int i = 0; i < maze.Width; i++)
			{
				for (int j = 0; j < maze.Height; j++)
				{
					// found a start pixel. Return a new MazeNode with no parent
					// This will the be end of all linked list paths and 
					// lets ColorPathFromFinishToStart end
					if (IsStartPixel(i, j))
						return new MazeNode(i, j, null);
				}
			}
			return null;
		}

		// Returns true if the node's pixel color is a wallColor 
		public bool IsWallNode(MazeNode node)
		{
			// Dont need to check out of bounds since AddAllUnvisitedChildren does bounds checking
			return maze.GetPixel(node.GetX(), node.GetY()).ToArgb().Equals(wallColor.ToArgb());
		}

		// Given the x and y coordinates returns true if the pixel is a start color pixel
		// This is to find a start pixel to create a start node for bfs
		public bool IsStartPixel(int x, int y)
		{
			return maze.GetPixel(x, y).ToArgb().Equals(startColor.ToArgb());
		}

		// checks if the node has the same color as a finishing pixel.
		// this can be edited to put any custom finish point 
		public bool IsFinishNode(MazeNode node)
		{
			return maze.GetPixel(node.GetX(), node.GetY()).ToArgb().Equals(finishColor.ToArgb());
		}
		
		//Will count the current level of the current node based on its fathers recursively
		public int getCurrentLevel(MazeNode node)
		{
			int nLevel = 0;
			MazeNode current = node;
			while (current != null)
			{
				nLevel += 1;
				current = current.GetParent();
			}
			return nLevel;
		}

		// Incase you want a custom start node
		public MazeNode CreateStartNode(int x, int y)
		{
			return new MazeNode(x, y, null);
		}
	}
}
