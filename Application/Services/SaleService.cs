using Application.Abstractions.IRepository;
using Application.Abstractions.IService;
using Application.Dto;
using Domain.Entities;

namespace Application.Services;

public class SaleService : ISaleService
{
    private readonly ISaleRepository _saleRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRequestRepository _requestRepository;
    private readonly IOrderProductRepository _orderProductRepository;

    public SaleService(ISaleRepository saleRepository, IOrderRepository orderRepository, IProductRepository productRepository, ICustomerRepository customerRepository, IRequestRepository requestRepository, IOrderProductRepository orderProductRepository, IUserRepository userRepository = null)
    {
        _saleRepository = saleRepository;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _requestRepository = requestRepository;
        _orderProductRepository = orderProductRepository;
        _userRepository = userRepository;

    }


    public async Task<BaseResponse<IList<SalesDto>>> CalculateAllMonthlySalesAsync(int year)
    {
        List<decimal> percentagesales = new List<decimal>();
        int month = 1;
        for (int i = 1; i < 13; i++)
        {
            var sale = await _saleRepository.GetTotalMonthlySalesAsync(month, year);
            var request = await _requestRepository.GetSumOfApprovedRequestOfTheMonthAsync(month, year);
            if (request + sale == 0)
            {
                percentagesales.Add(0);
            }
            else
            {
                percentagesales.Add(sale / (sale + request) * 100);
            }
            month++;
        }

        return new BaseResponse<IList<SalesDto>>
        {
            Message = "Succesfull",
            Status = true,
            Data = percentagesales.Select(x => new SalesDto
            {
                AmountPaid = Math.Ceiling(x)
            }).ToList()
        };
    }

    public async Task<BaseResponse<ProfitDto>> CalculateMonthlyProfitAsync(int month, int year)
    {
        var products = await _productRepository.GetAllProductAsync();
        if (products.Count() == 0)
        {
            return new BaseResponse<ProfitDto>
            {
                Message = "No products yet",
                Status = false
            };
        }
        var expenses = await _requestRepository.GetSumOfApprovedRequestOfTheMonthAsync(month, year);
        if (expenses == 0)
        {
            return new BaseResponse<ProfitDto>
            {
                Message = "No expense found for this month",
                Status = false
            };
        }
        var sales = await _saleRepository.GetTotalMonthlySalesAsync(month, year);
        if (sales == 0)
        {
            return new BaseResponse<ProfitDto>
            {
                Message = "No sales found for this month",
                Status = false
            };
        }
        return new BaseResponse<ProfitDto>
        {
            Message = "Calulated Successfully",
            Status = true,
            Data = new ProfitDto
            {
                Profit = sales - expenses,
                Percentage = ((sales - expenses) * 100) / expenses
            }
        };

    }

    public async Task<BaseResponse<ProfitDto>> CalculateNetProfitAsync(int year, int month, decimal extraExpenses)
    {
        var expense = await _requestRepository.GetSumOfApprovedRequestOfTheMonthAsync(month, year);
        var sales = await _saleRepository.GetTotalMonthlySalesAsync(month, year);
        return new BaseResponse<ProfitDto>
        {
            Message = "Net profit calculated successfully",
            Status = true,
            Data = new ProfitDto
            {
                Profit = sales - (expense + extraExpenses)
            }

        };
    }

    public async Task<BaseResponse<ProfitDto>> CalculateThisMonthProfitAsync()
    {
        var products = await _productRepository.GetAllProductAsync();
        if (products.Count() == 0)
        {
            return new BaseResponse<ProfitDto>
            {
                Message = "No products yet",
                Status = false
            };
        }
        var expenses = await _requestRepository.GetSumOfApprovedReqeustForTheMonthAsync();
        if (expenses == 0)
        {
            return new BaseResponse<ProfitDto>
            {
                Message = "No expense found for this month",
                Status = false
            };
        }
        var sales = await _saleRepository.GetTotalMonthlySalesAsync();
        if (sales == 0)
        {
            return new BaseResponse<ProfitDto>
            {
                Message = "No sales found for this month",
                Status = false
            };
        }
        return new BaseResponse<ProfitDto>
        {
            Message = "Calulated Successfully",
            Status = true,
            Data = new ProfitDto
            {
                Profit = sales - expenses,
                Percentage = ((sales - expenses) * 100) / expenses
            }
        };
    }

    public async Task<BaseResponse<ProfitDto>> CalculateThisYearProfitAsync()
    {
        var products = await _productRepository.GetAllProductAsync();
        if (products.Count() == 0)
        {
            return new BaseResponse<ProfitDto>
            {
                Message = "No products yet",
                Status = false
            };
        }
        var request = await _requestRepository.GetSumOfApprovedRequestForTheYearAsync();
        var sales = await _saleRepository.GetTotalYearlySalesAsync();
        return new BaseResponse<ProfitDto>
        {
            Data = new ProfitDto
            {
                Profit = sales - request,
                Percentage = ((sales - request) * 100) / request
            }
        };
    }

    public async Task<BaseResponse<ProfitDto>> CalculateYearlyProfitAsync(int year)
    {
        var products = await _productRepository.GetAllProductAsync();
        if (products.Count() == 0)
        {
            return new BaseResponse<ProfitDto>
            {
                Message = "No products yet",
                Status = false
            };
        }
        var request = await _requestRepository.GetSumOfApprovedRequestForTheYearAsync(year);
        var sales = await _saleRepository.GetTotalYearlySalesAsync();
        return new BaseResponse<ProfitDto>
        {
            Data = new ProfitDto
            {
                Profit = sales - request,
                Percentage = ((sales - request) * 100) / request
            }
        };

    }

    public async Task<BaseResponse<SalesDto>> CreateSales(string id)
    {
        var order = await _orderProductRepository.GetAsync(x => x.Order.Id == id);
        if (order == null)
        {
            return new BaseResponse<SalesDto>
            {
                Message = "Product not found",
                Status = false
            };
        }
        decimal totalAmount = 0;
        foreach (var item in order)
        {
            totalAmount += item.AmountPaid;
        }
        var sales = new Sales
        {
            OrderId = id,
            AmountPaid = totalAmount
        };
        var sale = await _saleRepository.CreateAsync(sales);
        await _saleRepository.SaveAsync();

        return new BaseResponse<SalesDto>
        {
            Message = "Sale created successfully",
            Status = true
        };
    }

    public async Task<BaseResponse<IList<SalesDto>>> GetAllSales()
    {
        var getAll = await _saleRepository.GetAllSaleAsync();
        if (getAll != null)
        {
            List<OrderProductDto> orderProductDtos = new List<OrderProductDto>();
            foreach (var item in getAll)
            {
                var order = await _orderProductRepository.GetAsync(a => a.Order.Id == item.Order.Id);
                var orderProduct = new OrderProductDto
                {

                    AddressDto = new AddressDto
                    {
                        AddressId = order[0].Order.Address.Id,
                        State = order[0].Order.Address.State,
                        City = order[0].Order.Address.City,
                        Street = order[0].Order.Address.Street,
                        PostalCode = order[0].Order.Address.PostalCode,
                    },
                    OrderDto = order.Select(x => new OrderDto
                    {
                        Product = new ProductDto
                        {
                            ProductName = x.Product.ProductName,
                            Price = x.Product.Price,
                            Image = x.Product.Image,
                            Description = x.Product.Description
                        },
                        IsDelivered = x.Order.IsDelivered,
                        CreatedAt = x.Order.CreatedAt.ToLongDateString(),
                        DeliveredDate = x.Order.UpdatedAt.ToLongDateString(),


                    }).ToList(),
                    Quantity = order[0].Quantity,

                };
                orderProductDtos.Add(orderProduct);
            }
            return new BaseResponse<IList<SalesDto>> { Message = "Sales found successfully", Status = true, Data = orderProductDtos.Select(x => new SalesDto { OrderDtos = x.OrderDto, AmountPaid = x.OrderDto.Sum(x => (x.Product.Price * x.Quantity)), AddressId = x.AddressDto.AddressId }).ToList() };
        }
        return new BaseResponse<IList<SalesDto>> { Message = "Failed", Status = false };

    }

    public async Task<BaseResponse<IList<SalesDto>>> GetSalesByCustomerNameAsync(string name)
    {
        var customer = await _customerRepository.GetAsync(x => x.User.UserName == name);
        if (customer == null)
        {
            return new BaseResponse<IList<SalesDto>>
            {
                Message = $"Customer with {name} is not Found",
                Status = false
            };
        }
        var sales = await _saleRepository.GetSaleByCustomerIdAsync(customer.Id);
        if (sales.Count() == 0)
        {
            return new BaseResponse<IList<SalesDto>>
            {
                Message = "no sales found for this customer",
                Status = false
            };
        }
        List<OrderProductDto> orderProductDtos = new List<OrderProductDto>();
        foreach (var item in sales)
        {
            var order = await _orderProductRepository.GetAsync(a => a.Order.Id == item.Order.Id);
            var orderProduct = new OrderProductDto { AddressDto = new AddressDto { AddressId = order[0].Order.Address.Id, State = order[0].Order.Address.State, City = order[0].Order.Address.City, Street = order[0].Order.Address.Street, PostalCode = order[0].Order.Address.PostalCode, }, OrderDto = order.Select(x => new OrderDto { Product = new ProductDto { ProductName = x.Product.ProductName, Price = x.Product.Price, Image = x.Product.Image, Description = x.Product.Description }, IsDelivered = x.Order.IsDelivered, CreatedAt = x.Order.CreatedAt.ToLongDateString(), DeliveredDate = x.Order.UpdatedAt.ToLongDateString(), }).ToList(), Quantity = order[0].Quantity, };
            orderProductDtos.Add(orderProduct);
        }
        return new BaseResponse<IList<SalesDto>>
        {
            Message = "Sales found successfully",
            Status = true,
            Data = orderProductDtos.Select(x => new SalesDto
            {
                OrderDtos = x.OrderDto,
                AmountPaid = x.AmountPaid,
                AddressId = x.AddressDto.AddressId
            }).ToList()
        };
    }

    public async Task<BaseResponse<IList<SalesDto>>> GetSalesByProductNameForTheMonth(string productId, int month, int year)
    {
        var product = await _productRepository.GetAsync(productId);
        if (product == null && product.Id == productId)
        {
            return new BaseResponse<IList<SalesDto>>
            {
                Message = "Product not found",
                Status = false
            };
        }
        var sales = await _orderProductRepository.GetAllDeliveredOrderByProductIdForTheMonthAsync(productId, month, year);
        if (sales.Count() == 0)
        {
            return new BaseResponse<IList<SalesDto>>
            {
                Message = $"No sales found for {product.ProductName}",
                Status = false
            };
        }
        return new BaseResponse<IList<SalesDto>>
        {
            Message = "Sales found successfully",
            Status = true,
            Data = sales.Select(x => new SalesDto
            {
                AmountPaid = x.Quantity * x.Product.Price,
                AddressId = x.Order.AddressId,
                CustomerDto = new CustomerDto
                {
                    Users = new UserDto
                    {
                        UserName = $"{x.Order.Customer.User.FirstName}  {x.Order.Customer.User.LastName}",
                        PhoneNumber = x.Order.Customer.User.PhoneNumber,
                        ProfilePicture = x.Order.Customer.User.ProfilePicture
                    },
                },
            }).ToList()
        };

    }

    public async Task<BaseResponse<IList<SalesDto>>> GetSalesByProductNameForTheYear(string productId, int year)
    {
        var product = await _productRepository.GetAsync(productId);
        if (product == null)
        {
            return new BaseResponse<IList<SalesDto>>
            {
                Message = "Product not found",
                Status = false
            };
        }
        var sales = await _orderProductRepository.GetAllDeleiveredOrderByProductIdForTheYearAsync(productId, year);
        if (sales.Count() == 0)
        {
            return new BaseResponse<IList<SalesDto>>
            {
                Message = $"No sales found for {product.ProductName}",
                Status = false
            };
        }
        return new BaseResponse<IList<SalesDto>>
        {
            Message = "Sales found successfully",
            Status = true,
            Data = sales.Select(x => new SalesDto
            {
                AmountPaid = x.Quantity * x.Product.Price,
                AddressId = x.Order.AddressId,
                CustomerDto = new CustomerDto
                {
                    Users = new UserDto
                    {
                        UserName = $"{x.Order.Customer.User.FirstName}  {x.Order.Customer.User.LastName}",
                        PhoneNumber = x.Order.Customer.User.PhoneNumber,
                        ProfilePicture = x.Order.Customer.User.ProfilePicture
                    },
                },
            }).ToList()
        };
    }

    public async Task<BaseResponse<IList<OrderDto>>> GetSalesForTheMonthOnEachProduct(int month, int year)
    {
        var product = await _productRepository.GetAllProductAsync();
        if (product.Count() == 0)
        {
            return new BaseResponse<IList<OrderDto>>
            {
                Message = "No Products found",
                Status = true
            };
        }
        List<OrderProduct> monthlySales = new List<OrderProduct>();
        foreach (var item in product)
        {
            var salesPerProduct = await _orderProductRepository.GetAllDeliveredOrderByProductIdForTheMonthAsync(item.Id, month, year);
            decimal quantity = 0;
            foreach (var sal in salesPerProduct)
            {
                quantity += sal.Quantity;
            }
            var orderProducct = new OrderProduct
            {
                Quantity = salesPerProduct.Sum(x => x.Quantity),
                Order = new Order
                {
                },
                Product = new Product
                {
                    Id = item.Id,
                    ProductName = item.ProductName,
                    Image = item.Image,
                    Description = item.Description,
                    Price = item.Price
                }
            };
            monthlySales.Add(orderProducct);
        }
        return new BaseResponse<IList<OrderDto>>
        {
            Message = "Sales found Successfully",
            Status = true,
            Data = monthlySales.Select(x => new OrderDto
            {

                AmountPaid = x.Quantity * x.Product.Price,
                Quantity = x.Quantity,
                Product = new ProductDto
                {
                    Id = x.Product.Id,
                    ProductName = x.Product.ProductName,
                    Image = x.Product.Image,
                    Price = x.Product.Price
                }
            }).ToList()
        };

    }

    public async Task<BaseResponse<IList<OrderDto>>> GetSalesForTheYearOnEachProduct(int year)
    {
        var product = await _productRepository.GetAllProductAsync();
        if (product.Count() == 0)
        {
            return new BaseResponse<IList<OrderDto>>
            {
                Message = "No Products found",
                Status = true
            };
        }
        List<OrderProduct> monthlySales = new List<OrderProduct>();
        foreach (var item in product)
        {
            var salesPerProduct = await _orderProductRepository.GetAllDeleiveredOrderByProductIdForTheYearAsync(item.Id, year);
            decimal quantity = 0;
            foreach (var sal in salesPerProduct)
            {
                quantity += sal.Quantity;
            }
            var orderProducct = new OrderProduct
            {
                Quantity = salesPerProduct.Sum(x => x.Quantity),
                Order = new Order
                {
                },
                Product = new Product
                {
                    Id = item.Id,
                    ProductName = item.ProductName,
                    Image = item.Image,
                    Description = item.Description,
                    Price = item.Price
                }
            };
            monthlySales.Add(orderProducct);
        }
        return new BaseResponse<IList<OrderDto>>
        {
            Message = "Sales found Successfully",
            Status = true,
            Data = monthlySales.Select(x => new OrderDto
            {

                AmountPaid = x.Quantity * x.Product.Price,
                Quantity = x.Quantity,
                Product = new ProductDto
                {
                    Id = x.Product.Id,
                    ProductName = x.Product.ProductName,
                    Image = x.Product.Image,
                    Price = x.Product.Price
                }
            }).ToList()
        };
    }

    public async Task<BaseResponse<IList<SalesDto>>> GetSalesForThisMonth()
    {
        var getForMonth = await _saleRepository.GetAllSalesOfTheMonthAsync();
        if (getForMonth != null)
        {
            List<OrderProductDto> orderProductDtos = new List<OrderProductDto>();
            foreach (var item in getForMonth)
            {
                var order = await _orderProductRepository.GetAsync(a => a.Order.Id == item.Order.Id);
                var orderProduct = new OrderProductDto
                {

                    AddressDto = new AddressDto
                    {
                        AddressId = order[0].Order.Address.Id,
                        State = order[0].Order.Address.State,
                        City = order[0].Order.Address.City,
                        Street = order[0].Order.Address.Street,
                        PostalCode = order[0].Order.Address.PostalCode,
                    },
                    OrderDto = order.Select(x => new OrderDto
                    {
                        Product = new ProductDto
                        {
                            ProductName = x.Product.ProductName,
                            Price = x.Product.Price,
                            Image = x.Product.Image,
                            Description = x.Product.Description
                        },
                        IsDelivered = x.Order.IsDelivered,
                        CreatedAt = x.Order.CreatedAt.ToLongDateString(),
                        DeliveredDate = x.Order.UpdatedAt.ToLongDateString(),
                        // Quantity = x.Order.


                    }).ToList(),
                    Quantity = order[0].Quantity,

                };
                orderProductDtos.Add(orderProduct);
            }
            return new BaseResponse<IList<SalesDto>> { Message = "Sales found successfully", Status = true, Data = orderProductDtos.Select(x => new SalesDto { OrderDtos = x.OrderDto, AmountPaid = x.OrderDto.Sum(x => (x.Product.Price * x.Quantity)), AddressId = x.AddressDto.AddressId }).ToList() };
        }
        return new BaseResponse<IList<SalesDto>> { Message = "Failed", Status = false };

    }

    public async Task<BaseResponse<IList<SalesDto>>> GetSalesForThisYear()
    {
        var getForTheYear = await _saleRepository.GetAllSalesOfTheYearAsync();
        if (getForTheYear != null)
        {
            List<OrderProductDto> orderProductDtos = new List<OrderProductDto>();
            foreach (var item in getForTheYear)
            {
                var order = await _orderProductRepository.GetAsync(a => a.Order.Id == item.Order.Id);
                var orderProduct = new OrderProductDto
                {

                    AddressDto = new AddressDto
                    {
                        AddressId = order[0].Order.Address.Id,
                        State = order[0].Order.Address.State,
                        City = order[0].Order.Address.City,
                        Street = order[0].Order.Address.Street,
                        PostalCode = order[0].Order.Address.PostalCode,
                    },
                    OrderDto = order.Select(x => new OrderDto
                    {
                        Product = new ProductDto
                        {
                            ProductName = x.Product.ProductName,
                            Price = x.Product.Price,
                            Image = x.Product.Image,
                            Description = x.Product.Description
                        },
                        IsDelivered = x.Order.IsDelivered,
                        CreatedAt = x.Order.CreatedAt.ToLongDateString(),
                        DeliveredDate = x.Order.UpdatedAt.ToLongDateString(),
                        // Quantity = x.Order.


                    }).ToList(),
                    Quantity = order[0].Quantity,

                };
                orderProductDtos.Add(orderProduct);
            }
            return new BaseResponse<IList<SalesDto>> { Message = "Sales found successfully", Status = true, Data = orderProductDtos.Select(x => new SalesDto { OrderDtos = x.OrderDto, AmountPaid = x.OrderDto.Sum(x => (x.Product.Price * x.Quantity)), AddressId = x.AddressDto.AddressId }).ToList() };
        }
        return new BaseResponse<IList<SalesDto>> { Message = "Failed", Status = false };
    }

}


