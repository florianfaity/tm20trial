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
    
    public async Task<ContentResult> GetAutorize()
    {

        var result = _trackmaniaConfiguration.BaseLoginURL
            .AppendPathSegments("oauth", "authorize")
            .SetQueryParams(new
            {
                reponse_type = "code",
                client_id = _trackmaniaConfiguration.ClientId,
                redirect_uri = "https://localhost:44447/",
            });
            
        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync(result);

        if (response.IsSuccessStatusCode)
        {
            // Read the HTML content as string
            return new ContentResult
            {
                ContentType = "text/html", 
                Content = await response.Content.ReadAsStringAsync(),
                StatusCode = 200
            };
            //  var result = await response.Content.ReadAsStringAsync();
            //  result.
        }
        else
        {
            // Handle unsuccessful response
            Console.WriteLine($"Error: {response.StatusCode}");
        }

        return new ContentResult();
    }
    
    
}
