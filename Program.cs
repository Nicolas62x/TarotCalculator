
using System.Text;
using Tarot;

var Games = File.ReadAllLines("./TarotGG.csv").Skip(1).Select(l => new TarotGame(l.Split(';')));

Dictionary<string, int> Scores = new();
Dictionary<string, int> Win = new();
Dictionary<string, int> Loose = new();

foreach (TarotGame Game in Games)
    foreach (string j in Game.Joueurs)
    {
        Scores[j] = 0;
        Win[j] = 0;
        Loose[j] = 0;
    }
        
Console.WriteLine($"Parties: {Games.Count()}");

using var historyCSV = File.Open("./history.csv", FileMode.Create);
historyCSV.Write(Encoding.Latin1.GetBytes(string.Join(';', Scores.Keys) + '\n'));

foreach (TarotGame Game in Games)
{
    string[] Opps = Game.Opps;
    int score = Game.ScoreFinal;

    Console.WriteLine(Game);

    foreach (string Opp in Opps)
    {
        Scores[Opp] -= score;

        if (score > 0)
            Loose[Opp]++;
        else
            Win[Opp]++;
    }

    if (score > 0)
        Win[Game.Preneur]++;
    else
        Loose[Game.Preneur]++;     
        

    if (Game.Partenaire is not null)
    {
        Scores[Game.Partenaire] += score;
        Scores[Game.Preneur] += score * (Opps.Length - 1);

        if (score > 0)
            Win[Game.Partenaire]++;
        else
            Loose[Game.Partenaire]++;            
    }
    else
        Scores[Game.Preneur] += score * Opps.Length;

    historyCSV.Write(Encoding.Latin1.GetBytes(string.Join(';', Scores.Values)  + '\n'));
}

var Classement = Scores.OrderByDescending(x => x.Value);

using var ratingCSV = File.Open("./rating.csv", FileMode.Create);
ratingCSV.Write(Encoding.Latin1.GetBytes($"Joueur;Points;Victoires;Parties;Ratio\n"));

foreach (var joueur in Classement)
    ratingCSV.Write(Encoding.Latin1.GetBytes($"{joueur.Key};{joueur.Value};{Win[joueur.Key]};{Win[joueur.Key] + Loose[joueur.Key]};{100 * Win[joueur.Key] / (double)(Win[joueur.Key] + Loose[joueur.Key]):#.#}%\n"));
