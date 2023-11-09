using Application.Abstractions;
using Application.Abstractions.IRepository;
using Application.Abstractions.IService;
using Application.Dto;
using Domain.Entities;

namespace Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IVerificationRepository _verificationRepository;
    private readonly IEmailService _emailService;
    private readonly IShopezyUpload _imageRepository;


    public CustomerService(ICustomerRepository customerRepository, IUserRepository userRepository = null, IRoleRepository roleRepository = null, IVerificationRepository verificationRepository = null, IEmailService emailService = null, IShopezyUpload imageRepository = null)
    {
        _customerRepository = customerRepository;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _verificationRepository = verificationRepository;
        _emailService = emailService;
        _imageRepository = imageRepository;
    }

    public async Task<BaseResponse<CustomerDto>> CreateCustomerAsync(CreateCustomerRequestModel model)
    {
        int code = new Random().Next(100000, 999999);
        var get = await _customerRepository.CheckIfEmailExistAsync(model.Email);
        if (get == false)
        {
            var password = BCrypt.Net.BCrypt.HashPassword(model.Password);
            var image = await _imageRepository.UploadFiles(model.ProfilePicture);
            var customer = new Customer
            {
                User = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = password,
                    PhoneNumber = model.PhoneNumber,
                    ProfilePicture = image,
                    Gender = model.Gender,
                    UserName = $"{model.FirstName} {model.LastName}",


                }
            };
            var role = await _roleRepository.GetRoleAsync(x => x.RoleName == "customer");
            if (role == null)
            {
                return new BaseResponse<CustomerDto>
                {
                    Message = "Role not found",
                    Status = false
                };
            }
            var create = await _customerRepository.CreateAsync(customer);
            await _customerRepository.SaveAsync();
            var userRole = new UserRole
            {
                UserId = create.User.Id,
                RoleId = role.Id
            };
            create.User.UserRoles.Add(userRole);
            await _customerRepository.UpdateAsync(create);
            var verification = new Verification
            {
                Code = code,
                CustomerId = create.Id

            };
            var test = await _verificationRepository.CreateAsync(verification);
            await _verificationRepository.SaveAsync();
            var mailRequest = new MailRequest
            {
                Subject = "Confirmation Code",
                ToEmail = customer.User.Email,
                ToName = $"{customer.User.FirstName} {customer.User.LastName}",
                HtmlContent = $"<html><body><h1>Hello {customer.User.FirstName} {customer.User.LastName}, Welcome to Shopezy...</h1><h4>Your confirmation code is {verification.Code} to continue with the registration</h4></body></html>",
            };
            _emailService.SendEMailAsync(mailRequest);

            return new BaseResponse<CustomerDto>
            {
                Message = "Added Successfully",
                Status = true,
                Data = new CustomerDto
                {
                    Users = new UserDto
                    {
                        Id = create.Id,
                        FirstName = customer.User.FirstName,
                        LastName = customer.User.LastName,
                        Email = customer.User.Email,
                        PhoneNumber = customer.User.PhoneNumber,
                        ProfilePicture = customer.User.ProfilePicture,
                        Gender = customer.User.Gender,
                        UserName = $"{customer.User.FirstName} {customer.User.LastName}"
                    }
                }
            };
        }
        return new BaseResponse<CustomerDto>
        {
            Message = "Already exist",
            Status = false
        };
    }

    public async Task<BaseResponse<CustomerDto>> DeleteAsync(string id)
    {
        var get = await _customerRepository.GetAsync(id);
        if (get != null)
        {
            get.IsDeleted = true;
            get.DeletedBy = "Super-Admin";
            await _customerRepository.SaveAsync();
            return new BaseResponse<CustomerDto>
            {
                Message = "Successfully deleted",
                Status = true,
            };
        }
        return new BaseResponse<CustomerDto> { Message = "Fail to deleted", Status = false, };
    }

    public async Task<BaseResponse<IEnumerable<CustomerDto>>> GetAllAsync()
    {
        var get = await _customerRepository.GetAllCustomerAsync();
        if (get != null)
        {
            return new BaseResponse<IEnumerable<CustomerDto>>
            {
                Message = "Successful",
                Status = true,
                Data = get.Select(x => new CustomerDto
                {
                    Users = new UserDto
                    {
                        UserName = $"{x.User.FirstName} {x.User.LastName}",
                        Email = x.User.Email,
                        PhoneNumber = x.User.PhoneNumber,
                        ProfilePicture = x.User.ProfilePicture,
                        Gender = x.User.Gender,
                        Role = "Customer",
                        RoleDescription = "Buy Products",

                    }
                }).ToList()
            };
        }
        return new BaseResponse<IEnumerable<CustomerDto>>
        {
            Message = "Failed",
            Status = false,

        };
    }

    public async Task<BaseResponse<CustomerDto>> GetByEmailAsync(string email)
    {
        var get = await _customerRepository.GetAsync(x => x.User.Email == email);
        if (get != null)
        {
            return new BaseResponse<CustomerDto>
            {
                Message = "Successful",
                Status = true,
                Data = new CustomerDto
                {
                    Users = new UserDto
                    {
                        UserName = $"{get.User.FirstName} {get.User.LastName}",
                        Email = get.User.Email,
                        PhoneNumber = get.User.PhoneNumber,
                        ProfilePicture = get.User.ProfilePicture,
                        Gender = get.User.Gender
                    }
                }
            };
        }
        return new BaseResponse<CustomerDto>
        {
            Message = "Failed",
            Status = false,
        };
    }

    public async Task<BaseResponse<CustomerDto>> GetByIdAsync(string id)
    {
        var get = await _customerRepository.GetAsync(x => x.User.Id == id);
        if (get != null)
        {
            return new BaseResponse<CustomerDto>
            {
                Message = "Successful",
                Status = true,
                Data = new CustomerDto
                {
                    Users = new UserDto
                    {
                        UserName = $"{get.User.FirstName} {get.User.LastName}",
                        Email = get.User.Email,
                        PhoneNumber = get.User.PhoneNumber,
                        ProfilePicture = get.User.ProfilePicture,
                        Gender = get.User.Gender
                    }
                }
            };
        }
        return new BaseResponse<CustomerDto>
        {
            Message = "Failed",
            Status = false,
        };
    }

    public Task<BaseResponse<IEnumerable<CustomerDto>>> GetSelectedAsync()
    {

        throw new NotImplementedException();
    }

    public async Task<BaseResponse<CustomerDto>> UpdateCustomerAsync(string id, UpdateCustomerRequestModel model)
    {
        var get = await _customerRepository.GetAsync(x => x.Id == id);
        if (get != null)
        {
            var image = await _imageRepository.UploadFiles(model.ProfilePicture);

            get.User.FirstName = model.FirstName.ToLower() ?? get.User.FirstName;
            get.User.LastName = model.LastName.ToLower() ?? get.User.LastName;
            get.User.Email = model.Email.ToLower() ?? get.User.Email;
            get.User.UserName = $"{model.FirstName} {model.LastName}" ?? get.User.UserName;
            get.User.ProfilePicture = image ?? get.User.ProfilePicture;
            get.User.PhoneNumber = model.PhoneNumber ?? get.User.PhoneNumber;
            await _customerRepository.UpdateAsync(get);
            await _customerRepository.SaveAsync();
            return new BaseResponse<CustomerDto>
            {
                Message = "Updated Successfully",
                Status = true,
                Data = new CustomerDto
                {
                    Users = new UserDto
                    {
                        FirstName = get.User.FirstName,
                        LastName = get.User.LastName,
                        Email = get.User.Email,
                        PhoneNumber = get.User.PhoneNumber,
                        ProfilePicture = get.User.ProfilePicture,
                        UserName = $"{get.User.FirstName} {get.User.LastName}"
                    }
                }
            };
        }
        return new BaseResponse<CustomerDto>
        {
            Message = "Failed",
            Status = false,
        };
    }
}

