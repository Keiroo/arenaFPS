using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ArenaFPS.Scripts
{
    public static class StaticMethods
    {
        public static int[] ConvertToIntArray(this string str)
        {
            HashSet<int> values = new();

            string cleanedInput = Regex.Replace(str, @"[^\d,-]", "");
            string[] parts = cleanedInput.Split(',');

            foreach (string part in parts)
            {
                if (part.Contains('-'))
                {
                    string[] rangeParts = part.Split('-');
                    if (rangeParts.Length != 2) continue;

                    if (!int.TryParse(rangeParts[0], out int start) ||
                        !int.TryParse(rangeParts[1], out int end)) continue;
                    if (start > end) continue;

                    for (int i = start; i <= end; i++)
                        values.Add(i);
                }
                else
                {
                    if (int.TryParse(part, out var number))
                        values.Add(number);
                }
            }

            var res = values.ToArray();
            Array.Sort(res);
            return res;
        }
    }
}
