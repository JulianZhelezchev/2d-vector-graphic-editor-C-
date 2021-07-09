using Draw.src.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Draw
{
	/// <summary>
	/// Класът, който ще бъде използван при управляване на диалога.
	/// </summary>
	public class DialogProcessor : DisplayProcessor
	{
		#region Constructor
		
		public DialogProcessor()
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Избран елемент.
		/// </summary>
		private List<Shape> selection = new List<Shape>();
		public List<Shape> Selection
		{
			get { return selection; }
			set { selection = value; }
		}

		//private Shape selectionMenu;
		//public Shape SelectionMenu
		//{
		//	get { return selectionMenu; }
		//	set { selectionMenu = value; }
		//}

		//private PointF coppy;
		//public PointF Coppy
		//{
		//	get { return coppy; }
		//	set { coppy = value; }
		//}



		/// <summary>
		/// Дали в момента диалога е в състояние на "влачене" на избрания елемент.
		/// </summary>
		private bool isDragging;
		public bool IsDragging
		{
			get { return isDragging; }
			set { isDragging = value; }
		}

		/// <summary>
		/// Последна позиция на мишката при "влачене".
		/// Използва се за определяне на вектора на транслация.
		/// </summary>
		private PointF lastLocation;
		public PointF LastLocation
		{
			get { return lastLocation; }
			set { lastLocation = value; }
		}

		#endregion

		/// <summary>
		/// Добавя примитив - правоъгълник на произволно място върху клиентската област.
		/// </summary>
		public void AddRandomRectangle()
		{
			Random rnd = new Random();
			int x = rnd.Next(100,1000);
			int y = rnd.Next(100,600);
			
			RectangleShape rect = new RectangleShape(new Rectangle(x,y,100,200));
			rect.FillColor = Color.White;
			rect.BorderColor = Color.Black;
			rect.Opacity = 255;
			rect.BorderWidth = 1;

			ShapeList.Add(rect);
		}

		public void AddRandomEllipse()
        {
			Random rnd = new Random();
			int x = rnd.Next(100, 1000);
			int y = rnd.Next(100, 600);

			EllipseShape elp = new EllipseShape(new Rectangle(x, y, 100, 200));
			elp.FillColor = Color.White;
			elp.BorderColor = Color.Black;
			elp.Opacity = 255;

			ShapeList.Add(elp);
		}

		public void AddRandomTriangle()
        {
			Random rnd = new Random();
			int x = rnd.Next(100, 1000);
			int y = rnd.Next(100, 600);

			TriangleShape tri = new TriangleShape(new Rectangle(x, y, 100, 200));
			tri.FillColor = Color.White;
			tri.BorderColor = Color.Black;
			tri.Opacity = 255;

			ShapeList.Add(tri);
		}

		public void AddRandomExamFigure0()
		{
			Random rnd = new Random();
			int x = rnd.Next(100, 1000);
			int y = rnd.Next(100, 600);

			ExamFigure0 examFigure0 = new ExamFigure0(new Rectangle(x, y, 250, 170));
			examFigure0.FillColor = Color.White;
			examFigure0.BorderColor = Color.Black;
			examFigure0.Opacity = 255;

			ShapeList.Add(examFigure0);
		}

	


		/// <summary>
		/// Проверява дали дадена точка е в елемента.
		/// Обхожда в ред обратен на визуализацията с цел намиране на
		/// "най-горния" елемент т.е. този който виждаме под мишката.
		/// </summary>
		/// <param name="point">Указана точка</param>
		/// <returns>Елемента на изображението, на който принадлежи дадената точка.</returns>
		public Shape ContainsPoint(PointF point)
		{
			for(int i = ShapeList.Count - 1; i >= 0; i--){
				if (ShapeList[i].Contains(point)){
					ShapeList[i].FillColor = Color.Red;
						
					return ShapeList[i];
				}	
			}
			return null;
		}

		/// <summary>
		/// Транслация на избраният елемент на вектор определен от <paramref name="p>p</paramref>
		/// </summary>
		/// <param name="p">Вектор на транслация.</param>
		public void TranslateTo(PointF p)
		{
			foreach (var item in Selection)
			{

				item.Move(p.X - lastLocation.X, p.Y - lastLocation.Y);
				item.Location = new PointF(item.Location.X + p.X - lastLocation.X, item.Location.Y + p.Y - lastLocation.Y);

			}
			lastLocation = p;
		}

		internal void SetSelectedFieldColor(Color color)
		{
			foreach (var item in Selection)

			{
				item.GroupFillColor(color);
				item.FillColor = color;
			}
		}

		internal void SetSelectedBorderColor(Color color)
		{
			foreach (var item in Selection)

			{
				item.GroupBorderColor(color);
				item.BorderColor = color;
			}
		}

		internal void SetOpacity(int opacity, string btn = " ")
		{
			foreach (var item in Selection)

			{
				item.GroupOpacity(opacity);
				item.Opacity = opacity;

			}
		}

		internal void SetBorderWidth(float borderWidth, string btn = " ")
		{
				foreach (var item in Selection)

				{
					item.GroupBorderWidth(borderWidth);
					item.BorderWidth = borderWidth;

				}
		}

		internal void SetSize(float width, float height, string btn = " ")
		{
			foreach (var item in Selection)
			{
				if (width != -1)
				{
					if (item.GetType().Equals(typeof(GroupShape)))
					{
						item.GroupReSizeWidth(width);
					}
					else
					{
						item.Width = width;
					}
				}
				if (height != -1)
				{
					if (item.GetType().Equals(typeof(GroupShape)))
					{
						item.GroupReSizeHeight(height);
					}
					else
					{
						item.Height = height;
					}
				}
			}
		}

		public void Rotate(float angle, string btn = " ")
		{
				if (Selection.Count != 0)
				{
					foreach (var item in Selection)
					{
						item.GroupRotate(angle);
						item.Angle = angle;

					}
				}
		}

		internal void DeleteSelected()
		{
			foreach (var item in Selection)
			{
				ShapeList.Remove(item);

			}
			Selection.Clear();
		}

		public void GroupSelected()
		{
			if (Selection.Count < 2) return;

			float minX = float.PositiveInfinity;
			float minY = float.PositiveInfinity;
			float maxX = float.NegativeInfinity;
			float maxY = float.NegativeInfinity;
			foreach (var item in Selection)
			{
				if (minX > item.Location.X)
				{
					minX = item.Location.X;
				}
				if (minY > item.Location.Y)
				{
					minY = item.Location.Y;
				}
				if (maxX < item.Location.X + item.Width)
				{
					maxX = item.Location.X + item.Width;
				}
				if (maxY < item.Location.Y + item.Height)
				{
					maxY = item.Location.Y + item.Height;
				}
			}
			var group = new GroupShape(new RectangleF(minX, minY, maxX - minX, maxY - minY));
			group.SubItem = Selection;
			foreach (var item in Selection)
			{
				ShapeList.Remove(item);
			}

			Selection = new List<Shape>();
			Selection.Add(group);
			ShapeList.Add(group);
		}

		public void ReGroup()
		{
			for (int i = 0; i < Selection.Count; i++)
			{
				if (Selection[i].GetType().Equals(typeof(GroupShape)))
				{
					GroupShape group = (GroupShape)Selection[i];

					ShapeList.AddRange(group.SubItem);
					group.SubItem.Clear();
					Selection[i] = null;
					group = null;
					ShapeList.Remove(Selection[i]);
					Selection.Remove(Selection[i]);


				}
			}
			GC.Collect();
		}
		internal void CopySelected()
		{
			MySerialize(Selection);
		}
		internal void PastSelected()
		{
			List<Shape> copyShapes = (List<Shape>)MyDeSerialize();
			ShapeList.AddRange(copyShapes);
		}
		public void MySerialize(object obj, string filePath = null)
		{
			IFormatter formatter = new BinaryFormatter();
			Stream stream;
			if (filePath != null)
			{
				stream = new FileStream(filePath + ".bin",
								  FileMode.Create);
			}
			else
			{
				stream = new FileStream("MyFile.bin",
										FileMode.Create,
										FileAccess.Write, FileShare.None);
			}
			formatter.Serialize(stream, obj);
			stream.Close();
		}
		public object MyDeSerialize(string filePath = null)
		{
			object obj;
			IFormatter formatter = new BinaryFormatter();
			Stream stream;
			if (filePath != null)
			{
				stream = new FileStream(filePath,
									 FileMode.Open,
									 FileAccess.Read, FileShare.None);
			}
			else
			{
				stream = new FileStream("MyFile.bin",
									FileMode.Open);
			}
			obj = formatter.Deserialize(stream);
			stream.Close();
			return obj;
		}
	}
}
