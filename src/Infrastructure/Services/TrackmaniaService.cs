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
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;

namespace tm20trial.Infrastructure.Services;

public class TrackmaniaService : ITrackmaniaService
{
    private const string UbiAppId = "86263886-327a-4328-ac69-527f0d20a237";
    private const string UserAgent = "Trial leaderboard App /";
    
    private readonly TrackmaniaConfiguration _trackmaniaConfiguration; 
    private readonly MemoryCacheEntryOptions _memoryCacheOptions;
    private readonly IMemoryCache _memoryCache;
    public TrackmaniaService(IOptions<TrackmaniaConfiguration> configuration, IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        _memoryCacheOptions = new MemoryCacheEntryOptions();
        _memoryCacheOptions.SetSlidingExpiration(TimeSpan.FromMinutes(60)); // Le cache expireras au bout de 60 minutes d'inutilisation

        _trackmaniaConfiguration = configuration.Value;
    }
    
    public async Task<string> GetToken()
    {
        return await GetFromCacheOrCreate($"ubisoft-token", async () =>
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
            }, 1
        ).Result;
    }
    
    public async Task<NadeoMapResponse> GetMapById(string mapId)
    {
        var token = await GetToken();
          
        if (mapId.Contains("-")){
            var resultId = await _trackmaniaConfiguration.BaseAPIURL
                .AppendPathSegments("maps", mapId)
                .WithHeader("Authorization", $"nadeo_v1 t={token}")
                .WithHeader("Content-Type", "application/json")
                .WithHeader("User-Agent", $"{UserAgent} {_trackmaniaConfiguration.LoginUbisoft}")
                .GetJsonAsync<NadeoMapResponse>();
             
           if(resultId == null)
           {
               throw new NotFoundException("Map not found", "GetMapById");
           }

           if (!String.IsNullOrEmpty(resultId.Author))
           {
               resultId.AuthorDisplayName = await GetDisplayNameById(resultId.Author);
           }
           
           
           
           
           return resultId;
        }
       
        throw new NotFoundException("Id wrong format", "GetMapById");
    }

    public async Task<string> GetDisplayNameById(string id)
    {
        var bearerToken = await GetBearerToken();
        var displayNameJson = await _trackmaniaConfiguration.BaseLoginURL
            .AppendPathSegments("api","display-names")
            .SetQueryParam("accountId[]", id)
            .WithHeader("Authorization", $"Bearer {bearerToken.access_token}")
            .WithHeader("Content-Type", "application/json")
            .WithHeader("User-Agent", $"{UserAgent} {_trackmaniaConfiguration.LoginUbisoft}")
            .GetStringAsync();
        
        JObject data = JObject.Parse(displayNameJson);
        var displayName = data.Properties().First();
        return displayName.Value.ToString();
    }


    public async Task<UbiServicesBearerTokenResponse> GetBearerToken()
    {
        return await GetFromCacheOrCreate($"ubisoft-bearer-token", async () =>
            {
                return await _trackmaniaConfiguration.BaseLoginURL
                    .AppendPathSegments("api","access_token")
                    .PostJsonAsync(new
                    {
                        grant_type="client_credentials",
                        client_id=_trackmaniaConfiguration.ClientId ,
                        client_secret=_trackmaniaConfiguration.ClientSecret
                    })
                    .ReceiveJson<UbiServicesBearerTokenResponse>()
                    ;
            }, 9
        ).Result;

    }
    public async Task<T> GetFromCacheOrCreate<T>(string id, Func<T> NoCacheAction, int cacheDurationInMinutes = 15)
    {
        return await _memoryCache.GetOrCreateAsync(id, cacheEntry =>
        {
            var cacheExpiryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(cacheDurationInMinutes),
                Priority = CacheItemPriority.High
            };

            return Task.FromResult(NoCacheAction());
        })?? throw new NotFoundException("ProblemeCache ", "GetFromCacheOrCreate");
    }
}
