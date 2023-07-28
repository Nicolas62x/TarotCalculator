
using System.Globalization;
using System.Text;
using Tarot;

var Games = File.ReadAllLines("./TarotGG.csv").Skip(1).Select(l => new TarotGame(l.Split(';')));

Dictionary<string, Joueur> Joueurs = new(Games.Select(g => g.Joueurs).SelectMany(x => x).Distinct().Select(x => new KeyValuePair<string,Joueur>(x, new Joueur(x))));
        
Console.WriteLine($"Parties: {Games.Count()}");

foreach (TarotGame Game in Games)
{
    Console.WriteLine(Game);

    foreach (string j in Game.Joueurs)
        Joueurs[j].PlayGame(Game);

    foreach (Joueur j in Joueurs.Values)
        j.UpdateHistory();
}

var Classement = Joueurs.Values.OrderByDescending(x => x.Score);

//Création des fichiers csv

using var historyCSV = File.Open("./output/history.csv", FileMode.Create);
historyCSV.Write(Encoding.Latin1.GetBytes(string.Join(';', Classement.Select(x => x.Name)) + '\n'));

var history = Classement.Select(x => x.ScoreHistory);
for (int i = 0; i < Games.Count(); i++)
    historyCSV.Write(Encoding.Latin1.GetBytes(string.Join(';', history.Select(x => x[i])) + '\n'));

using var ratingCSV = File.Open("./output/rating.csv", FileMode.Create);
ratingCSV.Write(Encoding.Latin1.GetBytes($"Joueur;Points;Victoires;Parties;Ratio\n"));

using var winRatesCSV = File.Open("./output/winrates.csv", FileMode.Create);
winRatesCSV.Write(Encoding.Latin1.GetBytes($"Joueur;WinPren;PartiePren;WrPren;;WinPart;PartiePart;WrPart;;WinOpp;PartieOpp;WrOpp\n"));

foreach (var joueur in Classement)
{
    ratingCSV.Write(Encoding.Latin1.GetBytes($"{joueur.Name};{joueur.Score};{joueur.Win};{joueur.Games.Count};{joueur.WinRate.ToString("0.00", CultureInfo.InvariantCulture)}%\n"));

    string[] Values = new string[] {
        joueur.Name,
        joueur.WinPren.ToString(),
        (joueur.WinPren + joueur.LoosePren).ToString(),
        joueur.WinRatePren.ToString("0.00", CultureInfo.InvariantCulture) + '%',
        "",
        joueur.WinPart.ToString(),
        (joueur.WinPart + joueur.LoosePart).ToString(),
        joueur.WinRatePart.ToString("0.00", CultureInfo.InvariantCulture) + '%',
        "",
        joueur.WinOpp.ToString(),
        (joueur.WinOpp + joueur.LooseOpp).ToString(),
        joueur.WinRateOpp.ToString("0.00", CultureInfo.InvariantCulture) + '%',
    };

    winRatesCSV.Write(Encoding.Latin1.GetBytes(string.Join(';', Values) + '\n'));
}
