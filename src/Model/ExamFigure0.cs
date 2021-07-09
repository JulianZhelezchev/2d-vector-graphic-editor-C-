using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draw.src.Model
{
	class ExamFigure0 : Shape
	{
		#region Constructor

		public ExamFigure0(RectangleF tri) : base(tri)
		{
		}

		public ExamFigure0(TriangleShape triangle) : base(triangle)
		{
		}

		#endregion

		/// <summary>
		/// Проверка за принадлежност на точка point към правоъгълника.
		/// В случая на правоъгълник този метод може да не бъде пренаписван, защото
		/// Реализацията съвпада с тази на абстрактния клас Shape, който проверява
		/// дали точката е в обхващащия правоъгълник на елемента (а той съвпада с
		/// елемента в този случай).
		/// </summary>
		public override bool Contains(PointF point)
		{
			if (base.Contains(point))
				// Проверка дали е в обекта само, ако точката е в обхващащия правоъгълник.
				// В случая на правоъгълник - директно връщаме true
				return true;
			else
				// Ако не е в обхващащия правоъгълник, то неможе да е в обекта и => false
				return false;
		}

		/// <summary>
		/// Частта, визуализираща конкретния примитив.
		/// </summary>
		public override void DrawSelf(Graphics grfx)
		{
			base.DrawSelf(grfx);
			base.RotateShape(grfx);

			Point[] p = { new Point((int)Rectangle.X + ((int)Rectangle.Width / 2), (int)Rectangle.Y), new Point((int)Rectangle.X, (int)(Rectangle.Y + Rectangle.Height)), new Point((int)(Rectangle.X + Rectangle.Width), (int)(Rectangle.Y + Rectangle.Height)) };
			grfx.FillPolygon(new SolidBrush(Color.FromArgb(Opacity, FillColor)), p);
			grfx.DrawPolygon(new Pen(BorderColor, BorderWidth), p);

			//Center point
			Point center = new Point((int)(Rectangle.X + Rectangle.Width / 2), (int)(Rectangle.Y + Rectangle.Height / 2));

			//From top to center
			Point p1 = new Point((int)Rectangle.X + ((int)Rectangle.Width / 2), (int)Rectangle.Y);
			grfx.DrawLine(new Pen(BorderColor, BorderWidth), p1, center);

			//From left to center
			Point p2 = new Point((int)Rectangle.X, (int)(Rectangle.Y + Rectangle.Height));
			grfx.DrawLine(new Pen(BorderColor, BorderWidth), p2, center);

			//From right to ceneter
			Point p3 = new Point((int)(Rectangle.X + Rectangle.Width), (int)(Rectangle.Y + Rectangle.Height));
			grfx.DrawLine(new Pen(BorderColor, BorderWidth), p3, center);

			grfx.ResetTransform();
		}
	}
}
