using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using Microsoft.Extensions.Options;
using tm20trial.Application.Common.Configurations;
using tm20trial.Application.Common.Interfaces;
using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using tm20trial.Application.Common.Models;

namespace tm20trial.Infrastructure.Services;

public class TrackmaniaService : ITrackmaniaService
{
    private const string UbiAppId = "86263886-327a-4328-ac69-527f0d20a237";
    private const string UserAgent = "Trial leaderboard App / ";
    
    private readonly TrackmaniaConfiguration _trackmaniaConfiguration; 
    public TrackmaniaService(IOptions<TrackmaniaConfiguration> configuration)
    {
        _trackmaniaConfiguration = configuration.Value;
    }
    
    public async Task<string> GetToken()
    {
        string encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(_trackmaniaConfiguration.LoginUbisoft + ":" + _trackmaniaConfiguration.PasswordUbisoft));
        // var client = new HttpClient();
        // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encoded);

        var profilesSessionsResponse = await _trackmaniaConfiguration.BasePublicUbiURL
            .AppendPathSegments("v3", "profiles", "sessions")
            .WithHeader("Content-Type", "application/json")
            .WithHeader("Ubi-AppId", UbiAppId)
            .WithHeader("User-Agent", $"{UserAgent} {_trackmaniaConfiguration.LoginUbisoft}")
            .WithHeader("Authorization", $"Basic {encoded}")
            .PostJsonAsync(new {})
            .ReceiveJson<ProfilesSessionsResponse>();
            ;
            
        if(profilesSessionsResponse == null || profilesSessionsResponse.ticket == null)
        {
            throw new AuthenticationException();
        }
        
        var result = await _trackmaniaConfiguration.BaseAPIURL
                .AppendPathSegments("v2", "authentication", "token", "ubiservices")
                .WithHeader("Content-Type", "application/json")
                .WithHeader("Authorization", $"ubi_v1 t={profilesSessionsResponse.ticket}")
                .WithHeader("User-Agent", $"{UserAgent} {_trackmaniaConfiguration.LoginUbisoft}")
                .PostJsonAsync(new {})
                .ReceiveJson<UbiServicesTokenResponse>()
            ;
               
        if(result == null || result.accessToken == null)
        {
            throw new AuthenticationException();
        }

        return result.accessToken;
    }
    
    public async Task<NadeoMapResponse> GetMapById(string mapId)
    {
        var isId =  mapId.Contains("-");
        var mapIdList = new { mapIdList = mapId };
        var mapUidList = new { mapUidList = mapId };

        var token =
            "eyJhbGciOiJIUzI1NiIsImVudiI6InRyYWNrbWFuaWEtcHJvZCIsInZlciI6IjEifQ.eyJqdGkiOiJiMDIzOGRjMi1lYzc3LTExZWUtYmQ1NC0wYTU4YTlmZWFjMDIiLCJpc3MiOiJOYWRlb1NlcnZpY2VzIiwiaWF0IjoxNzExNTcwOTI5LCJyYXQiOjE3MTE1NzI3MjksImV4cCI6MTcxMTU3NDUyOSwiYXVkIjoiTmFkZW9TZXJ2aWNlcyIsInVzZyI6IkNsaWVudCIsInNpZCI6ImIwMjM4OWQwLWVjNzctMTFlZS05MmE1LTBhNThhOWZlYWMwMiIsInNhdCI6MTcxMTU3MDkyOSwic3ViIjoiOWRhYzQ4MTctNzM4ZC00ZWIxLWFkYjItM2FkMThlZjYxZGRmIiwiYXVuIjoiTGVmb3VvcmUiLCJydGsiOmZhbHNlLCJwY2UiOmZhbHNlLCJ1YmlzZXJ2aWNlc191aWQiOiJjZWMyYjRkMC1jYzVmLTRkN2UtYTE0YS02NTUxNTQzY2M0MzgifQ.Hi7V1pY3lMROlc3JVtdlQB8v5UKcsgXr9xfrF7r70Wo";
        // await GetToken();

        if (isId)
        {

            var resultId = await _trackmaniaConfiguration.BaseAPIURL
                .AppendPathSegments("maps")
                .SetQueryParams(mapIdList)
                .WithHeader("Authorization", $"nadeo_v1 t={token}")
                .WithHeader("Content-Type", "application/json")
                .WithHeader("User-Agent", $"{UserAgent} {_trackmaniaConfiguration.LoginUbisoft}")
                .GetJsonAsync<List<NadeoMapResponse>>();
             
           if(resultId == null || resultId.Count == 0)
           {
               throw new NotFoundException("Map not found", "GetMapById");
           }

           return resultId[0];
           
        }
        else{
           var resultUid = await _trackmaniaConfiguration.BaseAPIURL
                    .AppendPathSegments("maps")
                    .SetQueryParams(mapUidList) 
                    .WithHeader("Authorization", $"nadeo_v1 t={token}")
                    .WithHeader("Content-Type", "application/json")
                    .WithHeader("User-Agent", $"{UserAgent} {_trackmaniaConfiguration.LoginUbisoft}")
                     .GetJsonAsync<List<NadeoMapResponse>>()
                ;
           if(resultUid == null || resultUid.Count == 0)
           {
               throw new NotFoundException("Map not found", "GetMapByUiId");
           }

           return resultUid[0];
        }
        // if(result == null || result.Count == 0)
        // {
        //     throw new NotFoundException("Map not found", "GetMapById");
        // }
        //
        // return result[0];
    }
    
}
