using System;
using System.Collections.Generic;

public class Line
{

    public List<int> clues;
    public List<List<bool>> possibleSolutions;
    public List<bool> currentSolution;
    public int length;


	public Line(List<int> clues, int length)
	{
        this.clues = clues;
        this.possibleSolutions = new List<List<bool>>();
        this.length = length;
        this.currentSolution = new List<bool>();
	}
}
