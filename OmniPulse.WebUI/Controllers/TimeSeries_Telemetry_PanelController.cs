using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using OmniPulse.Business.Services;
namespace OmniPulse.WebUI.Controllers;

public class TimeSeries_Telemetry_PanelController : Controller
{
    private readonly IOmniPulseService _projectService;
    public TimeSeries_Telemetry_PanelController(IOmniPulseService projectService) { _projectService = projectService; }

    public async Task<IActionResult> Index() 
    {
        return await Task.FromResult(View());
    }
}
