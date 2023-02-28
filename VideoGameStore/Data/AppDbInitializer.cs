using VideoGameStore.Data.Enums;
using VideoGameStore.Models;
using VideoGameStore.Data.Static;
using Microsoft.AspNetCore.Identity;

namespace VideoGameStore.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            // Using using because Dispose() will be called even if there is an exception that occurs.
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                // Create reference to App Data Context file to get/send info with DB.
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                // Ensure it is created so we can use.
                context.Database.EnsureCreated();

                // Add all the data if none exists:

                // Publishers
                if (!context.Publishers.Any())
                {
                    context.Publishers.AddRange(new List<Publisher>()
                    {
                        new Publisher()
                        {
                            CompanyName = "Team Cherry",
                            About = "Team Cherry, an Australian video game developer and publisher of Hollow Knight and Hollow Knight: Silksong.",
                            LogoURL = "https://images.squarespace-cdn.com/content/v1/606d4deb4db8c15ea53b3624/c176e609-2480-47a0-ab31-536b0e423e9b/logo2.png"

                        },
                        new Publisher()
                        {
                            CompanyName = "Supergiant Games",
                            About = "Supergiant Games is a small independent game development studio in San Francisco",
                            LogoURL = "https://static.wikia.nocookie.net/logopedia/images/5/54/Supergiant_games_logo.png/revision/latest?cb=20210219003456"
                        },
                        new Publisher()
                        {
                            CompanyName = "Activision Blizzard",
                            About = "Activision is the leading worldwide developer, publisher and distributor of interactive entertainment and products on consoles, mobile and PC.",
                            LogoURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/36/Activision_Blizzard.svg/2560px-Activision_Blizzard.svg.png"
                        },
                        new Publisher()
                        {
                            CompanyName = "Blizzard Entertainment",
                            About = "Creators of the Warcraft, Diablo, StarCraft, and Overwatch series.",
                            LogoURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/23/Blizzard_Entertainment_Logo_2015.svg/1200px-Blizzard_Entertainment_Logo_2015.svg.png"
                        }
                    });
                    context.SaveChanges();
                }

                //Developers
                if (!context.Developers.Any())
                {
                    context.Developers.AddRange(new List<Developer>()
                    {
                        new Developer()
                        {
                            CompanyName = "Team Cherry",
                            About = "Team Cherry, an Australian video game developer and publisher of Hollow Knight and Hollow Knight: Silksong.",
                            LogoURL = "https://images.squarespace-cdn.com/content/v1/606d4deb4db8c15ea53b3624/c176e609-2480-47a0-ab31-536b0e423e9b/logo2.png"

                        },
                        new Developer()
                        {
                            CompanyName = "Supergiant Games",
                            About = "Supergiant Games is a small independent game development studio in San Francisco",
                            LogoURL = "https://static.wikia.nocookie.net/transistor/images/5/5f/Supergiant_Games_Logo.png/revision/latest?cb=20150706054401"
                        },
                        new Developer()
                        {
                            CompanyName = "Activision Blizzard",
                            About = "Activision is the leading worldwide developer, publisher and distributor of interactive entertainment and products on consoles, mobile and PC.",
                            LogoURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/36/Activision_Blizzard.svg/2560px-Activision_Blizzard.svg.png"
                        }
                    });
                    context.SaveChanges();
                }

                //VideoGames
                if (!context.VideoGames.Any())
                {
                    context.VideoGames.AddRange(new List<VideoGame>()
                        {
                            new VideoGame()
                            {
                                Title = "Hollow Knight",
                                Description = "Hollow Knight is a challenging 2D action-adventure. You’ll explore twisting caverns, battle tainted creatures and escape intricate traps, all to solve an ancient long-hidden mystery.",
                                Price = 29.99,
                                ImageURL = "https://image.api.playstation.com/cdn/UP1822/CUSA13632_00/GuFQKWlrIVODEA1su20fCOrNZEYX5CB9.png",
                                ReleaseDate = new DateTime(2017, 02, 23),
                                DeveloperId = 1,
                                GameGenre = GameGenre.Platformer
                            },
                            new VideoGame()
                            {
                                Title = "Hades",
                                Description = "Hades is a rogue-like dungeon crawler that combines the best aspects of Supergiant's critically acclaimed titles, including the fast-paced action of Bastion, the rich atmosphere and depth of Transistor, and the character-driven storytelling of Pyre. BATTLE OUT OF HELL As the immortal Prince of the Underworld, you'll wield the powers and mythic weapons of Olympus to break free from the clutches of the god of the dead himself, while growing stronger and unraveling more of the story with each unique escape attempt.",
                                Price = 29.95,
                                ImageURL = "https://cdn1.epicgames.com/min/offer/1200x1600-1200x1600-e92fa6b99bb20c9edee19c361b8853b9.jpg",
                                ReleaseDate = new DateTime(2020, 09, 17),
                                DeveloperId = 2,
                                GameGenre = GameGenre.Action
                            },
                            new VideoGame()
                            {
                                Title = "Diablo IV",
                                Description = "Meet Your Maker\r\n\r\nLilith has returned to Sanctuary, summoned by a dark ritual after eons in exile. Her return ushers in an age of darkness and misery.\r\n\r\nSanctuary, a land once ravaged by war between the High Heavens and Burning Hells, has fallen once more into darkness. Lilith, daughter of Mephisto, Lord of Hatred, has been summoned by dark ritual after eons in exile. Now, hatred threatens to consume Sanctuary as evil spreads and a new wave of cultists and worshippers arise to embrace Lilith’s coming. Only a brave few dare to face this threat…",
                                Price = 69.99,
                                ImageURL = "https://upload.wikimedia.org/wikipedia/en/1/1c/Diablo_IV_cover_art.png",
                                ReleaseDate = new DateTime(2023, 06, 2),
                                DeveloperId = 3,
                                GameGenre = GameGenre.RPG
                            }
                        });
                    context.SaveChanges();
                }

                //Publishers_VideoGames
                if (!context.Publishers_VideoGames.Any())
                {
                    context.Publishers_VideoGames.AddRange(new List<Publisher_VideoGame>()
                    {
                        new Publisher_VideoGame()
                        {
                            PublisherId = 3,
                            VideoGameId = 3
                        },
                        new Publisher_VideoGame()
                        {
                            PublisherId = 4,
                            VideoGameId = 3
                        },

                         new Publisher_VideoGame()
                        {
                            PublisherId = 2,
                            VideoGameId = 2
                        },
                         new Publisher_VideoGame()
                        {
                            PublisherId = 1,
                            VideoGameId = 1
                        }
                    });
                    context.SaveChanges();
                }
            }
        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {

                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                string adminUserEmail = "admin@VideoGameStore.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new ApplicationUser()
                    {
                        FullName = "Admin User",
                        UserName = "app-admin",
                        Email = adminUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdminUser, "#iodu34;j(*Dh;etrgj;h3p489HGShuk43");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }


                string appUserEmail = "user@VideoGameStore.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new ApplicationUser()
                    {
                        FullName = "Application User",
                        UserName = "app-user",
                        Email = appUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAppUser, "#iodu34;j(*Dh;etrgj;h3p489HGShuk43");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    }
}
