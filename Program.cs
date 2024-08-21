using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

class AlkuaineTesti
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("Haluatko pelata (p) vai tarkastella tuloksia (t)? (q lopettaa)");
            string valinta = Console.ReadLine().ToLower();

            if (valinta == "p")
            {
                PelaaTestia();
            }
            else if (valinta == "t")
            {
                TarkasteleTuloksia();
            }
            else if (valinta == "q")
            {
                break;
            }
            else
            {
                Console.WriteLine("Virheellinen valinta. Yritä uudelleen.");
            }
        }
    }

    static void PelaaTestia()
    {
        var alkuaineTiedot = LueAlkuaineTiedot("alkuaineet.txt");

        // Satunnaisesti valitut 5 kysymystä kaikista alkuaineista
        List<string> kysyttavatAlkuaineet = alkuaineTiedot.Keys
                                        .OrderBy(x => Guid.NewGuid())
                                        .Take(5)
                                        .ToList();

        int oikeinMenneet = 0;
        List<string> oikeatVastaukset = new List<string>();
        List<string> vaaratVastaukset = new List<string>();

        foreach (string alkuaineNimi in kysyttavatAlkuaineet)
        {
            Console.WriteLine($"Mikä on alkuaineen {alkuaineNimi} kemiallinen merkki?");
            string vastaus = Console.ReadLine().Trim();

            if (alkuaineTiedot.TryGetValue(alkuaineNimi, out string alkuaineLyhenne))
            {
                if (StringComparer.OrdinalIgnoreCase.Compare(vastaus, alkuaineLyhenne) == 0)
                {
                    oikeinMenneet++;
                    oikeatVastaukset.Add(alkuaineNimi);
                }
                else
                {
                    vaaratVastaukset.Add(alkuaineNimi);
                }
            }
            else
            {
                Console.WriteLine($"Alkuainetta {alkuaineNimi} ei löytynyt tiedoista.");
            }
        }

        Console.WriteLine($"Sait {oikeinMenneet} oikein ja {5 - oikeinMenneet} väärin.");

        if (oikeatVastaukset.Count > 0)
        {
            Console.WriteLine("Oikeat vastaukset:");
            foreach (string alkuaine in oikeatVastaukset)
            {
                Console.WriteLine($"- {alkuaine}");
            }
        }

        if (vaaratVastaukset.Count > 0)
        {
            Console.WriteLine("Väärät vastaukset:");
            foreach (string alkuaine in vaaratVastaukset)
            {
                Console.WriteLine($"- {alkuaine}");
            }
        }

        TallennaTulos(oikeinMenneet);
    }

    static Dictionary<string, string> LueAlkuaineTiedot(string tiedostoPolku)
    {
        var alkuaineTiedot = new Dictionary<string, string>();

        // Lukee tiedot Dictionaryyn ja tarkistaa että tiedostossa on oikea parimäärä
        var rivit = File.ReadLines(tiedostoPolku).ToList();

        if (rivit.Count % 2 != 0)
        {
            Console.WriteLine("Virhe: Tiedostossa ei ole parillista määrää rivejä.");
            Environment.Exit(1);
        }

        for (int i = 0; i < rivit.Count; i += 2)
        {
            alkuaineTiedot[rivit[i]] = rivit[i + 1];
        }

        return alkuaineTiedot;
    }

    static void TallennaTulos(int oikeinMenneet)
    {
        string hakemisto = DateTime.Now.ToString("ddMMyyyy");
        string tiedostoPolku = Path.Combine(hakemisto, "tulokset.json");

        Directory.CreateDirectory(hakemisto);

        List<int> tulokset = new List<int>();
        if (File.Exists(tiedostoPolku))
        {
            string json = File.ReadAllText(tiedostoPolku);
            tulokset = JsonConvert.DeserializeObject<List<int>>(json);
        }

        tulokset.Add(oikeinMenneet);

        string uusiJson = JsonConvert.SerializeObject(tulokset, Formatting.Indented);
        File.WriteAllText(tiedostoPolku, uusiJson);
    }

    static void TarkasteleTuloksia()
    {
        List<int> kaikkiTulokset = new List<int>();

        foreach (string hakemisto in Directory.GetDirectories("."))
        {
            string tiedostoPolku = Path.Combine(hakemisto, "tulokset.json");
            if (File.Exists(tiedostoPolku))
            {
                string json = File.ReadAllText(tiedostoPolku);
                List<int> tulokset = JsonConvert.DeserializeObject<List<int>>(json);
                kaikkiTulokset.AddRange(tulokset);
            }
        }

        if (kaikkiTulokset.Count > 0)
        {
            double keskiarvo = kaikkiTulokset.Average();
            Console.WriteLine($"Tulosten keskiarvo: {keskiarvo:F2} / 5 ({keskiarvo / 5 * 100:F2}%)");
            Console.WriteLine($"Paras tulos: {kaikkiTulokset.Max()} / 5");
            Console.WriteLine($"Huonoin tulos: {kaikkiTulokset.Min()} / 5");
        }
        else
        {
            Console.WriteLine("Ei tuloksia saatavilla.");
        }
    }
}
