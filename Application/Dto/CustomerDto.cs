using Domain.Enum;
using Microsoft.AspNetCore.Http;

namespace Application.Dto;

public class CustomerDto
    {
        public UserDto Users { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateCustomerRequestModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public IFormFile ProfilePicture{get;set;}
        public string Password { get; set; }
        public Gender Gender { get; set; }
    
    }

    public class UpdateCustomerRequestModel
    {
       public string FirstName{get;set;}
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public IFormFile ProfilePicture { get; set; }
    }

