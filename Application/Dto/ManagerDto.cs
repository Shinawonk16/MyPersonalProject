using Domain.Enum;
using Microsoft.AspNetCore.Http;

namespace Application.Dto;

public class ManagerDto
    {
        public UserDto UserDto { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }


    }
    public class CreateManagerRequestModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
    }

    public class CompleteManagerRegistrationRequestModel
    {
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email {get; set;}  
        public string Password { get; set; }
        public IFormFile ProfileImage { get; set; }
        public Gender Gender { get; set; }

    }

    public class UpdateManagerRequestModel
    {
        public string FirstName { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public IFormFile ProfilePicture { get; set; }
    }

