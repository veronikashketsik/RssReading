using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace RssReading.Infrastructure
{
    public class Sorting
    {
        public List<SortField> SortFields { get; set; }
        public static bool TryParse(string sort, out Sorting result)
        {
            var sortFields = new List<SortField>();
            try
            {
                if (!string.IsNullOrEmpty(sort))
                {
                    foreach (var s in sort.Split(','))
                    {
                        if (s.Contains(":"))
                        {
                            var pieces = s.Trim().Split(':');
                            sortFields.Add(new SortField
                            {
                                FieldName = pieces[0].ToLower(),
                                Direction = pieces[1].ToLower() == "asc" ? SortDirection.Ascending : SortDirection.Descending
                            });
                        }
                        else
                        {
                            sortFields.Add(new SortField
                            {
                                FieldName = s.Trim(),
                                Direction = SortDirection.Ascending
                            });
                        }
                    }
                }
            }
            catch
            {
                result = null;
                return false;
            }
            result = new Sorting { SortFields = sortFields };
            return true;
        }
    }
}