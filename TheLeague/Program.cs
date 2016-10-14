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
            var sportLeague = new Sport("Basketball");
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
                    lastTeam.homeTown = (str.Substring(1, str.Length -1));
                    teams.Add(lastTeam);
                }
                else if (str.IndexOf("_") == 0)
                {
                    lastCoach = new Coach(str.Substring(1, str.Length - 1));
                    coaches.Add(lastCoach);
                    lastTeam.coaches = lastTeam.coaches.Concat(new Team[] { lastTeam });
                }
                else if (str.IndexOf("-") == 0)
                {
                    lastPlayer = new Player(str.Substring(1, str.Length - 1));
                    players.Add(lastPlayer);
                    lastTeam.players = lastTeam.players.Concat(new Team[] { lastTeam });
                }
                else if (str.IndexOf("|") == 0)
                {
                    lastPlayer.points = int.Parse(str.Substring(1, str.Length - 1));
                    players.Add(lastPlayer);
                }
            };
            foreach(string s in DATA1.Split(new char[] { ',' }))
            {
                process(s.Trim());
            }
            foreach(string s in DATA2.Split(new char[] { ',' }))
            {
                process(s.Trim());
            }
            foreach(string s in DATA3.Split(new char[] { ',' }))
            {
                process(s.Trim());
            }
            
            Directory.CreateDirectory("html");
            File.WriteAllText(@"html/index.html", team.ToString);

            
            Console.ReadLine();
        }
        class Sport {
            public string name; 
            public Sport(string name)
            {
                this.name = name;
            }
            enum SportType {
                Basketball,
                Tennis,
                Soccer,
                Football,
            }
            public IEnumerable<League> leagues = new List<League>();
            public override string ToString()
            {
                return $"{name}";
            }
        }
        class League {
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
        class Team {
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
        class Coach {
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
        class Player {
            public string name { get; set; }
            public int points;
            public Player(string name)
            {
                this.name = name;
            }
            public override string ToString()
            {
                return $"{name + points}";
            }
        }

    }
}
