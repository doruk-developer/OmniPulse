using System.Collections.Generic;
using System.Threading.Tasks;
using OmniPulse.Entities.Common;
using OmniPulse.Entities.Models.Dto;

namespace OmniPulse.Business.Services;

public interface IOmniPulseService
{
    Task<IDataResult<string>> GetNodeIntegrityStatusAsync();
    Task<IResult> SynchronizeInternalStateAsync();
    
}