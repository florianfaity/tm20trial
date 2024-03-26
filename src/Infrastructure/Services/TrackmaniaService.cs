using Microsoft.Extensions.Options;
using tm20trial.Application.Common.Configurations;
using tm20trial.Application.Common.Interfaces;
using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;

namespace tm20trial.Infrastructure.Services;

public class TrackmaniaService : ITrackmaniaService
{

    private readonly TrackmaniaConfiguration _trackmaniaConfiguration; 
    public TrackmaniaService(IOptions<TrackmaniaConfiguration> configuration)
    {
        _trackmaniaConfiguration = configuration.Value;
    }
    
    public async Task<string> GetToken()
    {
        // var result = _trackmaniaConfiguration.BasePublicUbiURL
        //     .AppendPathSegments("v3", "profiles", "sessions")
        //     .WithHeaders()
            
        // var httpClient = new HttpClient();
        // var response = await httpClient.GetAsync(result);
        //
        // if (response.IsSuccessStatusCode)
        // {
        //     
        // }
        // else
        // {
        //     // Handle unsuccessful response
        //     Console.WriteLine($"Error: {response.StatusCode}");
        // }

        return "";
    }
    
    
}
