# Bankomat Uppgift SY25S - Objektorienterad programmering med C#

## Användning
När programmet startas skickas användaren till `MainMenu`, där de kan navigera med följande tangenter:

- `Pil ner` / `J` – Flytta ner
- `Pil upp` / `K` – Flytta upp
- `Enter` – Välj alternativ


## Funktioner

### Startmeny
I startmenyn finns det sju alternativ:

1. **Sätt in pengar**
2. **Ta ut pengar**
3. **Överför pengar** (Använder "Sätt in pengar" + "Ta ut pengar" för att föra över mellan konton)
4. **Visa alla konton**
5. **Skapa konto**
6. **Ta bort konto**
7. **Avsluta programmet**


### Sätt in pengar
Användaren ser en lista över alla konton med deras fullständiga namn och saldo. 
Efter att ha valt ett konto anger användaren hur mycket pengar som ska sättas in. 
En animation spelas upp och överföringen godkänns eller nekas beroende på om summan är giltig.

### Ta ut pengar
Fungerar på samma sätt som "Sätt in pengar", men kontrollerar även att uttaget inte resulterar i ett negativt saldo.

### Överför pengar
En överföring innebär två steg:
1. Summan dras från konto A
2. Summan sätts in på konto B

Överföringen godkänns endast om summan uppfyller villkoren för både insättning och uttag.

### Visa alla konton
Användaren kan välja att visa information om ett antal konton. Följande information hämtas:
- `Id`
- `FirstName`
- `LastName`
- `CreatedAt`
- `BirthDay`
- `Amount`

### Skapa konto
Användaren skapar ett nytt konto genom att fylla i följande information:
- `FirstName`
- `LastName`
- `BirthDay`

### Ta bort konto
Användaren ser en lista över alla konton och kan välja vilket konto som ska tas bort.

### Avsluta programmet
Sparar alla ändringar i `bank.json` och avslutar programmet.
