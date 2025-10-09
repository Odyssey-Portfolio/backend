using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyPortfolio_Libraries.Helpers
{
    public static class EntityUtils
    {
        public static string GenerateEntityId<T>()
        {
            string typeName = typeof(T).Name;
            // Extract initials from PascalCase words
            string typePrefix = string.Concat(
                System.Text.RegularExpressions.Regex
                    .Matches(typeName, @"[A-Z][a-z]*")
                    .Select(m => m.Value[0])
            );
            string dateTimePart = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss"); 
            return $"{typePrefix}_{dateTimePart}";
        }
    }
}
