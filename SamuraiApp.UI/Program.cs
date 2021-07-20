using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SamuraiApp.UI
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();
        // private static SamuraiContext _contextNT = new SamuraiContextNoTracking();

        private static void Main(string[] args)
        {
            // GetSamurais();  //setup with _contextNT
            // QueryFilters();  //setup with _contextNT
            // QueryAggregates();  //setup with _contextNT

            // AddSamuraisByName("Wade", "Kota", "Ember", "Sammie");
            // AddVariousTypes();
            /*Console.Write("Press any key...");
            Console.ReadKey();*/
            // RetrieveAndUpdateSamurai();
            // RetrieveAndUpdateMultipleSamurais();
            // MultipleDatabaseOperations();
            // RetrieveAndDeleteASamurai();
            // QueryAndUpdateBattles_Disconnected();

            // InsertNewSamuraiWithQuote();
            // InsertNewSamuraiWithManyQuotes();
            // AddQuoteToExistingSamuraiWhileTracked();
            // AddQuoteToExistingSamuraiNotTracked(2);
            // Simpler_AddQuoteToExistingSamuraiNotTracked(2);

            // EagerLoadSamuraiWithQuotes();
            // ProjectSomeProperties();
            // ProjectSamuraisWithQuotes();
            // ExplicitLoadQuotes();
            // LazyLoadQuotes();

            // FilteringWithRelatedData();
            // ModifyingRelatedDataWhenTracked();
            // ModifyingRelatedDataWhenNotTracked();

            // AddingNewSamuraiToAnExistingBattle();
            // ReturnBattleWithSamurais();
            // ReturnAllBattlesWithSamurais();
            // AddAllSamuraisToAllBattles();
            // RemoveSamuraiFromABattle();
            // RemoveSamuraiFromABattleExplicit();

            // AddNewSamuraiWithHorse();
            // AddNewHorseToSamuraiUsingId();
            // AddNewHorseToSamuraiObject();
            // AddNewHorseToDisconnectedSamuraiObject();
            // ReplaceAHorse();
            // GetSamuraiWithHorse();
            // GetHorsesWithSamurai();
        }

        private static void AddVariousTypes()
        {
            _context.AddRange(new Samurai { Name = "Shikimbo" },
                              new Samurai { Name = "Okimoto" },
                              new Battle { Name = "Battle of Nagashino" },
                              new Battle { Name = "Battle of Okinawa" });
            /*_context.Samurais.AddRange(
                new Samurai { Name = "Skikimbo" },
                new Samurai { Name = "Okimoto" });
            _context.Battles.AddRange(
                new Battle { Name = "Battle of Nagashino" },
                new Battle { Name = "Battle of Okinawa" });*/
            _context.SaveChanges();
        }

        private static void AddSamuraisByName(params string[] names)
        {
            foreach (string name in names)
            {
                _context.Samurais.Add(new Samurai { Name = name });
            }
            _context.SaveChanges();
        }

        private static void GetSamurais()
        {
            var samurais = _context.Samurais
                .TagWith("ConsoleApp.Program.GetSamurais method")
                .ToList();
            Console.WriteLine($"Samurai count is {samurais.Count}");
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }

        private static void QueryFilters()
        {
            // var name = "Kota";
            // var samurais = _context.Samurais.Where(s => s.Name == name).ToList();
            var filter = "S%";
            var samurais = _context.Samurais
                .Where(s => EF.Functions.Like(s.Name, filter)).ToList();
        }

        private static void QueryAggregates()
        {
            // var name = "Wade";
            // var samurai = _context.Samurais.Where(s => s.Name == name).FirstOrDefault();
            var samurai = _context.Samurais.Find(2);
        }

        private static void RetrieveAndUpdateSamurai()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += "Clyde";
            _context.SaveChanges();
        }

        private static void RetrieveAndUpdateMultipleSamurais()
        {
            var samurais = _context.Samurais.Skip(1).Take(4).ToList();
            samurais.ForEach(s => s.Name += "Pate");
            _context.SaveChanges();
        }

        private static void MultipleDatabaseOperations()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += "Kota";
            _context.Samurais.Add(new Samurai { Name = "Shino" });
            _context.SaveChanges();
        }

        private static void RetrieveAndDeleteASamurai()
        {
            var samurai = _context.Samurais.Find(10);
            _context.Samurais.Remove(samurai);
            _context.SaveChanges();
        }

        private static void QueryAndUpdateBattles_Disconnected()
        {
            List<Battle> disconnectedBattles;
            using (var context1 = new SamuraiContext())
            {
                disconnectedBattles = _context.Battles.ToList();
            } //context1 is disposed
            disconnectedBattles.ForEach(b =>
            {
                b.StartDate = new DateTime(1570, 01, 01);
                b.EndDate = new DateTime(1570, 12, 1);
            });

            using (var context2 = new SamuraiContext())
            {
                context2.UpdateRange(disconnectedBattles);
                context2.SaveChanges();
            }
        }

        private static void InsertNewSamuraiWithQuote()
        {
            var samurai = new Samurai
            {
                Name = "Kambei Shimada",
                Quotes = new List<Quote>
                {
                    new Quote { Text = "I've come to save you" }
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void InsertNewSamuraiWithManyQuotes()
        {
            var samurai = new Samurai
            {
                Name = "Kyouzi",
                Quotes = new List<Quote>
                {
                    new Quote { Text = "Watch out for my sharp sword!" },
                    new Quote { Text = "I told you to watch out for my sharp sword! Oh well!" }
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void AddQuoteToExistingSamuraiWhileTracked()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Quotes.Add(new Quote
            {
                Text = "I bet you're happy that I saved you!"
            });
            _context.SaveChanges();
        }

        private static void AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            var samurai = _context.Samurais.Find(samuraiId);
            samurai.Quotes.Add(new Quote
            {
                Text = "Now that I saved you, will you feed me dinner?"
            });
            using (var newContext = new SamuraiContext())
            {
                newContext.Samurais.Attach(samurai);  // Uses .Attach for performance instead of .Update
                newContext.SaveChanges();
            }
        }

        private static void Simpler_AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            var quote = new Quote { Text = "Thanks for dinner!", SamuraiId = samuraiId };
            using var newContext = new SamuraiContext();
            newContext.Quotes.Add(quote);
            newContext.SaveChanges();
        }

        private static void EagerLoadSamuraiWithQuotes() // Eage Loading is all or nothing
        {
            // var samuraiWithQuotes = _context.Samurais.Include(s => s.Quotes).ToList();
            // var splitQuery = _context.Samurais.AsSplitQuery().Include(s => s.Quotes).ToList();
            var filteredInclude = _context.Samurais
                .Include(s => s.Quotes.Where(q => q.Text.Contains("Thanks"))).ToList(); // Can Include multiple .Include queries
        }

        private static void ProjectSomeProperties() // Projecting an undefined anonymous type
        {
            var someProperties = _context.Samurais.Select(s => new { s.Id, s.Name }).ToList();
            // anonymous types are only recognized within stated function unless Casted to a list of defined types =>
            var idAndNames = _context.Samurais.Select(s => new IdAndName(s.Id, s.Name)).ToList();
        }
        public struct IdAndName
        {
            public IdAndName(int id, string name)
            {
                Id = id;
                Name = name;
            }
            public int Id;
            public string Name;
        }

        private static void ProjectSamuraisWithQuotes()
        {
            //var somePropsWithQuotes = _context.Samurais
            //    .Select(s => new { s.Id, s.Name, s.Quotes })
            //    .ToList();

            //var somePropsWithQuoteCount = _context.Samurais
            //    .Select(s => new { s.Id, s.Name, NumberOfQuotes = s.Quotes.Count })
            //    .ToList();

            //var somePropsWithQuoteSpecifics = _context.Samurais        // returns circular reference
            //    .Select(s => new
            //    {
            //        s.Id,
            //        s.Name,
            //        HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy"))
            //    })
            //    .ToList();

            var samuraiAndQuotes = _context.Samurais
                .Select(s => new
                {
                    Samurai = s,
                    HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy"))
                })
                .ToList();
            var firstsamurai = samuraiAndQuotes[0].Samurai.Name += " The Happiest";

            // _context.SaveChanges();
        }

        private static void ExplicitLoadQuotes()
        {
            // make sure there's a horse in the DB, then clear the context's change tracker
            // _context.Set<Horse>().Add(new Horse { SamuraiId = 1, Name = "Mr. Ed" });
            // _context.SaveChanges();
            // _context.ChangeTracker.Clear();
            // ---------------------------
            var samurai = _context.Samurais.Find(1);
            _context.Entry(samurai).Collection(s => s.Quotes).Load();
            _context.Entry(samurai).Reference(s => s.Horse).Load();
        }

        private static void LazyLoadQuotes()
        {
            var samurai = _context.Samurais.Find(2);
            var quoteCount = samurai.Quotes.Count();  // won't run without LL setup
        }

        private static void FilteringWithRelatedData()
        {
            var samurais = _context.Samurais
                            .Where(s => s.Quotes.Any(q => q.Text.Contains("happy")))
                            .ToList();
        }

        private static void ModifyingRelatedDataWhenTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes)
                                            .FirstOrDefault(s => s.Id == 2);
            samurai.Quotes[0].Text = "Did you hear that";
            _context.SaveChanges();
        }

        private static void ModifyingRelatedDataWhenNotTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes)
                                            .FirstOrDefault(s => s.Id == 2);
            var quote = samurai.Quotes[0];
            quote.Text += "Did you hear that again?";

            using var newContext = new SamuraiContext();
            // newContext.Quotes.Update(quote);
            newContext.Entry(quote).State = EntityState.Modified;
            newContext.SaveChanges();
        }

        private static void AddingNewSamuraiToAnExistingBattle()
        {
            var battle = _context.Battles.FirstOrDefault();
            battle.Samurais.Add(new Samurai { Name = "Takeda Shingen" });
            _context.SaveChanges();
        }

        private static void ReturnBattleWithSamurais()
        {
            var battle = _context.Battles.Include(b => b.Samurais).FirstOrDefault();
        }

        private static void ReturnAllBattlesWithSamurais()
        {
            var battles = _context.Battles.Include(b => b.Samurais).ToList();
        }

        private static void AddAllSamuraisToAllBattles()
        {
            var allBattles = _context.Battles.Include(b => b.Samurais).ToList();
            var allSamurais = _context.Samurais.ToList();
            // Not optimal if you have a large amount of data as it could slow down the change tracker
            foreach (var battle in allBattles)
            {
                battle.Samurais.AddRange(allSamurais);
            }
            _context.SaveChanges();
        }

        private static void RemoveSamuraiFromABattle()
        {
            var battleWithSamurai = _context.Battles
                .Include(b => b.Samurais.Where(s => s.Id == 12))
                .Single(s => s.BattleId == 1);
            var samurai = battleWithSamurai.Samurais[0];
            battleWithSamurai.Samurais.Remove(samurai);
            _context.SaveChanges();
        }

        private static void RemoveSamuraiFromABattleExplicit()
        {
            var b_s = _context.Set<BattleSamurai>()
                .SingleOrDefault(bs => bs.BattleId == 1 && bs.SamuraiId == 10);
            if (b_s != null)
            {
                _context.Remove(b_s); // context.Set<BattleSamurai>().Remove works too
                _context.SaveChanges();
            }
        }

        private static void AddNewSamuraiWithHorse()
        {
            var samurai = new Samurai { Name = "Jina Ujichika" };
            samurai.Horse = new Horse { Name = "Juniper" };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void AddNewHorseToSamuraiUsingId()
        {
            var horse = new Horse { Name = "Scooter", SamuraiId = 2 };
            _context.Add(horse);
            _context.SaveChanges();
        }

        private static void AddNewHorseToSamuraiObject()
        {
            var samurai = _context.Samurais.Find(12);
            samurai.Horse = new Horse { Name = "Red Thunda" };
            _context.SaveChanges();
        }

        private static void AddNewHorseToDisconnectedSamuraiObject()
        {
            var samurai = _context.Samurais.AsNoTracking().FirstOrDefault(s => s.Id == 5);
            samurai.Horse = new Horse { Name = "Bucky" };

            using var newContext = new SamuraiContext();
            newContext.Samurais.Attach(samurai);
            newContext.SaveChanges();
        }

        private static void ReplaceAHorse()
        {
            // var samurai = _context.Samurais.Include(s => s.Horse)
            //                                 .FirstOrDefault(s => s.Id == 5);
            // samurai.Horse = new Horse { Name = "Trigger" };
            // OR
            var horse = _context.Set<Horse>().FirstOrDefault(h => h.Name == "Mr. Ed");
            horse.SamuraiId = 5;   // currently owns Trigger!
            _context.SaveChanges();
        }

        private static void GetSamuraiWithHorse()
        {
            var samurais = _context.Samurais.Include(s => s.Horse).ToList();
        }

        private static void GetHorsesWithSamurai()
        {
            // var horseOnly = _context.Set<Horse>().Find(3);

            // var horseWithSamurai = _context.Samurais.Include(s => s.Horse)
            //                                          .FirstOrDefault(s => s.Horse.Id == 3);

            var horseSamuraiPairs = _context.Samurais
                .Where(s => s.Horse != null)
                .Select(s => new { Horse = s.Horse, Samurai = s })
                .ToList();
        }
    }
}
