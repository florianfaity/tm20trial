using tm20trial.Domain.Entities;

namespace tm20trial.Domain.Interfaces
{
    public interface IApplicationUser
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public Users UserDetails { get; set; }
    }
}