﻿namespace Bumblebee.Interfaces;

public interface ITable
{
	IEnumerable<string> Headers { get; }
	IEnumerable<ITableRow> Rows { get; }
	IEnumerable<string> Footers { get; }
	T HeaderAs<T>() where T : IBlock;
	IEnumerable<T> RowsAs<T>() where T : IBlock;
	T FooterAs<T>() where T : IBlock;
}