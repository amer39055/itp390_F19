using System;
using System.Collections.Generic;

namespace ECommerce.Models
{
    public partial class AspNetUsers
    {
        public AspNetUsers()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaims>();
            AspNetUserLogins = new HashSet<AspNetUserLogins>();
            AspNetUserRoles = new HashSet<AspNetUserRoles>();
            AspNetUserTokens = new HashSet<AspNetUserTokens>();
            Dispute = new HashSet<Dispute>();
            Order = new HashSet<Order>();
            Sprovider = new HashSet<Sprovider>();
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }

        public string BirthDate { get; set; }
        public string NationalId { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string HomeLocation { get; set; }
        public string Gender { get; set; }
        public DateTime RegisteredDate { get; set; }
        public string Status { get; set; }
        public string UserType { get; set; }

        public ICollection<AspNetUserClaims> AspNetUserClaims { get; set; }
        public ICollection<AspNetUserLogins> AspNetUserLogins { get; set; }
        public ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
        public ICollection<AspNetUserTokens> AspNetUserTokens { get; set; }
        public ICollection<Dispute> Dispute { get; set; }
        public ICollection<Order> Order { get; set; }
        public ICollection<Sprovider> Sprovider { get; set; }
    }
}
