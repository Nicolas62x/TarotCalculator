namespace Tarot;

public class Joueur
{
    public string Name;
    public Joueur(string name) => Name = name;

    public int WinPren = 0;
    public int WinPart = 0;
    public int WinOpp = 0;
    public int Win => WinPren + WinPart + WinOpp;
    public int LoosePren = 0;
    public int LoosePart = 0;
    public int LooseOpp = 0;
    public int Loose => LoosePren + LoosePart + LooseOpp;
    public double WinRate => 100 * Win / Math.Max(1.0, Win + Loose);
    public double WinRatePren => 100 * WinPren / Math.Max(1.0, WinPren + LoosePren);
    public double WinRatePart => 100 * WinPart / Math.Max(1.0, WinPart + LoosePart);
    public double WinRateOpp => 100 * WinOpp / Math.Max(1.0, WinOpp + LooseOpp);
    public int Score = 0;

    public List<TarotGame> Games = new();
    public List<int> ScoreHistory = new();

    public void PlayGame(TarotGame game)
    {
        int score = game.ScoreFinal;

        if (game.Preneur == Name)
        {
            if (score >= 0)
                WinPren++;
            else
                LoosePren++;

            Score += game.Partenaire is null ? score * game.Opps.Length : score * (game.Opps.Length-1);
        }
        else if (game.Partenaire == Name)
        {
            if (score >= 0)
                WinPart++;
            else
                LoosePart++;

            Score += score;
        }
        else if (game.Opps.Contains(Name))
        {
            if (score >= 0)
                LooseOpp++;
            else
                WinOpp++;

            Score -= score;
        }
        else
            throw new ArgumentException("This player does not play in this game");

        Games.Add(game);
    }

    public void UpdateHistory()
    {
        ScoreHistory.Add(Score);
    }
}