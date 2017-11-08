using System;
using System.Collections.Generic;

public class Grid
{
    public int numOfRows;
    public int numOfCols;
    public List<Line> rows;
    public List<Line> columns;

    public Grid()
	{
	}

    public Grid(int numOfRows, int numOfCols, List<Line> rows,List<Line>columns)
    {
        this.numOfRows = numOfRows;
        this.numOfCols = numOfCols;
        this.rows = rows;
        this.columns = columns;
    }
}
