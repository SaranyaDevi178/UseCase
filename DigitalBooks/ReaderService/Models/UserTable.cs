using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReaderService.Models
{
    public partial class UserTable
    {
        public UserTable()
        {
            Books = new HashSet<Book>();
        }

        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? EmailId { get; set; }
        public string? Password { get; set; }
        private int? roleId;
        [Required]
        public int? RoleId { get { return roleId; } set { roleId = 2; } }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool? Active { get; set; }

        public virtual RoleMaster? Role { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
