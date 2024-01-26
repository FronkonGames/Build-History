////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Martin Bustos @FronkonGames <fronkongames@gmail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of
// the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FronkonGames.Tools.BuildHistory
{
	/// <summary> Chart view mode. </summary>
	public enum ViewMode
	{
		NEVER,
		ON_SELECT,
		ALWAYS
	}

	/// <summary> Line chart. </summary>
	public class LineChart
	{
		public List<float>[] data;
		
		public List <string> dataLabels;
		
		public List <string> axisLabels;
		
		public float barSpacer = 0.5f;
		
		public float xBorder = 30;
		public float yBorder = 20;
		
		public string formatString = "{0:0}";
		public string axisFormatString = "{0:0}";
		
		public List<Color> colors = new() { Color.magenta, Color.cyan * 2.0f, Color.green };	
		
		public Color selectedColor = EditorHelper.PastelOrange;

		public float depth = 0.0f;

		public ViewMode valueViewMode = ViewMode.ON_SELECT;
		public ViewMode labelViewMode = ViewMode.ALWAYS;
		
		public int gridLines = 4;

		public float axisRounding = 10f;
		
		public Color axisColor = Color.white;
		public Color fontColor = Color.white;
		
		public GUIStyle boxStyle;
		
		public float pipRadius = 3.0f;
		
		public bool drawTicks = true;
		
		private float barFloor;
		private float barTop;
		private	float lineWidth;
		private float dataMax;
		
		private readonly EditorWindow window;
		private readonly float windowHeight;
		
		/// <summary> Constructor. </summary>
		public LineChart(EditorWindow window, float windowHeight)
		{
			this.window = window;
			this.windowHeight = windowHeight;
			
			dataLabels = new List<string>();
			axisLabels = new List<string>();
		}
		
		/// <summary> Draws the chart. </summary>
		public void DrawChart()
		{	
			if (data != null && data.Length > 0)
			{
				Rect rect = GUILayoutUtility.GetRect(Screen.width, windowHeight);
				barTop = rect.y + yBorder;
				lineWidth = (float) (Screen.width - (xBorder * 2)) / data[0].Count;
				barFloor = rect.y + rect.height - yBorder;
				dataMax = 0.0f;

				for (int i = 0; i < data.Length; ++i)
				{
					if (data[i] != null && data[i].Count > 0)
					{
						if (data[i].Max() > dataMax)
							dataMax = data[i].Max();
					}
				}
				
				// Box / border.
				if (boxStyle != null)
					GUI.Box(new Rect(rect.x + boxStyle.margin.left, rect.y + boxStyle.margin.top,
														rect.width - (boxStyle.margin.right + boxStyle.margin.left),
														rect.height - (boxStyle.margin.top + boxStyle.margin.bottom)), string.Empty, boxStyle);
				
				
				// Clean up variables.
				if (Math.Abs(dataMax % axisRounding) > float.Epsilon)
					dataMax = dataMax + axisRounding - (dataMax % axisRounding);

        // Text to Left.
        GUIStyle labelTextStyle = new()
        {
            alignment = TextAnchor.UpperRight
        };
        labelTextStyle.normal.textColor = fontColor;
				
				// Draw grid lines.
				if (gridLines > 0)
				{ 
					Handles.color = Color.grey;
					float lineSpacing = (barFloor - barTop) / (gridLines + 1);
					
					for (int i = 0; i <= gridLines; ++i)
					{
						if (i > 0)
							Handles.DrawLine(new Vector2(xBorder, barTop + (lineSpacing * i)), new Vector2(Screen.width - xBorder, barTop + (lineSpacing * i)));

						if ((dataMax * (1 - ((lineSpacing * i) / (barFloor - barTop)))) > 0)
							GUI.Label(new Rect(0, barTop + (lineSpacing * i) - 8, xBorder - 2, 50), string.Format(axisFormatString, (dataMax * (1 - ((lineSpacing * i) / (barFloor - barTop))))) , labelTextStyle);
					}

					Handles.color = Color.white;
				}

				int c = 0;
				for (int i = 0; i < data.Length; ++i)
				{
					if (data[i] != null)
					{
						DrawLine (data[i], colors[c++], i < dataLabels.Count ? dataLabels[i] : string.Empty);
					
						if (c > colors.Count - 1)
							c = 0;
					}
				}
				
				// Draw Axis.
				Handles.color = axisColor;
				Handles.DrawLine(new Vector2(xBorder, barTop), new Vector2(xBorder, barFloor));
				Handles.DrawLine(new Vector2(xBorder, barFloor), new Vector2(Screen.width - xBorder, barFloor));

        GUIStyle centeredStyle = new()
        {
            alignment = TextAnchor.UpperCenter
        };
        centeredStyle.normal.textColor = fontColor;
				
				// Draw ticks and labels.
				for (int i = 0; i < data[0].Count; ++i)
				{
					if (i > 0 && drawTicks)
						Handles.DrawLine(new Vector2(xBorder + (lineWidth * i), barFloor - 3), new Vector2(xBorder + (lineWidth * i), barFloor + 3));
				
					if (i < axisLabels.Count)
					{
						Rect labelRect = new Rect(xBorder + (lineWidth * i) - lineWidth / 2.0f, barFloor + 5, lineWidth, 16);			
						GUI.Label(labelRect, axisLabels[i], centeredStyle);				
					}
				}
				
				Handles.color = Color.white;
			}
		}
		
		private void DrawLine(List<float> data, Color color, string label)
		{
			Vector2 previousLine = Vector2.zero;
			Vector2 newLine;
			Handles.color = color;
			
			for (int i = 0; i < data.Count; ++i)
			{
				float lineTop = barFloor - ((barFloor - barTop) * (data[i] / dataMax));
				newLine = new Vector2(xBorder + (lineWidth * i), lineTop);

				if (i > 0)
					Handles.DrawAAPolyLine(previousLine, newLine);

				previousLine = newLine;
				Rect selectRect = new Rect((previousLine - (Vector2.up * 0.5f)).x - pipRadius * 3, (previousLine - (Vector2.up * 0.5f)).y - pipRadius * 3, pipRadius * 6, pipRadius * 6);
				
				if (selectRect.Contains(Event.current.mousePosition))
				{
          GUIStyle centeredStyle = new()
          {
              alignment = TextAnchor.UpperCenter
          };
          centeredStyle.normal.textColor = fontColor;
					Handles.DrawSolidDisc(previousLine - (Vector2.up * 0.5f), Vector3.forward, pipRadius * 2);
					
					if (valueViewMode == ViewMode.ON_SELECT)
					{
						selectRect.y -= 16; selectRect.width += 50; selectRect.x -= 25;
						GUI.Label(selectRect, string.Format(formatString, data[i]), centeredStyle);				
					}
		
					if (window != null)
						window.Repaint();
				}
				else
					Handles.DrawSolidDisc(previousLine - (Vector2.up * 0.5f), Vector3.forward, pipRadius);
			}
			
			if (label != null)
			{
				GUIStyle colorStyle = new();
				colorStyle.normal.textColor = color;
				
        Rect labelRect = new(previousLine.x + 8, previousLine.y - 8, 100, 16);			
				GUI.Label(labelRect, label, colorStyle);				
			}
		}
	}
}
