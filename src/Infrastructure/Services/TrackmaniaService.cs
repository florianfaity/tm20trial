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
    private const string KeyCachingUbisoftToken = "ubisoft-token";
    private const string KeyCachingUbisoftBearerToken = "ubisoft-bearer-token";
    
    private readonly TrackmaniaConfiguration _trackmaniaConfiguration; 
    private readonly MemoryCacheEntryOptions _memoryCacheOptionsToken;
    private readonly MemoryCacheEntryOptions _memoryCacheOptionsBearerToken;
    private readonly IMemoryCache _memoryCache;
    public TrackmaniaService(IOptions<TrackmaniaConfiguration> configuration, IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        _memoryCacheOptionsToken = new MemoryCacheEntryOptions();
        _memoryCacheOptionsBearerToken = new MemoryCacheEntryOptions();
        _memoryCacheOptionsToken.SetSlidingExpiration(TimeSpan.FromMinutes(120)); 
        _memoryCacheOptionsBearerToken.SetSlidingExpiration(TimeSpan.FromMinutes(9)); 
        
        _trackmaniaConfiguration = configuration.Value;
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

    public async Task<string> GetToken()
    {
        string? accessToken = "";
        var cacheExist = _memoryCache.TryGetValue(KeyCachingUbisoftToken, out accessToken);

        if (cacheExist && accessToken != null)
        {
            return accessToken;
        }

        string encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(_trackmaniaConfiguration.LoginUbisoft + ":" +
                                                                       _trackmaniaConfiguration.PasswordUbisoft));


        var profilesSessionsResponse = await _trackmaniaConfiguration.BasePublicUbiURL
            .AppendPathSegments("v3", "profiles", "sessions")
            .WithHeader("Content-Type", "application/json")
            .WithHeader("Ubi-AppId", UbiAppId)
            .WithHeader("User-Agent", $"{UserAgent} {_trackmaniaConfiguration.LoginUbisoft}")
            .WithHeader("Authorization", $"Basic {encoded}")
            .PostJsonAsync(new { })
            .ReceiveJson<ProfilesSessionsResponse>();
        ;

        if (profilesSessionsResponse?.ticket == null)
        {
            throw new AuthenticationException();
        }

        var result = await _trackmaniaConfiguration.BaseAPIURL
                .AppendPathSegments("v2", "authentication", "token", "ubiservices")
                .WithHeader("Content-Type", "application/json")
                .WithHeader("Authorization", $"ubi_v1 t={profilesSessionsResponse.ticket}")
                .WithHeader("User-Agent", $"{UserAgent} {_trackmaniaConfiguration.LoginUbisoft}")
                .PostJsonAsync(new { })
                .ReceiveJson<UbiServicesTokenResponse>()
            ;

        if (result == null || result.accessToken == null)
        {
            throw new AuthenticationException();
        }

        _memoryCache.Set(KeyCachingUbisoftToken, result.accessToken, _memoryCacheOptionsToken);

        return result.accessToken;
    }


    public async Task<UbiServicesBearerTokenResponse> GetBearerToken()
    {
        UbiServicesBearerTokenResponse? accessToken = new UbiServicesBearerTokenResponse();
        var cacheExist = _memoryCache.TryGetValue(KeyCachingUbisoftBearerToken, out accessToken);

        if (cacheExist && accessToken != null)
        {
            return accessToken;
        }
      
        var bearerToken = await _trackmaniaConfiguration.BaseLoginURL
            .AppendPathSegments("api","access_token")
            .PostJsonAsync(new
            {
                grant_type="client_credentials",
                client_id=_trackmaniaConfiguration.ClientId ,
                client_secret=_trackmaniaConfiguration.ClientSecret
            })
            .ReceiveJson<UbiServicesBearerTokenResponse>()
            ;
        _memoryCache.Set(KeyCachingUbisoftBearerToken, bearerToken, _memoryCacheOptionsBearerToken);
    
        return bearerToken;
    }
}
