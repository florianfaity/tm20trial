﻿using System.Runtime.InteropServices;
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
                },
            };
            
            _context.Maps.AddRange(maps);
        
            await _context.SaveChangesAsync();
        }
    }
}
