using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utils.Path
{
	/// <summary>
	/// Описание одного сегмента пути
	/// </summary>
	class PathSegment
	{
		/// <summary>
		/// Имя сегмента
		/// </summary>
		public string SegmentName;
		/// <summary>
		/// Имя используемого генератора пути
		/// </summary>
		public string PathGeneratorName;
		/// <summary>
		/// Список опорных точек
		/// </summary>
		public List<Point> BasePoints;
		/// <summary>
		/// Количество генерируемых точек
		/// </summary>
		public int Count;
	}
}
