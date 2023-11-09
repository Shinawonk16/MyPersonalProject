using System.Security.Cryptography.X509Certificates;
using Application.Abstractions.IRepository;
using Application.Abstractions.IService;
using Application.Dto;
using Domain.Entities;

namespace Application.Services;

public class OrderServices : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IOrderProductRepository _orderProductRepository;
    private readonly ISaleRepository _saleRepository;
        private readonly ISaleService _saleServices;

    private readonly IPaymentService _paymentService;

    public OrderServices(IOrderRepository orderRepository, IProductRepository productRepository, ICustomerRepository customerRepository, IOrderProductRepository orderProductRepository, ISaleRepository saleRepository, IPaymentService paymentService, ISaleService saleServices = null)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _orderProductRepository = orderProductRepository;
        _saleRepository = saleRepository;
        _paymentService = paymentService;
        _saleServices = saleServices;

    }


    public async Task<BaseResponse<OrderDto>> CreateOrderAsync(CreateOrderRequestModel model, string customerId)
    {
        var customer = await _customerRepository.GetAsync(customerId);
        if (customer == null)
        {
            return new BaseResponse<OrderDto> { Message = "Failed asd", Status = false };
        }


        if (model.OrderRequestModels != null)
        {
            foreach (var productId in model.OrderRequestModels)
            {

                var notAvailableCount = 0;
                var notFoundCount = 0;
                var product = await _productRepository.GetAsync(productId.ProductId);
                if (product is null)
                {
                    notFoundCount++;
                    continue;
                }
                else if (product.Quantity < productId.Quantity)
                {
                    notAvailableCount++;
                    continue;
                }
                product.Quantity -= productId.Quantity;

                await _productRepository.SaveAsync();
            }
            var ord = new Order
            {
                CustomerId = customer.Id,
                IsDelivered = false,
                Address = new Address
                {
                    PostalCode = model.Address.PostalCode,
                    City = model.Address.City,
                    State = model.Address.State,
                    Street = model.Address.Street,
                },
            };
            var check = await _orderRepository.CreateAsync(ord);
            foreach (var item in model.OrderRequestModels)
            {
                var see = await _productRepository.GetAsync(item.ProductId);
                var orderProduct = new OrderProduct
                {
                    ProductId = item.ProductId,
                    OrderId = check.Id,
                    Quantity = item.Quantity,
                    AmountPaid = item.Quantity * see.Price
                    

                };
                await _orderProductRepository.CreateAsync(orderProduct);
            }
            await _orderProductRepository.SaveAsync();
            await _orderRepository.SaveAsync();
            if (check == null)
            {
                return new BaseResponse<OrderDto> { Message = "Failed to Create", Status = false };
            }
            return new BaseResponse<OrderDto>
            {
                Message = "Success",
                Status = true,
                Data = new OrderDto
                {
                    Id = check.Id,
                    IsDelivered = check.IsDelivered
                }
            };
        }
        return new BaseResponse<OrderDto>
        {
            Message = "Order can be Empty",
            Status = false
        };


    }

    public async Task<BaseResponse<IEnumerable<OrderProductDto>>> GetAllDeliveredOrdersAsync()
    {
        var getAll = await _orderRepository.GetSelectedAsync(x => x.IsDelivered == true);
        if (getAll.Count() != 0)
        {
            List<OrderProductDto> allOrders = new List<OrderProductDto>();
            foreach (var order in getAll)
            {
                var productOrders = await _orderProductRepository.GetAsync(
                    x => x.Order.Id == order.Id
                );
                var orderProduct = new OrderProductDto
                {
                    AddressDto = new AddressDto
                    {
                        State = productOrders[0].Order.Address.State,
                        City = productOrders[0].Order.Address.City,
                        Street = productOrders[0].Order.Address.Street,
                        PostalCode = productOrders[0].Order.Address.PostalCode,
                    },
                    OrderDto = productOrders
                        .Select(
                            x =>
                                new OrderDto
                                {
                                    Product = new ProductDto
                                    {
                                        ProductName = x.Product.ProductName,
                                        Price = x.Product.Price,
                                        Image = x.Product.Image,
                                        IsAvailable = x.Product.IsAvailable,
                                        Description = x.Product.Description
                                    },
                                    Customer = new CustomerDto
                                    {
                                        Users = new UserDto
                                        {
                                            UserName =
                                                $"{productOrders[0].Order.Customer.User.FirstName} {productOrders[0].Order.Customer.User.LastName}",
                                            PhoneNumber = productOrders[0]
                                                .Order
                                                .Customer
                                                .User
                                                .PhoneNumber,
                                            Email = productOrders[0].Order.Customer.User.Email,
                                            ProfilePicture = productOrders[0]
                                                .Order
                                                .Customer
                                                .User
                                                .ProfilePicture
                                        }
                                    },
                                    Quantity = x.Quantity,
                                    OrderedDate = x.Order.CreatedAt.ToLongDateString(),
                                    IsDelivered = productOrders[0].Order.IsDelivered,
                                    Id = order.Id
                                }
                        )
                        .ToList(),
                };
                allOrders.Add(orderProduct);
            }

            return new BaseResponse<IEnumerable<OrderProductDto>>
            {
                Message = "Successful",
                Status = true
            };
        }
        return new BaseResponse<IEnumerable<OrderProductDto>>
        {
            Message = "Failed",
            Status = false
        };

    }

    public async Task<BaseResponse<IEnumerable<OrderProductDto>>> GetAllOrdersAsync()
    {
        var orders = await _orderProductRepository.GetAllOrderAsync();
        if (orders == null)
        {
            return new BaseResponse<IEnumerable<OrderProductDto>>
            {
                Message = "Orders not found",
                Status = false
            };
        }
        List<OrderProductDto> allOrders = new List<OrderProductDto>();
        foreach (var order in orders)
        {
            var orderProducts = await _orderProductRepository.GetAsync(
                x => x.Order.Id == order.Order.Id
            );

            var orderProduct = new OrderProductDto
            {
                OrderDto = orderProducts
                    .Select(
                        x =>
                            new OrderDto
                            {
                                Product = new ProductDto
                                {
                                    ProductName = x.Product.ProductName,
                                    Price = x.Product.Price,
                                    Image = x.Product.Image,
                                    IsAvailable = x.Product.IsAvailable,
                                    Description = x.Product.Description
                                },
                                Customer = new CustomerDto
                                {
                                    Users = new UserDto
                                    {
                                        UserName =
                                            $"{orderProducts[0].Order.Customer.User.FirstName} {orderProducts[0].Order.Customer.User.LastName}",
                                        PhoneNumber = orderProducts[0]
                                            .Order
                                            .Customer
                                            .User
                                            .Email,
                                        Email = orderProducts[0].Order.Customer.User.Email,
                                        ProfilePicture = orderProducts[0]
                                            .Order
                                            .Customer
                                            .User
                                            .ProfilePicture
                                    }
                                },
                                Quantity = x.Quantity,
                                OrderedDate = x.Order.CreatedAt.ToLongDateString(),
                                IsDelivered = orderProducts[0].Order.IsDelivered,
                                Id = order.Id
                            }
                    )
                    .ToList(),
                AddressDto = new AddressDto
                {
                    AddressId = order.Id,
                    State = orderProducts[0].Order.Address.State,
                    City = orderProducts[0].Order.Address.City,
                    Street = orderProducts[0].Order.Address.Street,
                    PostalCode = orderProducts[0].Order.Address.PostalCode,
                },
            };
            allOrders.Add(orderProduct);
        }
        return new BaseResponse<IEnumerable<OrderProductDto>>
        {
            Message = "Orders found successfully",
            Status = true,
            Data = allOrders
                .Select(
                    x =>
                        new OrderProductDto
                        {
                            AddressDto = x.AddressDto,
                            OrderDto = x.OrderDto,
                            AmountPaid = x.OrderDto.Sum(x => x.Quantity * x.Product.Price),
                        }
                )
                .ToList()
        };

    }

    public async Task<BaseResponse<IEnumerable<OrderProductDto>>> GetAllUnDeliveredOrdersAsync()
    {
        var orders = await _orderProductRepository.GetSelectedAsync(
            x => x.Order.IsDelivered == false
        );
        if (orders.Count() == 0)
        {
            return new BaseResponse<IEnumerable<OrderProductDto>>
            {
                Message = "Orders not found",
                Status = false
            };
        }
        List<OrderProductDto> allOrders = new List<OrderProductDto>();
        foreach (var order in orders)
        {
            var orderProducts = await _orderProductRepository.GetAsync(
                x => x.Order.Id == order.Order.Id
            );

            var orderProduct = new OrderProductDto
            {
                OrderDto = orderProducts
                    .Select(
                        x =>
                            new OrderDto
                            {
                                Product = new ProductDto
                                {
                                    ProductName = x.Product.ProductName,
                                    Price = x.Product.Price,
                                    Image = x.Product.Image,
                                    IsAvailable = x.Product.IsAvailable,
                                    Description = x.Product.Description
                                },
                                Customer = new CustomerDto
                                {
                                    Users = new UserDto
                                    {
                                        UserName =
                                            $"{orderProducts[0].Order.Customer.User.FirstName} {orderProducts[0].Order.Customer.User.LastName}",
                                        PhoneNumber = orderProducts[0]
                                            .Order
                                            .Customer
                                            .User
                                            .Email,
                                        Email = orderProducts[0].Order.Customer.User.Email,
                                        ProfilePicture = orderProducts[0]
                                            .Order
                                            .Customer
                                            .User
                                            .ProfilePicture
                                    }
                                },
                                Quantity = x.Quantity,
                                OrderedDate = x.Order.CreatedAt.ToLongDateString(),
                                IsDelivered = orderProducts[0].Order.IsDelivered,
                                Id = order.Id
                            }
                    )
                    .ToList(),
                AddressDto = new AddressDto
                {
                    AddressId = order.Id,
                    State = orderProducts[0].Order.Address.State,
                    City = orderProducts[0].Order.Address.City,
                    Street = orderProducts[0].Order.Address.Street,
                    PostalCode = orderProducts[0].Order.Address.PostalCode,
                },
            };
            allOrders.Add(orderProduct);
        }
        return new BaseResponse<IEnumerable<OrderProductDto>>
        {
            Message = "Orders found successfully",
            Status = true,
            Data = allOrders
                .Select(
                    x =>
                        new OrderProductDto
                        {
                            AddressDto = x.AddressDto,
                            OrderDto = x.OrderDto,
                            AmountPaid = x.OrderDto.Sum(x => x.Quantity * x.Product.Price),
                        }
                )
                .ToList()
        };

    }

    public async Task<BaseResponse<OrderProductDto>> GetOrderByIdAsync(string id)
    {
        var orders = await _orderProductRepository.GetSelectedAsync(x => x.Order.Id == id);
        if (orders.Count() == 0)
        {
            return new BaseResponse<OrderProductDto>
            {
                Message = "Orders not found",
                Status = false
            };
        }
        return new BaseResponse<OrderProductDto>
        {
            Message = "Successful",
            Status = true,
            Data = new OrderProductDto
            {
                OrderDto = orders.Select(x => new OrderDto
                {
                    Product = new ProductDto
                    {
                        ProductName = x.Product.ProductName,
                        Price = x.Product.Price,
                        Image = x.Product.Image,
                        IsAvailable = x.Product.IsAvailable,
                        Description = x.Product.Description
                    },
                    Customer = new CustomerDto
                    {
                        Users = new UserDto
                        {
                            FirstName = x.Order.Customer.User.FirstName,
                            LastName = x.Order.Customer.User.LastName,
                            Email = x.Order.Customer.User.Email,
                            ProfilePicture = x.Order.Customer.User.ProfilePicture,
                            PhoneNumber = x.Order.Customer.User.PhoneNumber,
                        }
                    },
                    Quantity = x.Quantity,
                    AmountPaid = x.Product.Price * x.Quantity,
                    OrderedDate = x.Order.CreatedAt.ToLongDateString(),
                    IsDelivered = x.Order.IsDelivered,
                    Id = x.Order.Id,
                    Address = new AddressDto
                    {
                        AddressId = x.Order.AddressId,
                        State = x.Order.Address.State,
                        City = x.Order.Address.City,
                        Street = x.Order.Address.Street,
                        PostalCode = x.Order.Address.PostalCode,
                    },

                }).ToList(),



            }
        };

    }

    public async Task<BaseResponse<IList<OrderProductDto>>> GetOrdersByCustomerIdAsync(string customerId)
    {
        var customer = await _customerRepository.GetAsync(customerId);
        if (customer is null)
        {
            return new BaseResponse<IList<OrderProductDto>>
            {
                Message = "failed",
                Status = false
            };
        }
        var orders = await _orderRepository.GetSelectedAsync(x => x.Customer.Id == customer.Id);
        if (orders.Count() == 0)
        {
            return new BaseResponse<IList<OrderProductDto>>
            {
                Message = "failed",
                Status = false
            };
        }
        List<OrderProductDto> allOrders = new List<OrderProductDto>();
        foreach (var order in orders)
        {
            var ord = await _orderProductRepository.GetSelectedAsync(x => x.Order.Id == order.Id);
            var orderProduct = new OrderProductDto
            {
                OrderDto = ord.Select(x => new OrderDto
                {
                    Product = new ProductDto
                    {
                        ProductName = x.Product.ProductName,
                        Price = x.Product.Price,
                        Image = x.Product.Image,
                        IsAvailable = x.Product.IsAvailable,
                        Description = x.Product.Description
                    },
                    Customer = new CustomerDto
                    {
                        Users = new UserDto
                        {
                            FirstName = x.Order.Customer.User.FirstName,
                            LastName = x.Order.Customer.User.LastName,
                            UserName = $"{x.Order.Customer.User.FirstName} {x.Order.Customer.User.LastName}",
                            Email = x.Order.Customer.User.Email,
                            ProfilePicture = x.Order.Customer.User.ProfilePicture,
                            PhoneNumber = x.Order.Customer.User.PhoneNumber,
                        }
                    },
                    Quantity = x.Quantity,
                    OrderedDate = x.Order.CreatedAt.ToLongDateString(),
                    IsDelivered = x.Order.IsDelivered,
                    Id = x.Order.Id,
                    Address = new AddressDto
                    {
                        AddressId = x.Order.AddressId,
                        State = x.Order.Address.State,
                        City = x.Order.Address.City,
                        Street = x.Order.Address.Street,
                        PostalCode = x.Order.Address.PostalCode,
                    },

                }).ToList(),


            };
            allOrders.Add(orderProduct);
        }

        return new BaseResponse<IList<OrderProductDto>>
        {
            Message = "Order Retrived Sucessfully",
            Status = true,
            Data = allOrders.Select(x => new OrderProductDto
            {
                OrderDto = x.OrderDto,
            }).ToList(),

        };

    }

    public async Task<BaseResponse<OrderDto>> UpdateOrderAsync(string id, UpdateOrderRequestModel model)
    {
        var get = await _orderRepository.GetOrderAsync(id);
        if (get is null)
        {
            return new BaseResponse<OrderDto>
            {
                Message = "Not Found",
                Status = false,
            };
        }
        get.IsDelivered = true;
        await _orderRepository.SaveAsync();
        var test = await _saleServices.CreateSales(id);
        if (test is null)
        {
            return new BaseResponse<OrderDto>
            {
                Message = "Sale not Found",
                Status = false,
            };
        }
        return new BaseResponse<OrderDto>
        {
            Message = "successful",
            Status = true,
        };


    }

}
