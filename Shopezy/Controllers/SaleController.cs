using Application.Abstractions.IService;
using Microsoft.AspNetCore.Mvc;

namespace Shopezy.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SaleController : ControllerBase
{
    private readonly ISaleService _saleService;

    public SaleController(ISaleService saleService)
    {
        _saleService = saleService;
    }

    // [HttpPost("CreateSale")]

    [HttpGet("GetByCustomerName")]
    public async Task<IActionResult> GetByNameAsync([FromQuery] string name)
    {
        var sales = await _saleService.GetSalesByCustomerNameAsync(name);
        if (!sales.Status)
        {
            return BadRequest(sales);
        }
        return Ok(sales);
    }

    [HttpGet("GetSalesForTheMonth/{month}/{year}")]
    public async Task<IActionResult> GetSalesForTheMonthOnEachProductAsync(int month, int year)
    {
        var sales = await _saleService.GetSalesForTheMonthOnEachProduct(month, year);
        if (!sales.Status)
        {
            return BadRequest(sales);
        }
        return Ok(sales);
    }

    [HttpGet("GetAllSalesForTheYearOnEachProduct/{year}")]
    public async Task<IActionResult> GetSalesForTheYearOnEachProductAsync(int year)
    {
        var sales = await _saleService.GetSalesForTheYearOnEachProduct(year);
        if (sales.Status)
        {
            return Ok(sales);
        }
        return BadRequest(sales);
    }

    [HttpGet("GetAllSales")]
    public async Task<IActionResult> GetAllSalesAsync()
    {
        var sales = await _saleService.GetAllSales();
        if (!sales.Status)
        {
            return BadRequest(sales);
        }
        return Ok(sales);
    }

    [HttpGet("GetThisYearSales")]
    public async Task<IActionResult> GetThisYearSales()
    {
        var sales = await _saleService.GetSalesForThisYear();
        if (!sales.Status)
        {
            return BadRequest(sales);
        }
        return Ok(sales);
    }

    [HttpGet("GetThisMonthSales")]
    public async Task<IActionResult> GetThisMonthSales()
    {
        var sales = await _saleService.GetSalesForThisMonth();
        if (sales.Status)
        {
            return Ok(sales);
        }
        return NotFound(sales);
    }

    [HttpGet("CalculateProfit")]
    public async Task<IActionResult> CalculateProfitAsync()
    {
        var profit = await _saleService.CalculateThisMonthProfitAsync();
        if (profit.Status)
        {
            return Ok(profit);
        }
        return BadRequest(profit);
    }

    [HttpGet("CalculateProfit/{month}/{year}")]
    public async Task<IActionResult> CalculateProfitAsync(int month, int year)
    {
        var profit = await _saleService.CalculateMonthlyProfitAsync(month, year);
        if (!profit.Status)
        {
            return BadRequest(profit);
        }
        return Ok(profit);
    }

    [HttpGet("GetSalesByProductNameForTheMonth/{productId}/{month}/{year}")]
    public async Task<IActionResult> GetSalesByProductNameForTheMonth(
        string productId,
        int month,
        int year
    )
    {
        var sales = await _saleService.GetSalesByProductNameForTheMonth(productId, month, year);
        if (sales.Status)
        {
            return Ok(sales);
        }
        return NotFound(sales);
    }

    [HttpGet("GetSalesByProductNameForTheYear/{productId}/{year}")]
    public async Task<IActionResult> GetSalesByProductNameForTheYearAsync(
        string productId,
        int year
    )
    {
        var sales = await _saleService.GetSalesByProductNameForTheYear(productId, year);
        if (!sales.Status)
        {
            return BadRequest(sales);
        }
        return Ok(sales);
    }

    [HttpGet("CalculateAllMonthlySales/{year}")]
    public async Task<IActionResult> CalculateAllMonthlySalesAsync(int year)
    {
        var sales = await _saleService.CalculateAllMonthlySalesAsync(year);
        if (!sales.Status)
        {
            return BadRequest(sales);
        }
        return Ok(sales);
    }

    [HttpGet("CalculateNetProfit/{month}/{year}/{extraExpenses}")]
    public async Task<IActionResult> CalculateNetProfitAsync(
        int month,
        int year,
        [FromRoute] decimal extraExpenses
    )
    {
        var test = await _saleService.CalculateNetProfitAsync(
            year,
            month,
            extraExpenses
        );
        if (!test.Status)
        {
            return BadRequest(test);
        }
        return Ok(test);
    }

}
