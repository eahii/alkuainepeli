# alkuainepeli

To whomever is actually going to read this: This is my first AI-assisted coding project and maybe someday i'll translate the Finnish to English.
Here's my changelog(which is also created with AI).
Created by using: Claude.ai 3.5 and chatgpt (free versions) 

Muutosloki
Koodin Parannukset ja Muutokset

1.1. Koodin rakenne ja optimointi

Alkuaineiden tiedot:
Korvattu tavallinen File.ReadAllLines -menetelmä LueAlkuaineTiedot -metodilla, joka lukee tiedot Dictionary-rakenteeseen ja tarkistaa tiedoston rivimäärän parillisuuden.
Kysymysten valinta:
Muutettu kysymysten valinta kattamaan kaikki alkuaineet tiedostosta satunnaisesti.
Virheenkäsittely:
Lisätty tarkistus tiedoston rivimäärän parillisuudelle ja virheilmoituksia virheellisistä tiedoista.
1.2. Pelaamisen logiikka

Satunnaistaminen:
Käytetty edelleen OrderBy(x => Guid.NewGuid()) satunnaistamaan kysymyksiä.
Pelaamisen loppu:
Lisätty tulosten tulostaminen ja oikeiden/väärien vastausten listaaminen.
1.3. Tulosten tallennus ja tarkastelu

Tulosten tallennus:
Korjattu tulosten tallennus niin, että tulokset kirjoitetaan JSON-muodossa ja hakemistorakenne luodaan tarvittaessa.
Lisätty Formatting.Indented JSON-tiedostoon selkeyttämään tallennusta.
Tulosten tarkastelu:
Lisätty tulosten keskiarvon, parhaiden ja huonoimpien tulosten laskenta.
Muutettu tulosten tarkastelua kattamaan kaikki olemassa olevat tulokset hakemistoissa.
Pelaamisen ja tulosten näyttäminen

Oikeat vastaukset:
Näytetään oikeat vastaukset oikeiden lyhenteiden kanssa sekä oikein menneissä että väärissä vastauksissa.
Virheenkäsittely ja null-arvot

Null-arvot:
Korjattu virheelliset null-viittaukset koodissa, erityisesti oikeiden ja väärien vastausten käsittelyssä.
Koodin esimerkkimuutos

PelaaPelia ja TarkasteleTuloksia:
Näytetään myös oikeat lyhenteet väärissä vastauksissa.
Oikeiden vastauksien listaamiseen käytetään (string alkuaine, string oikeaLyhenne) -rakennetta.
Yhteenveto muutoksista:
Pelaamisen kattavuuden laajentaminen: Kysymykset voivat nyt koskea kaikkia alkuaineita.
Koodin virheenkäsittely: Tarkistettu tiedoston rivimäärä ja lisätty virheilmoituksia.
Tulosten käsittely: Tallennetaan ja tarkastellaan tuloksia JSON-muodossa, lisätty tulosten keskiarvo, paras ja huonoin tulos.
Koodin optimointi ja selkeys: Pienempiä optimointeja ja parannuksia koodin selkeyteen ja ylläpidettävyyteen.
Oikeat vastaukset näkyvät myös oikein menneissä vastauksissa: Näytetään oikeat lyhenteet sekä oikein menneissä että väärissä vastauksissa.
