using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace TheLeague
{
    class Program
    {

        static void Main(string[] args)
        {
            
            var leagueName = new League("National Basketball Association");
            const string DATA1 = "*Rockets*, 'Houston', _Mike D'Antoni_, _Jeff Bzdelik_, -Ryan Anderson-, |1124|, -Trevor Ariza-, |1025|, -Patrick Beverley-, |700|, -Corey Brewer-, |590|, -James Hardon-, |2376|";
            const string DATA2 = "*Bulls*, 'Chicago', _Fred Hoiberg_, _Randy Brown_, -Jimmy Butler-, |1399|, -Taj Gibson-, |1935|, -Rajon Rondo-, |2536|, -Tony Snell-, |338|, -Robin Lopez-, |842|";
            const string DATA3 = "*Knicks*, 'New York', _Jeff Hornacek_, _Kurt Rambis_, -Derrick Williams-, |746|, -Justin Holiday-, |239|, -Thomas Lance-, |484|, -Brandon Jennings-, |175|, -Carmelo Anthony-, |1573|";
            List<Team> teams = new List<Team>();
            List<Coach> coaches = new List<Coach>();
            List<Player> players = new List<Player>();
            Team lastTeam = null;
            Coach lastCoach = null;
            Player lastPlayer = null;
            Action<string> process = str =>
            {
                if (str.IndexOf("*") == 0)
                {
                    lastTeam = new Team(str.Substring(1, str.Length - 1));
                    teams.Add(lastTeam);
                }
                else if (str.IndexOf("'") == 0)
                {
                    lastTeam.homeTown = (str.Substring(1, str.Length - 1));
                    teams.Add(lastTeam);
                }
                else if (str.IndexOf("_") == 0)
                {
                    lastCoach = new Coach(str.Substring(1, str.Length - 1));
                    coaches.Add(lastCoach);
                    lastTeam.coaches = lastTeam.coaches.Concat(new Coach[] { lastCoach });
                }
                else if (str.IndexOf("-") == 0)
                {       // not sure how to fix this! How else do I specify that this is the string name?
                    lastPlayer.name = new Player(str.Substring(1, str.Length - 1)); 
                    players.Add(lastPlayer);
                    lastTeam.players = lastTeam.players.Concat(new Player[] { lastPlayer });
                }
                else if (str.IndexOf("|") == 0)
                {
                    lastPlayer.points = int.Parse(str.Substring(1, str.Length - 1));
                    players.Add(lastPlayer);
                }
            };
            foreach (string s in DATA1.Split(new char[] { ',' }))
            {
                process(s.Trim());
            }
            foreach (string s in DATA2.Split(new char[] { ',' }))
            {
                process(s.Trim());
            }
            foreach (string s in DATA3.Split(new char[] { ',' }))
            {
                process(s.Trim());
            }
            Directory.CreateDirectory("html");

            File.WriteAllText(@"html/index.html", teams.ElementAt(0).ToString());
            //creates list of players in league
            var sportLeague = new Sport("Basketball");
            for(var k=0; k<1; k++) {
                var l = new League("");
                sportLeague.leagues = sportLeague.leagues.Concat(new List<League> { l });
                    //for each of the three teams
                    for (var n = 0; n < 3; n++)
                    {
                        var t = new Team("");
                        l.teams = l.teams.Concat(new List<Team> { t });

                        //for each of the five players in each team
                        for (var m = 0; m < 5; m++) {
                        var p = new Player("", 0);
                        t.players = t.players.Concat(new List<Player> { p });
                    }
                }
            }
            //concats list of players points from the league to the players in each team 
            Func<Team, IEnumerable<Player>> getPlayersFromTeam = (t) => t.players;
            Func<League, IEnumerable<Player>> getPlayersFromLeague = (l) =>
            {
                return l.teams.Aggregate(new List<Player>(),
                    (acc, t) => acc.Concat(getPlayersFromTeam(t)).ToList());
            };
            Func<Sport, IEnumerable<Player>> getAllPlayers = (s) =>
            {
                return s.leagues.Aggregate(new List<Player>(),
                    (acc, l) => acc.Concat(getPlayersFromLeague(l)).ToList());
            };
            IEnumerable<Player> allPlayers = getAllPlayers(sportLeague);
            foreach(var p in players)
            {
                Console.WriteLine($"{p.name}{p.points}");
            }
            Func<Sport, Player> playerOfTheYear = (sport) =>
            {
                Player maxPoints = new Player("", 0); 
                foreach (Player p in getAllPlayers(sport))
                {
                    if (p.points > maxPoints.points)
                    {
                        maxPoints = p;
                    }
                }
                return maxPoints;
            };
            Console.WriteLine(playerOfTheYear(sportLeague).points);
            // need to add in a Func to create averages of each team and then getCoachOfTheYear
            // need to figure out how to print this to the damn console!
        }
    }

    class Sport
    {
        public string name;
        public Sport(string name)
        {
            this.name = name;
        }
        enum SportType
        {
            Basketball,
            Tennis,
            Soccer,
            Football,
        }
        public IEnumerable<League> leagues = new List<League>();
       
        public static int Max(IEnumerable<int> players) //don't need this if using aggregates.
        {
        List<Player> result = new List<Player>();
        Player max = result.Max();
        foreach (int points in players)
            {
                result.Max();
            }
            return players.Max();
        }
        public override string ToString()
            {
                return $"{name}";
            }
            
        }
    class League
        {
        public string name;
        public League(string name)
            {
                this.name = name;
            }
        public IEnumerable<Team> teams = new List<Team>();
            
        public override string ToString()
            {
                return $"{name}";
            }
        }
    class Team
        {
        public string name;
        public string homeTown;
        public Team(string name)
            {
                this.name = name;
            }
        public IEnumerable<Coach> coaches = new List<Coach>();
        public IEnumerable<Player> players = new List<Player>();
        
        public override string ToString()
            {
                string c = String.Join(", ", coaches);
                string p = String.Join(", ", players);
                return String.Format(@"
            <div>
                <h1>{0} {1}</h1>
                    <p>Coaches: {2} </p>
                    <p>Players: {3} </p>
            </div>
            ", homeTown, name, c, p);
            }
        }
    class Coach
        {
         public string name;
         public Coach(string name)
            {
                this.name = name;
            }
         public override string ToString()
            {
                return $"{name}";
            }
        }
    class Player
        {
        public string name;
        public int points; 
        public Player(string name, int points)
            {
                this.name = name;
                this.points = points;
            }
          
        public override string ToString()
            { 
            string n = String.Join(", ", name);
            string p = String.Join(", ", points);
                return String.Format(@"
            <div>
                <p>{0}: {1}</p>  
            </div>
            ", n, p);
            }
        }
    }



