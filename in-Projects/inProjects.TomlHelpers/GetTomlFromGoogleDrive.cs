namespace inProjects.TomlHelpers
{
    public static class GetTomlFromGoogleDrive
    {
        public static string GetUrlRessource(string url)
        {
            string[] splitedUrl = url.Split("?");
            string path = splitedUrl[0];
            string query = splitedUrl[1];

            path = path.Replace("open", "uc");

            return path + "?" + query;
        }
    }
}
