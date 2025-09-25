using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Models
{
    public class UserRoleViewModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string RoleId { get; set; }

    }
}
