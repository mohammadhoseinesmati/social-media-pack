using System.Text.RegularExpressions;

namespace social_media.Data.Helpers
{
    public class HashtagHelper
    {
        public static List<string> GetHashtags(string gethashtagpost)
        {
            var hashtagPattern = new Regex(@"#\w+");
            var matchs = hashtagPattern.Matches(gethashtagpost)
                .Select(p => p.Value.TrimEnd('.' , '!' , ',' , '?'))
                .Distinct()
                .ToList();
            return matchs;
                
        }
    }
}
