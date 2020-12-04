namespace API.Helpers
{
    public class UserParams : PaginationParams
    {
        public string CurrentUserName { get; set; }
        public string Gender { get; set; }
        public string PrintStatus { get; set; }
        public string SearchUser { get; set; }
        public int SearchUserID { get; set; }
        public string SearchPrinter { get; set; }
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 150;
        public string OrderBy { get; set; } = "lastActive";
    }
}