using System.Text.Json;

namespace F1FantasySim.Util
{
    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            // Convert snake_case to camelCase
            if (string.IsNullOrEmpty(name)) return name;

            var parts = name.Split('_');
            for (int i = 1; i < parts.Length; i++)
            {
                if (parts[i].Length > 0)
                {
                    parts[i] = char.ToUpper(parts[i][0]) + parts[i].Substring(1);
                }
            }
            return string.Join(string.Empty, parts);
        }
    }

}
