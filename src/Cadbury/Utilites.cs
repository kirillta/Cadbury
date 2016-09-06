using System.Linq;

namespace Floxdc.Cadbury
{
    public class Utilites
    {
        public static string GetCsvPath(string[] args)
            => args.FirstOrDefault() == null ? string.Empty : args.First();
    }
}
