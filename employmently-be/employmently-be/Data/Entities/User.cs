using employmently_be.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace employmently_be.Entities
{
    public class User : IdentityUser
    {
        public string? EmailConfirmationToken { get; set; }
        public virtual Company? Company { get; set; }

    }
}
