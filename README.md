**Viesti sovellus raportti**

**Johdanto**

Sovellus on backend viestintä sovellus, jossa käyttäjä voi luoda käyttäjän ja lähettää, muokata ja poistaa viestejä. Sovelluksessa käytetään Visual studiota, kielenä on C# ja tietokantana on MySQL. Sovellus on tehty pääosin ohjeiden mukaan, ja apuna on käytetty hieman tekoälyä.

**Sovelluksen rakenne**

Sovellus on niin sanottu kolmikerrosrakenteinen. Se koostuu kolmesta eri kerroksesta, Presentation layer, Application layer ja Data layer.
Presentation layer on käyttäjälle näkyvä kerros. Se koostuu esimerkiksi HTML, CSS tai muut vastaavat. Kyseessä on Front end tyyppinen kieli.
Application layer on kerros, jossa kaikki laskutoiminnot ja tietojenkäsittely tapahtuu. Täällä tietoa ohjataan käyttäjän inputin ja databasen välillä.
Data layer on nimensä mukaisesti tietokerros. Tietokerros koostuu tietokannasta ja käyttäjän kaikesta tallentamasta tiedosta. Tietokantana voi olla esimerkiksi, vaikka MySQL.

**Rajapinnat eli API (Application Programin interface)**

Sovelluksessa käytettään rajapintoja eli API (Application Programming Interface). Käyttäjä asettaa tiedot JSON muodossa, käyttäen jotain näistä API komennoista.
POST - lähettää tietoa palvelimelle
GET – hakee tietoa palvelimelta
PUT – muokkaa käyttäjän tietoja
DELETE – poistaa käyttäjän tietoja

**Toiminta käytännössä**

Käyttäjän pitää ensin tehdä käyttäjä POST komennolla. Käyttäjä kirjoittaa JSON muodossa parametreja kuten username, firstname, lastname, password. Salasana salataan API avaimella sekä authentikaattori toiminnolla. Salasana niin sanotusti hassataan. Tämän jälkeen käyttäjä voi lähettää viestejä itselleen tai muille kyseisen tietokannan henkilöille. Käyttäjät ja käyttäjien viestit tallentuvat kaikki MySQL tietokantaan.
Käyttäjä voi myös halutessaan käyttää GET, PUT tai DELETE komentoja. GET komennolla käyttäjä voi hakea muita käyttäjiä tai heidän viestejään tietokannasta. PUT komennolla käyttäjä voi muokata omaa lähettämäänsä viestiä, ja DELETE poistaa tietyn viestin tai käyttäjän.

**Kansiorakenne**

Kansiorakenne helpottaa koodin eri osien lukemista. Selvän kansiorakenteen takia, koodia on helppo lähteä muokkaamaan tarvittaessa. Kun koodi on jaettu selvään kansiorakenteeseen, ja jos koodia lähdetään jakamaan eteenpäin, helpottaa se työnjakoa muille henkilöille. 
Kun koodi rajataan kansioilla pienempiin osiin, tekee se koodista helpommin luettavan, hallittavan testattavan ja vähemmän virhealttiin.


**Esimerkkitapaus: Viestin lähetys:**

Asiakas lähettää JSON-muotoisen viestin osoitteeseen /api/messages.
MessagesController.PostMessage vastaanottaa pyynnön ja kutsuu metodia _messageService.CreateMessageAsync(message).
MessageService muuntaa DTO:n Message-olioksi metodilla DTOToMessageAsync.
Tässä vaiheessa palvelu:
•	kopioi otsikon, sisällön ja Id-arvon DTO:sta
•	hakee lähettäjän ja vastaanottajan User-objektit käyttäjänimien perusteella metodilla IUserRepository.GetUserAsync
•	jos PrevMessage on annettu, lataa edellisen viestin ja linkittää sen uuteen viestiin
Palvelu antaa täytetyn Message-olion repositorylle, joka kutsuu metodia IMessageRepository.CreateMessageAsync. MessageRepository lisää viestin Entity Framework Core -kontekstiin ja suorittaa SaveChangesAsync(). Tällöin Entity Framework tallentaa uuden rivin tietokantaan ja generoi tarvittavat automaattiset arvot.
Tallennettu Message-olio palautetaan palvelulle, joka muuntaa sen takaisin MessageDTO:ksi ja palauttaa kontrollerille.
Kontrolleri vastaa asiakkaalle HTTP 201 Created -statuksella ja palauttaa luodun viestin tiedot.
