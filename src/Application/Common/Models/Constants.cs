namespace tm20trial.Application.Common.Models;

public static class Constants
{
    public static class UserRoles
    {
        public const string Administrator = "Administrator";
        public const string Player = "Player";
        public const string Mapper = "Mapper";
    }
    
    public static class UserCustomClaims
    {
        public const string UserId = "UserId";
    }
    
    public static class UserPolicies
    {
        public const string AdministratorPolicy = "AdministratorPolicy";
        public const string PlayerPolicy = "PlayerPolicy";
        public const string MapperPolicy = "MapperPolicy";
        public const string ConnectedPolicy = "ConnectedPolicy";
    }
}
