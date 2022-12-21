using employmently_be.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace employmently_be.Entities
{
    public class User : IdentityUser
    {
        public string? UniqueIdentifierCompany { get; set; }
        public virtual Company? Company { get; set; }
        public string? Description { get; set; }

        public string ProfilePicture { get; set; } = "https://employmentlystorage.blob.core.windows.net/fileupload/default_profilepic.png";

    }
}
