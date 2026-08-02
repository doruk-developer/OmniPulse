using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using OmniPulse.Business.Services;
namespace OmniPulse.WebUI.Controllers;

public class Vehicle__Device_InventoryController : Controller
{
    private readonly IOmniPulseService _projectService;
    public Vehicle__Device_InventoryController(IOmniPulseService projectService) { _projectService = projectService; }

    public async Task<IActionResult> Index() 
    {
        return await Task.FromResult(View());
    }
}
