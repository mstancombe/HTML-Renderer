using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlRenderer.Core.Parse
{
    public class CssParserUtils
    {
        public static string[] ParseClasses(string classAttribValue)
        {
            if (string.IsNullOrEmpty(classAttribValue))
                return [];

            return classAttribValue.Split(' ');
        }
    }
}
