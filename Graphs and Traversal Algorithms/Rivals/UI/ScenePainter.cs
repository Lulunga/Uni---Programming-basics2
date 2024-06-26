﻿using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using Brushes = Avalonia.Media.Brushes;
using Color = Avalonia.Media.Color;
using Size = Avalonia.Size;

namespace Rivals.UI;

public class ScenePainter : Canvas
{
	public Size Size => new(currentMap.Maze.GetLength(0) * CellSize.Width,
		currentMap.Maze.GetLength(1) * CellSize.Height);

	private Map currentMap;
	private int currentIteration;
	private Dictionary<Map, List<OwnedLocation>> mapStates;
	private Bitmap grass;
	private Bitmap path;
	private Size CellSize => grass.Size;

	private static readonly SolidColorBrush[] colourValues = new[]
	{
		"#FF0000", "#00FF00", "#0000FF", "#FFFF00", "#FF00FF", "#00FFFF", "#000000",
		"#800000", "#008000", "#000080", "#808000", "#800080", "#008080", "#808080"
	}.Select(c => (SolidColorBrush)new BrushConverter().ConvertFrom(c)).ToArray();

	public ScenePainter()
	{
		LoadResources();
	}

	public void LoadMaps(Map[] maps)
	{
		currentMap = maps[0];
		PlayLevels(maps);
		currentIteration = 0;
	}

	// Loading non-string resources via ResourcesManages causes error with BuildTools2022
	private void LoadResources()
	{
		const string resourcesPrefix = "UI/Images/";

		grass = new Bitmap($"{resourcesPrefix}Grass.png");
		path = new Bitmap($"{resourcesPrefix}Path.png");
	}

	private void PlayLevels(Map[] maps)
	{
		mapStates = new Dictionary<Map, List<OwnedLocation>>();
		foreach (var map in maps)
			mapStates[map] = RivalsTask.AssignOwners(map).ToList();
	}

	public void ChangeLevel(Map newMap)
	{
		currentMap = newMap;
		currentIteration = 0;
		Width = Size.Width;
		Height = Size.Height;
		InvalidateVisual();
	}

	public void Update()
	{
		currentIteration = Math.Min(currentIteration + 1, mapStates[currentMap].Count);
		InvalidateVisual();
	}

	public override void Render(DrawingContext context)
	{
		base.Render(context);
		RenderMap(context);
		DrawPath(context);
	}

	private void DrawPath(DrawingContext context)
	{
		var mapState = mapStates[currentMap];
		var textTop = new Avalonia.Point(0, 0.25 * CellSize.Height);
		var typeface = new Typeface("Segoe UI Light");
		const int fontSize = 26;

		foreach (var cell in mapState.Take(currentIteration))
		{
			var cellLocation = new Avalonia.Point(cell.Location.X * CellSize.Width, cell.Location.Y * CellSize.Height);
			var rect = new Rect(cellLocation, CellSize);
			var color = colourValues[cell.Owner % colourValues.Length];

			context.FillRectangle(new SolidColorBrush(Color.FromArgb(100, color.Color.R, color.Color.G, color.Color.B)),
				rect);

			var formattedText = new FormattedText(
				cell.Distance.ToString(),
				typeface,
				fontSize,
				TextAlignment.Center,
				TextWrapping.NoWrap,
				CellSize);

			context.DrawText(Brushes.Beige, cellLocation + textTop, formattedText);
		}
	}

	private void RenderMap(DrawingContext context)
	{
		var dungeonWidth = currentMap.Maze.GetLength(0);
		var dungeonHeight = currentMap.Maze.GetLength(1);
		
		var cellWidth = CellSize.Width;
		var cellHeight = CellSize.Height;

		for (var x = 0; x < dungeonWidth; x++)
		{
			for (var y = 0; y < dungeonHeight; y++)
			{
				var image = currentMap.Maze[x, y] == MapCell.Empty ? path : grass;
				context.DrawImage(image, new Rect(x * cellWidth, y * cellHeight, cellWidth, cellHeight));
			}
		}
	}
}