namespace YawnMassage.Platform.Services.Helpers
{
    public class ConfigurationHelper
    {
        public static int GetConfigurationPriority(string group, string culture, string section)
        {
            var specificGroup = group != "*" ? 1 : 0;
            var specificCulture = culture != "*" ? 1 : 0;
            var specificSection = section != "*" ? 1 : 0;

            var priority = ((4 * specificGroup) | (2 * specificCulture) | (1 * specificSection)) ^ 7;

            //To start it from 1 instead of 0
            return priority + 1;
        }
    }
}
