using System;

namespace SMEAppHouse.Core.AppMgt.AuthMgr.Models
{
    public class UserGrant
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public bool Disabled { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int ClientId { get; set; }
        public string OptOutEmail { get; set; }
        public string AllowedCountries { get; set; }
        public string AuthToken { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public string AvatarUrl { get; set; }
        public string Language_ID { get; set; }
        public string PersistentToken { get; set; }
    }
}
