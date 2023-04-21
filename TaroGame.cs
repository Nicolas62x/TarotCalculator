
namespace Tarot;

enum InfoIdx : int
{
    Mise = 0,
    Preneur,
    Partenaire,
    Roi,
    Opp1,
    Opp2,
    Opp3,
    Opp4,
    B21,
    B1,
    BE,
    Résultat,
    PTB,
    Poignée,
    SA,
    ST,
    Chelem
}

public enum Mises : int {
    Pousse = 1,
    Garde = 2,
    Garde_Sans = 4,
    Garde_Contre = 6
}

public class TarotGame
{
    string[] Infos;
    public TarotGame(string[] GameInfo) => Infos = GameInfo;

    public override string ToString()
    {
        return $"{Prise} de {Preneur}{(Partenaire is null ? "" : $" avec {Partenaire}")}: {Score} => {ScoreFinal} contre: {string.Join(", ", Opps)}";
    }

    string? StringOrNull(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim() == "X" ? null : value.Trim();

    public string Preneur => Infos[(int)InfoIdx.Preneur].Trim();
    public string? Partenaire => StringOrNull(Infos[(int)InfoIdx.Partenaire]);
    public string Roi => Infos[(int)InfoIdx.Roi].Trim();

    public string[] Opps {
        get {

            List<string?> temp = new();

            temp.Add(StringOrNull(Infos[(int)InfoIdx.Opp1]));
            temp.Add(StringOrNull(Infos[(int)InfoIdx.Opp2]));
            temp.Add(StringOrNull(Infos[(int)InfoIdx.Opp3]));
            temp.Add(StringOrNull(Infos[(int)InfoIdx.Opp4]));

            string[] res = (string[])temp.Where(x => x is not null && x is not "X").ToArray();

            if (res.Length < 3)
                throw new ArgumentException("Less than 3 opponents is not possible");

            return res;
        }
    }

    public string[] Joueurs {
        get {

            List<string?> temp = new();

            temp.Add(Infos[(int)InfoIdx.Preneur]);
            temp.Add(StringOrNull(Infos[(int)InfoIdx.Partenaire]));
            temp.Add(StringOrNull(Infos[(int)InfoIdx.Opp1]));
            temp.Add(StringOrNull(Infos[(int)InfoIdx.Opp2]));
            temp.Add(StringOrNull(Infos[(int)InfoIdx.Opp3]));
            temp.Add(StringOrNull(Infos[(int)InfoIdx.Opp4]));

            string[] res = (string[])temp.Where(x => x is not null && x is not "X").ToArray();

            if (res.Length < 4)
                throw new ArgumentException("Less than 4 opponents is not possible");

            return res;
        }
    }

    public Mises Prise => Infos[(int)InfoIdx.Mise].Trim() switch {
        "Pousse" => Mises.Pousse,
        "Garde" => Mises.Garde,
        "Garde Sans" => Mises.Garde_Sans,
        "Garde Contre" => Mises.Garde_Contre,
        _ => throw new ArgumentException("Invalid Mise")
    };

    public double Score => double.Parse(Infos[(int)InfoIdx.Résultat]);

    public bool B1 => Infos[(int)InfoIdx.B1].Trim() == "1";
    public bool B21 => Infos[(int)InfoIdx.B21].Trim() == "1";
    public bool BE => Infos[(int)InfoIdx.BE].Trim() == "1";

    public int PTBBonus => Infos[(int)InfoIdx.PTB].Trim() switch
    {
        "G" => 10,
        "P" => -10,
        "X" => 0,
        _ => throw new ArgumentException("Invalid PTB value")
    };

    public int PgBonus => Infos[(int)InfoIdx.Poignée].Trim() switch
    {
        "SG" => 20,
        "SP" => -20,
        "DG" => 30,
        "DP" => -30,
        "TG" => 40,
        "TP" => -40,
        "X" => 0,
        _ => throw new ArgumentException("Invalid Pg value")
    };

    public int SABonus => Infos[(int)InfoIdx.SA].Trim() switch
    {
        "P" => 10,
        "O" => -10,
        "X" => 0,
        _ => throw new ArgumentException("Invalid SA value")
    };

    public int STBonus => Infos[(int)InfoIdx.ST].Trim() switch
    {
        "P" => 10,
        "O" => -10,
        "X" => 0,
        _ => throw new ArgumentException("Invalid ST value")
    };

    public int ChelemBonus => Infos[(int)InfoIdx.Chelem].Trim() switch
    {
        "GA" => 400,
        "G" => 200,
        "P" => -200,
        "X" => 0,
        _ => throw new ArgumentException("Invalid Chelem value")
    };

    public int ScoreFinal {
        get {
            int tempScore = Math.Abs((int)Math.Ceiling(Score)) + 25;

            if (Score < 0)
                tempScore *= -1;

            tempScore += PTBBonus;

            tempScore *= (int)Prise;

            tempScore += PgBonus;
            tempScore += ChelemBonus;
            tempScore += SABonus;
            tempScore += STBonus;

            return tempScore;
        }
    }
}