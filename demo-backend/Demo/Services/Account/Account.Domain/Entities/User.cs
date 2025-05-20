using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Domain.Common.Constants;

namespace Account.Domain.Entities
{
    public partial class User
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public string? AvatarUrl { get; set; }

        public string Status { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string DisplayName { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        public bool IsAdmin { get; set; }

        public virtual ICollection<UserRelationship> UserRelationshipAddressees { get; set; } = new List<UserRelationship>();

        public virtual ICollection<UserRelationship> UserRelationshipRequesters { get; set; } = new List<UserRelationship>();
    }
}
