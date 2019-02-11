namespace ViWork
{
    public class UserData
    {
        public int UserId { get;}

        public string Email { get; set; }

        public byte[] Password { get; set; }

        public string GithubAccessToken { get; set; }
        
        public string GoogleRefreshToken { get; set; }

        public string GoogleId { get; set; }

        public int GithubId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
