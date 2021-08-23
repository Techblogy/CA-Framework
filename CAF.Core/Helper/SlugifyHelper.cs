using Slugify;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.Helper
{
    public static class DeryaSlugifyHelper
    {
        public static string ToUrl(string text, int id)
        {
            SlugHelper helper = new SlugHelper();

            var response = helper.GenerateSlug($"{text}-{id}");
            return response;
        }
    }
}
