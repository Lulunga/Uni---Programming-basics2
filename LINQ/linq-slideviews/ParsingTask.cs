using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace linq_slideviews
{
    public class ParsingTask
    {
        /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
        /// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
        /// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
        public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
        {
            return lines
                .Skip(1)
                .Select(line => line.Split(';'))
                .Select(MakeSlideRecord)
                .Where(slideRecord => slideRecord != null)
                .ToDictionary(slideRecord => slideRecord.SlideId);
        }

        public static SlideRecord MakeSlideRecord(string[] arr)
        {
            return arr.Length != 3
                 || !int.TryParse(arr[0], out int slideId)
                 || !Enum.TryParse(arr[1], true, out SlideType slideType)
                 ? null : new SlideRecord(slideId, slideType, arr[2]);
        }

        /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
        /// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
        /// Такой словарь можно получить методом ParseSlideRecords</param>
        /// <returns>Список информации о посещениях</returns>
        /// <exception cref="FormatException">Если среди строк есть некорректные</exception>
        public static IEnumerable<VisitRecord> ParseVisitRecords(
        IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
        {
            return lines.Skip(1).Select(line =>
            {
                var data = line.Split(new[] { ';' }, 3, StringSplitOptions.RemoveEmptyEntries);
                return MakeVisitRecord(data, slides, line);
            });
        }

        public static VisitRecord MakeVisitRecord(string[] arr, IDictionary<int, SlideRecord> slides, string line)
        {
            return arr.Length != 3 ||
             !int.TryParse(arr[0], out int userID) ||
             !int.TryParse(arr[1], out int slideID) ||
             !slides.TryGetValue(slideID, out SlideRecord slideRecord) ||
             !DateTime.TryParseExact(arr[2], "yyyy-MM-dd;HH:mm:ss",
                                     CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime)
             ? throw new FormatException($"Wrong line [{line}]")
             : new VisitRecord(userID, slideID, dateTime, slideRecord.SlideType);
        }
    }
}