namespace SkillSnap.Client.Services
{
    public class UserSessionService
    {
        public string? UserId { get; set; }
        public string? Role { get; set; }

        public string? CurrentProjectId { get; set; }
        public string? CurrentEditingSecion { get; set; }

        public void ClearSession()
        {
            UserId = null;
            Role = null;
            CurrentProjectId = null;
            CurrentEditingSecion = null;
        }
    }
}