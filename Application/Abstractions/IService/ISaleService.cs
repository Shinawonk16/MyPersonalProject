using Application.Dto;

namespace Application.Abstractions.IService;

public interface ISaleService
{

    Task<BaseResponse<IList<SalesDto>>> CalculateAllMonthlySalesAsync(int year);
    Task<BaseResponse<ProfitDto>> CalculateMonthlyProfitAsync(int month, int year);
    Task<BaseResponse<ProfitDto>> CalculateNetProfitAsync(int year, int month, decimal extraExpenses);
    Task<BaseResponse<ProfitDto>> CalculateThisMonthProfitAsync();
    Task<BaseResponse<ProfitDto>> CalculateThisYearProfitAsync();
    Task<BaseResponse<ProfitDto>> CalculateYearlyProfitAsync(int year);
    Task<BaseResponse<SalesDto>> CreateSales(string id);
    Task<BaseResponse<IList<SalesDto>>> GetAllSales();
    Task<BaseResponse<IList<SalesDto>>> GetSalesByCustomerNameAsync(string name);
    Task<BaseResponse<IList<SalesDto>>> GetSalesByProductNameForTheMonth(string productId, int month, int year);
    Task<BaseResponse<IList<SalesDto>>> GetSalesByProductNameForTheYear(string productId, int year);
    Task<BaseResponse<IList<OrderDto>>> GetSalesForTheMonthOnEachProduct(int month, int year);
    Task<BaseResponse<IList<OrderDto>>> GetSalesForTheYearOnEachProduct(int year);
    Task<BaseResponse<IList<SalesDto>>> GetSalesForThisMonth();
    Task<BaseResponse<IList<SalesDto>>> GetSalesForThisYear();
}
