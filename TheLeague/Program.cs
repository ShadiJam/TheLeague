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
            var leagueSport = new Sport("Basketball");
            var leagueName = new League("National Basketball Association");
            const string DATA1 = "*Rockets*, _Mike D'Antoni_, _Jeff Bzdelik_, Ryan Anderson, Trevor Ariza, Patrick Beverley, Corey Brewer, James Hardon";
            const string DATA2 = "*Bulls*, _Fred Hoiberg_, _Randy Brown_, Jimmy Butler, Tah Gibson, Rajon Rondo, Tony Snell, Robin Lopez";
            const string DATA3 = "*Knicks*, _Jeff Hornacek_, _Kurt Rambis_, Derrick Williams, Justin Holiday, Thomas Lance, Brandon Jennings, Carmelo Anthony";
            List<Team> teams = new List<Team>();
            List<Coach> coaches = new List<Coach>();
            List<Player> players = new List<Player>();
            Team lastteam = null;
            Coach lastcoach = null;
            Player lastplayer = null;
            Action<string> process = str =>
            {
                if (str.IndexOf("*") == 0)
                {
                    lastteam = new Team(str.Substring(1, str.Length - 1));
                    teams.Add(lastteam);
                } else if (str.IndexOf("_") == 0)
                {
                    lastcoach = new Coach(str.Substring(1, str.Length - 1));
                    coaches.Add(lastcoach);
                } else
                {
                    var p = new Player[] { new Player(str) };
                    players.Add(lastplayer);
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
            
            Console.WriteLine(teams);
            Console.ReadLine();
        }
        class Sport {
            public string name { get; private set; }
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
            IEnumerable<League> leagues = new List<League>();
            public override string ToString()
            {
                return $"{name}";
            }
        }
        class League {
            public string name { get; private set; }
            public League(string name)
            {
                this.name = name;
            }
            IEnumerable<Team> teams = new List<Team>();
            public override string ToString()
            {
                return $"{name}";
            }
        }
        class Team {
            public string name { get; private set; }
            public string hometown;
            public Team(string name = "", string hometown = "")
            {
                this.name = name;
                this.hometown = hometown;
            }
            IEnumerable<Coach> coaches = new List<Coach>();
            IEnumerable<Player> players = new List<Player>();
            public override string ToString()
            {
                return $"{name}";
            }
        }
        class Coach {
            public string name { get; private set; }
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
            public string name;
            public int points;
            public Player(string name)
            {
                this.name = name;
            }
            public override string ToString()
            {
                return $"{name}";
            }
        }

    }
}
