using System.Runtime.InteropServices;
using tm20trial.Domain.Constants;
using tm20trial.Domain.Entities;
using tm20trial.Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using tm20trial.Domain.Enums;

namespace tm20trial.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var administratorRole = new IdentityRole(Roles.Administrator);
        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }
        
        var playerRole = new IdentityRole(Roles.Player);
        if (_roleManager.Roles.All(r => r.Name != playerRole.Name))
        {
            await _roleManager.CreateAsync(playerRole);
        }
        
        var mapperRole = new IdentityRole(Roles.Mapper);
        if (_roleManager.Roles.All(r => r.Name != mapperRole.Name))
        {
            await _roleManager.CreateAsync(mapperRole);
        }

        // Default users
        var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Administrator1!");
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new [] { administratorRole.Name });
            }
        }
        
        if (!_context.Maps.Any())
        {
            var maps = new List<Maps>
            {
                new Maps
                {
                    Name = "Current",
                    Author = "Giilo.",
                    Difficulty = EDifficulty.Easy,
                    TypeTrial = ETypeTrial.Classic,
                    Points = 5,
                    TmIoId = "aae56090-e395-40a2-bc1f-9e615cf2c166",
                    TmxLink = "https://trackmania.exchange/maps/127530/current",
                    ImageLink = "https://core.trackmania.nadeo.live/storageObjects/adbc3ced-ac4e-4f9f-8082-f99977ec76d4.jpg",
                    NumberCheckpoint = 0,
                    State = EStateValidation.Validate,
                    FileUrl = "https://core.trackmania.nadeo.live/storageObjects/15e58b55-b25f-4860-9b0c-a03d06e7ef6d"
                },
                new Maps
                {
                    Name = "Storei",
                    Author = "Adripeal.",
                    Difficulty = EDifficulty.Easy,
                    TypeTrial = ETypeTrial.Classic,
                    Points = 5,
                    TmIoId = "a0bf44e1-94f7-419a-ace0-9a939e4e4a63",
                    TmxLink = "https://trackmania.exchange/maps/108447/trial-storei",
                    ImageLink = "https://core.trackmania.nadeo.live/storageObjects/4b8dee87-14d9-43f9-b94c-735ffa1964e7.jpg",
                    NumberCheckpoint = 1,
                    State = EStateValidation.Validate,
                    FileUrl = "https://core.trackmania.nadeo.live/storageObjects/ca5e75d1-f075-4143-a3ca-e3e97ae1d2b9"
                },
                
                new Maps
                {
                    Name = "SMC - Toccata",
                    Author = "Vst2m.",
                    Difficulty = EDifficulty.Easy,
                    TypeTrial = ETypeTrial.Classic,
                    Points = 5,
                    TmIoId = "94890b64-c71e-4b94-a109-cd6d4bf71ed2",
                    TmxLink = "https://trackmania.exchange/maps/107597/smc-toccata",
                    ImageLink = "https://core.trackmania.nadeo.live/storageObjects/f3649e8e-acf8-4984-ae30-7c33977cfed0.jpg",
                    NumberCheckpoint = 1,
                    State = EStateValidation.Validate,
                    FileUrl = "https://core.trackmania.nadeo.live/storageObjects/a2885e9d-01fd-456e-9392-a8d0e6d559de"
                },
                new Maps
                {
                    Name = "Obstruction",
                    Author = "Naxanria",
                    Difficulty = EDifficulty.Easy,
                    TypeTrial = ETypeTrial.Classic,
                    Points = 5,
                    TmIoId = "00f1569a-c5ab-4c90-914e-741fa621cb1a",
                    TmxLink = "https://trackmania.exchange/maps/128070/obstruction",
                    ImageLink = "https://core.trackmania.nadeo.live/storageObjects/59356e8c-9fe2-45ba-abcb-c2431f0b50bd.jpg",
                    NumberCheckpoint = 16,
                    State = EStateValidation.Validate,
                    FileUrl = "https://core.trackmania.nadeo.live/storageObjects/ce37064c-f90b-40b2-9201-2417c6bf6ec3"
                },
                new Maps
                {
                    Name = "Gordian Knot",
                    Author = "Xefas.",
                    Difficulty = EDifficulty.Intermediate,
                    TypeTrial = ETypeTrial.Classic,
                    Points = 10,
                    TmIoId = "6913628e-ec44-4d0c-804b-4becd3728681",
                    TmxLink = "https://trackmania.exchange/tracks/view/109781",
                    VideoLink = "https://www.youtube.com/watch?v=Fjw94AJj_FY",
                    ImageLink = "https://core.trackmania.nadeo.live/storageObjects/335c371d-4673-4c89-9e7c-3f52ed18462a.jpg",
                    NumberCheckpoint = 1,
                    State = EStateValidation.Validate,
                    FileUrl = "https://core.trackmania.nadeo.live/storageObjects/23591e60-eea3-4ac3-8bfe-e40bebbebac2"
                },
                new Maps
                {
                    Name = "Immortalized",
                    Author = "Whiskey..",
                    Difficulty = EDifficulty.Intermediate,
                    TypeTrial = ETypeTrial.Classic,
                    Points = 10,
                    TmIoId = "7f0b52eb-e333-473a-9cc5-703bc05d50e0",
                    TmxLink = "https://trackmania.exchange/tracks/view/107687",
                    VideoLink = "https://www.youtube.com/watch?v=3-cjdrccii4",
                    ImageLink = "https://core.trackmania.nadeo.live/storageObjects/604214c7-2408-4575-a8a3-30f2aee225d6.jpg",
                    NumberCheckpoint = 1,
                    State = EStateValidation.Validate,
                    FileUrl = "https://core.trackmania.nadeo.live/storageObjects/e0c9cc3f-7537-40bd-b04e-e53fbd420225"
                },
                new Maps
                {
                    Name = "Unfortunate Development",
                    Author = "Kyle.A26",
                    Difficulty = EDifficulty.Intermediate,
                    TypeTrial = ETypeTrial.Classic,
                    Points = 15,
                    TmIoId = "bd8b1962-63d2-4183-ae0a-955463b67509",
                    TmxLink = "https://trackmania.exchange/tracks/view/108288",
                    VideoLink = "https://www.youtube.com/watch?v=jJC0FPVEPv8",
                    ImageLink = "https://core.trackmania.nadeo.live/storageObjects/c9613bab-8a61-406b-91ae-426f53dddf30.jpg",
                    NumberCheckpoint = 2,
                    State = EStateValidation.Validate,
                    FileUrl = "https://core.trackmania.nadeo.live/storageObjects/abbf13c4-7ce7-4a59-aa8f-67e7042762f3"
                },
                new Maps
                {
                    Name = "Send Me Courage",
                    Author = "DoogiieMD",
                    Difficulty = EDifficulty.Intermediate,
                    TypeTrial = ETypeTrial.Classic,
                    Points = 10,
                    TmIoId = "24ed43bb-d754-42ed-b51c-f154463df2cf",
                    TmxLink = "https://trackmania.exchange/tracks/view/109005",
                    VideoLink = "",
                    ImageLink = "https://core.trackmania.nadeo.live/storageObjects/617c97a4-4a63-4aad-b767-4a76803c190c.jpg",
                    NumberCheckpoint = 1,
                    State = EStateValidation.Validate,
                    FileUrl = "https://core.trackmania.nadeo.live/storageObjects/32fbcdad-cce7-45c2-bde1-bd31f85d14cb"
                },
                new Maps
                {
                    Name = "Divine Azure",
                    Author = "RacePhilip",
                    Difficulty = EDifficulty.Intermediate,
                    TypeTrial = ETypeTrial.Classic,
                    Points = 15,
                    TmIoId = "ee3502fa-d45c-4363-90b0-d237e988dfce",
                    TmxLink = "https://trackmania.exchange/tracks/view/132146",
                    VideoLink = "https://www.youtube.com/watch?v=hvr6B1pjYqg",
                    ImageLink = "https://core.trackmania.nadeo.live/storageObjects/5914d98f-56cd-4708-acea-a74ff9c0e9a6.jpg",
                    NumberCheckpoint = 19,
                    State = EStateValidation.Validate,
                    FileUrl = "https://core.trackmania.nadeo.live/storageObjects/d7278b44-ddb9-44d0-a342-a9dc92f964f5"
                },
                new Maps
                {
                    Name = "Umbrag \u2662",
                    Author = "vcou.",
                    Difficulty = EDifficulty.Intermediate,
                    TypeTrial = ETypeTrial.Classic,
                    Points = 15,
                    TmIoId = "ac41e3a8-ec63-4674-a7da-f816c189f364",
                    TmxLink = "https://trackmania.exchange/tracks/view/117260",
                    VideoLink = "https://www.youtube.com/watch?v=o7r0ECM7xlA",
                    ImageLink = "https://core.trackmania.nadeo.live/storageObjects/3896989b-b5da-4b22-831d-0d3aca0c5611.jpg",
                    NumberCheckpoint = 2,
                    State = EStateValidation.Validate,
                    FileUrl = "https://core.trackmania.nadeo.live/storageObjects/dd2aaf14-002e-4cb6-ad20-f1363041e3c1"
                },
                new Maps
                {
                    Name = "AMBYSTOMA",
                    Author = "Jorday.",
                    Difficulty = EDifficulty.Advanced,
                    TypeTrial = ETypeTrial.Classic,
                    Points = 15,
                    TmIoId = "ff45f249-ff23-4c55-b280-17712c40ded8",
                    TmxLink = "https://trackmania.exchange/tracks/view/108792",
                    VideoLink = "https://www.youtube.com/watch?v=wkxFTBLQ7WY",
                    ImageLink = "https://core.trackmania.nadeo.live/storageObjects/d0b41813-9b86-45ad-b1f4-e96e3e9dfee1.jpg",
                    NumberCheckpoint = 2,
                    State = EStateValidation.Validate,
                    FileUrl = "https://core.trackmania.nadeo.live/storageObjects/18e43bc3-f81a-4d0e-ada4-f859c2200194"
                },
                new Maps
                {
                    Name = "Final Noslide",
                    Author = "C4m_45",
                    Difficulty = EDifficulty.Advanced,
                    TypeTrial = ETypeTrial.Classic,
                    Points = 15,
                    TmIoId = "7f1ca0ba-21ab-4e6f-9fed-d1f0ddfbc5d3",
                    TmxLink = "https://trackmania.exchange/tracks/view/66646",
                    VideoLink = "",
                    ImageLink = "https://core.trackmania.nadeo.live/storageObjects/16ac24f2-847e-4b63-a34c-79c0e4cfa6ab.jpg",
                    NumberCheckpoint = 10,
                    State = EStateValidation.Validate,
                    FileUrl = "https://core.trackmania.nadeo.live/storageObjects/6a34e3b4-32d2-47e1-9474-c2e648e93bee"
                },
                new Maps
                {
                    Name = "Community Summer '23 - Trial",
                    Author = "Roquete",
                    Difficulty = EDifficulty.Advanced,
                    TypeTrial = ETypeTrial.Classic,
                    Points = 25,
                    TmIoId = "f15c6e29-3512-4c37-b0b7-460aa1c70850",
                    TmxLink = "https://trackmania.exchange/tracks/view/122855",
                    VideoLink = "",
                    ImageLink = "https://core.trackmania.nadeo.live/storageObjects/4b6c12c4-8ce2-4f8f-afb6-713396680685.jpg",
                    NumberCheckpoint = 7,
                    State = EStateValidation.Validate,
                    FileUrl = "https://core.trackmania.nadeo.live/storageObjects/0d544ad2-8806-49a8-8778-fca1db86f059"
                },
                new Maps
                {
                    Name = "Alive",
                    Author = "Dlgger",
                    Difficulty = EDifficulty.Advanced,
                    TypeTrial = ETypeTrial.Classic,
                    Points = 25,
                    TmIoId = "9c3df527-cce7-420b-b96c-e7a0c36a8054",
                    TmxLink = "https://trackmania.exchange/tracks/view/29721",
                    VideoLink = "https://www.youtube.com/watch?v=tG6XWsjTIqs",
                    ImageLink = "https://core.trackmania.nadeo.live/storageObjects/14739e36-a7d7-4230-bdc2-0974bd19379b.jpg",
                    NumberCheckpoint = 6,
                    State = EStateValidation.Validate,
                    FileUrl = "https://core.trackmania.nadeo.live/storageObjects/bf234dde-fbe3-4cdb-bf3d-6dd500d92741"
                },
                new Maps
                {
                    Name = "Nyctophobia",
                    Author = "Kyle.A26",
                    Difficulty = EDifficulty.Advanced,
                    TypeTrial = ETypeTrial.Classic,
                    Points = 25,
                    TmIoId = "863b27c0-3daf-4516-ba23-6e553fbc89a3",
                    TmxLink = "https://trackmania.exchange/tracks/view/34817",
                    VideoLink = "https://www.youtube.com/watch?v=hjy9fN4oUk4",
                    ImageLink = "https://core.trackmania.nadeo.live/storageObjects/d92b3dc4-a975-4516-a124-b904b7e47dd8.jpg",
                    NumberCheckpoint = 11,
                    State = EStateValidation.Validate,
                    FileUrl = "https://core.trackmania.nadeo.live/storageObjects/f2eed9ce-099d-467d-8dd7-8012f5071fe3"
                },
                new Maps
                {
                    Name = "Gravis",
                    Author = "Enysado",
                    Difficulty = EDifficulty.Advanced,
                    TypeTrial = ETypeTrial.Classic,
                    Points = 30,
                    TmIoId = "c32bd33c-58ac-4390-9852-00b6fe7e6ac0",
                    TmxLink = "https://trackmania.exchange/tracks/view/45897",
                    VideoLink = "",
                    ImageLink = "https://core.trackmania.nadeo.live/storageObjects/77e61479-0ae0-48b9-988b-8e11c47ef10d.jpg",
                    NumberCheckpoint = 9,
                    State = EStateValidation.Validate,
                    FileUrl = "https://core.trackmania.nadeo.live/storageObjects/9eb87eaf-e986-43c6-95ef-7d76e4e24fe8"
                },
                new Maps
                {
                    Name = "N․O․M․A․D",
                    Author = "WaypointMapping",
                    Difficulty = EDifficulty.Advanced,
                    TypeTrial = ETypeTrial.Classic,
                    Points = 30,
                    TmIoId = "fc9cffc9-0a2e-48db-870d-d71a0af97441",
                    TmxLink = "https://trackmania.exchange/tracks/view/144602",
                    VideoLink = "https://www.youtube.com/watch?v=pICCf6xyZp8",
                    ImageLink = "https://core.trackmania.nadeo.live/storageObjects/add0719f-d99a-429d-95ac-648d2d28f077.jpg",
                    NumberCheckpoint = 9,
                    State = EStateValidation.Validate,
                    FileUrl = "https://core.trackmania.nadeo.live/storageObjects/715248d0-5927-4b25-b96b-c6278a8356c3"
                },
                new Maps
                {
                    Name = "CCP#23 - Subzero \u2744",
                    Author = "Roquete ft' Whiskey",
                    Difficulty = EDifficulty.Advanced,
                    TypeTrial = ETypeTrial.Classic,
                    Points = 30,
                    TmIoId = "1debe74b-1cc8-4852-8f83-8f7e19d139f4",
                    TmxLink = "https://trackmania.exchange/tracks/view/147223",
                    VideoLink = "https://www.youtube.com/watch?v=wVVv4mnmXDo",
                    ImageLink = "https://core.trackmania.nadeo.live/storageObjects/8f7edaae-1951-48d4-8787-32d0fc7b4058.jpg",
                    NumberCheckpoint = 8,
                    State = EStateValidation.Validate,
                    FileUrl = "https://core.trackmania.nadeo.live/storageObjects/fbb2f993-b0b9-4415-a693-2048f17801e9"
                },
                new Maps
                {
                    Name = "Skyreach",
                    Author = "Karlberg.",
                    Difficulty = EDifficulty.Hard,
                    TypeTrial = ETypeTrial.Classic,
                    Points = 35,
                    TmIoId = "d8ffc872-0102-4c11-9911-84224cb7b0a3",
                    TmxLink = "https://trackmania.exchange/tracks/view/62195",
                    VideoLink = "https://www.youtube.com/watch?v=rzPzmcwPxDQ",
                    ImageLink = "https://core.trackmania.nadeo.live/storageObjects/0c56c652-9295-4d60-9964-49a9eb4a1484.jpg",
                    NumberCheckpoint = 7,
                    State = EStateValidation.Validate,
                    FileUrl = "https://core.trackmania.nadeo.live/storageObjects/15b34fa7-f277-4155-8f77-6893d9dec7ba"
                },
                new Maps
                {
                    Name = "Zelos․",
                    Author = "aZixTM",
                    Difficulty = EDifficulty.Hard,
                    TypeTrial = ETypeTrial.Classic,
                    Points = 35,
                    TmIoId = "0c3dbd2d-75a9-4adb-bb0e-d0c7a22a0b0f",
                    TmxLink = "https://trackmania.exchange/tracks/view/72700",
                    VideoLink = "https://www.youtube.com/watch?v=G08obhbfIKU",
                    ImageLink = "https://core.trackmania.nadeo.live/storageObjects/9b401b3f-b2a1-467a-848e-c709b383f89e.jpg",
                    NumberCheckpoint = 11,
                    State = EStateValidation.Validate,
                    FileUrl = "https://core.trackmania.nadeo.live/storageObjects/d5478e4b-ba04-4272-8ee9-8c7d7cf6ab6d"
                },
                new Maps
                {
                    Name = "Symbiosis",
                    Author = "Roquete",
                    Difficulty = EDifficulty.Hard,
                    TypeTrial = ETypeTrial.Classic,
                    Points = 45,
                    TmIoId = "2c4879ea-6f5a-4c6b-b26f-e289e7bc1af9",
                    TmxLink = "https://trackmania.exchange/tracks/view/71552",
                    VideoLink = "https://www.youtube.com/watch?v=Q8-LoBoapcE",
                    ImageLink = "https://core.trackmania.nadeo.live/storageObjects/83efc6c9-60ea-45cc-a7ab-17cba642aac5.jpg",
                    NumberCheckpoint = 11,
                    State = EStateValidation.Validate,
                    FileUrl = "https://core.trackmania.nadeo.live/storageObjects/63aeb362-914b-42e7-a844-0bf8407ba260"
                },
                new Maps
                {
                    Name = "ITITANI",
                    Author = "simo_900",
                    Difficulty = EDifficulty.Hard,
                    TypeTrial = ETypeTrial.Classic,
                    Points = 50,
                    TmIoId = "d048efe3-c41c-4075-9aa5-6c2faded667e",
                    TmxLink = "https://trackmania.exchange/tracks/view/67218",
                    VideoLink = "https://www.youtube.com/watch?v=7WBTHCSlU7U",
                    ImageLink = "https://core.trackmania.nadeo.live/storageObjects/83a97120-af5f-4859-b023-1d920e1dc0ca.jpg",
                    NumberCheckpoint = 12,
                    State = EStateValidation.Validate,
                    FileUrl = "https://core.trackmania.nadeo.live/storageObjects/bd1dd79c-610f-4001-bb74-fc014c733673"
                },
                new Maps
                {
                    Name = "Everything Backwards",
                    Author = "Arth4nn",
                    Difficulty = EDifficulty.Hard,
                    TypeTrial = ETypeTrial.Classic,
                    Points = 60,
                    TmIoId = "0d12b92e-b28e-4751-9ee8-9613ae10839e",
                    TmxLink = "https://trackmania.exchange/tracks/view/53732",
                    VideoLink = "https://www.youtube.com/watch?v=eSmHGpkU6FQ",
                    ImageLink = "https://core.trackmania.nadeo.live/storageObjects/74ab9d9d-0fd1-420c-8962-1ce26a2728a7.jpg",
                    NumberCheckpoint = 25,
                    State = EStateValidation.New,
                    FileUrl = "https://core.trackmania.nadeo.live/storageObjects/270640fe-695a-4fe8-8d73-b724edfe1ac8"
                },
                new Maps
                {
                    Name = "Purple Quarter",
                    Author = "Dlgger",
                    Difficulty = EDifficulty.Expert,
                    TypeTrial = ETypeTrial.Classic,
                    Points = 65,
                    TmIoId = "e743cfca-b554-49a4-8283-e8f55e0af470",
                    TmxLink = "https://trackmania.exchange/tracks/view/35847",
                    VideoLink = "",
                    ImageLink = "https://core.trackmania.nadeo.live/storageObjects/4c1f1ec2-ab58-4941-b1ea-5a72e30d9c0f.jpg",
                    NumberCheckpoint = 10,
                    State = EStateValidation.Validate,
                    FileUrl = "https://core.trackmania.nadeo.live/storageObjects/c9bb7d46-8899-4cc3-89b9-a07892a642b1"
                },
                new Maps
                {
                    Name = "Purple Quarter 2",
                    Author = "Dlgger",
                    Difficulty = EDifficulty.Expert,
                    TypeTrial = ETypeTrial.Classic,
                    Points = 65,
                    TmIoId = "1b2d6b0c-0eb2-4994-9bc3-786ce0a0f5e4",
                    TmxLink = "https://trackmania.exchange/tracks/view/56318",
                    VideoLink = "",
                    ImageLink = "https://core.trackmania.nadeo.live/storageObjects/7a5e1e2e-0a21-4ac4-9071-9ed55ead14b6.jpg",
                    NumberCheckpoint = 8,
                    State = EStateValidation.Validate,
                    FileUrl = "https://core.trackmania.nadeo.live/storageObjects/20870ee6-6931-41f3-875a-1d9531dd2599"
                },
                new Maps
                {
                    Name = "Sienna",
                    Author = "Nixotica",
                    Difficulty = EDifficulty.Expert,
                    TypeTrial = ETypeTrial.Classic,
                    Points = 70,
                    TmIoId = "f8254979-9dda-4698-9659-683f8567df8c",
                    TmxLink = "https://trackmania.exchange/tracks/view/46594",
                    VideoLink = "",
                    ImageLink = "https://core.trackmania.nadeo.live/storageObjects/5d0e0258-ec0b-4753-9947-9bcfc477f574.jpg",
                    NumberCheckpoint = 25,
                    State = EStateValidation.Validate,
                    FileUrl = "https://core.trackmania.nadeo.live/storageObjects/c1f57f40-6329-4ad4-b4f9-d2ae57cc12d7"
                },
                new Maps
                {
                    Name = "ITYPHONI",
                    Author = "simo_900",
                    Difficulty = EDifficulty.Expert,
                    TypeTrial = ETypeTrial.Classic,
                    Points = 75,
                    TmIoId = "00fdbff0-6655-4805-a8d3-e4d4f4052f07",
                    TmxLink = "https://trackmania.exchange/tracks/view/126939",
                    VideoLink = "https://www.youtube.com/watch?v=-2RKkGlyS-o",
                    ImageLink = "https://core.trackmania.nadeo.live/storageObjects/a62b0418-50ea-4333-9902-d42102b64904.jpg",
                    NumberCheckpoint = 10,
                    State = EStateValidation.Validate,
                    FileUrl = "https://core.trackmania.nadeo.live/storageObjects/835f564f-1ce3-4542-a467-9f980d2ffd79"
                },
                new Maps
                {
                    Name = "Unsafe Path",
                    Author = "OstrAleX_TM",
                    Difficulty = EDifficulty.Expert,
                    TypeTrial = ETypeTrial.Classic,
                    Points = 80,
                    TmIoId = "ec9e1f23-65b0-4a14-88b9-e092eb9cab91",
                    TmxLink = "https://trackmania.exchange/tracks/view/98964",
                    VideoLink = "https://www.youtube.com/watch?v=vldHtlTwpQg",
                    ImageLink = "https://core.trackmania.nadeo.live/storageObjects/e5c18286-5718-4a1c-a44e-ebf0d93aa7c0.jpg",
                    NumberCheckpoint = 15,
                    State = EStateValidation.Validate,
                    FileUrl = "https://core.trackmania.nadeo.live/storageObjects/98fa9011-a9b0-41ef-90b3-ac6d379badfe"
                },
                
                // new Maps
                // {
                //     Name = "Defiant",
                //     Author = "",
                //     Difficulty = EDifficulty.Insane,
                //     TypeTrial = ETypeTrial.Classic,
                //     Points = 70,
                //     TmIoId = "",
                //     TmxLink = "",
                //     VideoLink = "",
                //     ImageLink = "",
                //     NumberCheckpoint = 7,
                //     State = EStateValidation.Validate,
                //     FileUrl = ""
                // },
            };
            
            _context.Maps.AddRange(maps);
        
            await _context.SaveChangesAsync();
        }
    }
}
