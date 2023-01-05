using CleanArchitecture.Application;
using CleanArchitecture.CrossCutting;
using CleanArchitecture.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace CleanArchitecture.Api.Tests;

public class CustomerControllerTest
{
    public CustomerControllerTest(ITestOutputHelper testOutputHelper)
    {
        testOutputHelper.WriteLine(nameof(CustomerControllerTest));
    }

    private static CustomerController Controller
    {
        get
        {
            var services = new ServiceCollection();

            services.AddContextInMemoryDatabase();
            services.AddClassesMatchingInterfaces();
            services.AddMediator();
            services.AddScoped<CustomerController, CustomerController>();

            return services.BuildServiceProvider().GetRequiredService<CustomerController>();
        }
    }

    [Fact]
    public async Task AddShouldReturnBadRequest()
    {
        var request = new AddCustomerRequest(default);

        var result = await Controller.AddAsync(request) as BadRequestObjectResult;

        Assert.NotNull(result);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task AddShouldReturnOk()
    {
        var request = new AddCustomerRequest(Guid.NewGuid().ToString());

        var result = (await Controller.AddAsync(request) as OkObjectResult)!.Value as AddCustomerResponse;

        Assert.NotNull(result);

        Assert.IsType<AddCustomerResponse>(result);

        Assert.Equal(1, Convert.ToInt64(result.Id));
    }

    [Fact]
    public async Task DeleteNonExistentShouldReturnOk()
    {
        var result = await Controller.DeleteAsync(1) as OkResult;

        Assert.NotNull(result);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteShouldReturnBadRequest()
    {
        var result = await Controller.DeleteAsync(default) as BadRequestObjectResult;

        Assert.NotNull(result);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task DeleteShouldReturnOk()
    {
        var controller = Controller;

        var request = new AddCustomerRequest(Guid.NewGuid().ToString());

        await controller.AddAsync(request);

        var result = await controller.DeleteAsync(1) as OkResult;

        Assert.NotNull(result);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task GetShouldReturnBadRequest()
    {
        var result = await Controller.GetAsync(default) as BadRequestObjectResult;

        Assert.NotNull(result);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetShouldReturnNoContent()
    {
        var result = await Controller.GetAsync(1) as NoContentResult;

        Assert.NotNull(result);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task GetShouldReturnOk()
    {
        var controller = Controller;

        var request = new AddCustomerRequest(Guid.NewGuid().ToString());

        await controller.AddAsync(request);

        var result = await controller.GetAsync(1) as OkObjectResult;

        Assert.NotNull(result);

        Assert.IsType<OkObjectResult>(result);

        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task ListShouldReturnNoContent()
    {
        var result = await Controller.ListAsync() as NoContentResult;

        Assert.NotNull(result);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task ListShouldReturnOk()
    {
        var controller = Controller;

        var request = new AddCustomerRequest(Guid.NewGuid().ToString());

        await controller.AddAsync(request);

        var result = (await controller.ListAsync() as OkObjectResult)!.Value as ListCustomerResponse;

        Assert.NotNull(result);

        Assert.IsType<ListCustomerResponse>(result);

        Assert.True(result.Customers.Any());
    }

    [Fact]
    public async Task UpdateNonExistentShouldReturnOk()
    {
        var updateRequest = new UpdateCustomerRequest(1, Guid.NewGuid().ToString());

        var result = await Controller.UpdateAsync(updateRequest) as OkResult;

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task UpdateShouldReturnOk()
    {
        var controller = Controller;

        var addRequest = new AddCustomerRequest(Guid.NewGuid().ToString());

        await controller.AddAsync(addRequest);

        var addResponse = (await controller.GetAsync(1) as OkObjectResult)!.Value as GetCustomerResponse;

        var updateRequest = new UpdateCustomerRequest(1, Guid.NewGuid().ToString());

        await controller.UpdateAsync(updateRequest);

        var updateResponse = (await controller.GetAsync(1) as OkObjectResult)!.Value as GetCustomerResponse;

        Assert.NotNull(addResponse);

        Assert.NotNull(updateResponse);

        Assert.NotEqual(addResponse.Customer.Name, updateResponse.Customer.Name);
    }
}
