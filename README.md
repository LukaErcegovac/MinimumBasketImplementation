# MinimumBasketImplementation

Mikroservisi:
- User mikroservis: Port: 7282
- Order mikroservis: Port: 7290
- Basket mikroservis: Port: 7075

Najbitnije je da port na order mikroservisu bude 7290 jer je tako postavljeno u kodu.
Basket mikroservis se spaja preko tog porta kako bi kreirao narudžbu nakon što se ona potvrdi.

## Pokretanje

Progmar se može pokrenuti na dva načina. 
Prvi način je onaj koji sam i ja koristio kako bi pokretao projekt.

### 1. Način
Desni klik na Solution i zatim se izabere Properties. Kada se otvori prozor Propertie u Common Properties se izabere Configure Startup Project.
U Configure Startup Project se mora izabrati opcija Multiple startup project i u drop down meniju u stupcu Action se postavi Start, te se pritisne Primjeni i nakon toga U redu.
Kada su odrađeni svi koraci projekt se može pokrenuti preko gumba Start.

Ovaj način je za mene bio bolja opcija jer mi je odmah otvarao Swagger za svaki mikroservis.

### 2. Način
Drugi način da se pokrene projekt je da se otvori više CMD-ova ili PowerShell-ova i u svako se uđe u file jednog projekta, te se pokrene svaku pojedinačno.

cd .\UserMicroservice\
dotnet run

cd .\OrderMicroservice\
dotnet run

 cd .\MinimumBasketImplementation\
 dotnet run
