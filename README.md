# PictureGallery

Ovo je projekt ra�en za vje�tinu "Razvoj aplikacija u programskom jeziku C#" odr�ane u zimskom semestru akademske godine 2017./2018. na Fakultetu elektrtehnike i ra�unarstva u Zagrebu.
Radi se o web aplikaciji koja se upotrebljava kao dru�tvena mre�a za razmjenu slika izme�u korisnika. Korisnici mogu kreirati svoj profil te pregledavati slike drugih korisnika kao i objavljivati
svoje vlastite slike.

This is a project developed for the "Application development in C# programming language" course held in the winter semester of the academic year 2017./2018. at the Faculty of Electrical Engineering and Computing in Zagreb.
It is a web application that is use as a social media for sharing images between users. User can create their own profile and view other user's pictures as well as publish his own pictures.


## Osnovni podatci

URL link do web aplikacije je: https://picturesgallery.azurewebsites.net/
Aplikaciju je izradio Ivan Maltar (ivan.maltar@fer.hr)

## Basic data

URL link to the web application is: https://picturesgallery.azurewebsites.net/
The application was developed by Ivan Maltar (ivan.maltar@fer.hr)

## Zahtjevi za kori�tenje aplikacije

Za uspje�no kori�tenje ove web aplikacije potreban je jedan od novijih preglednika (IE 9, Chrome, Firefox, Edge ...)

## Application Request

To successfully use this web application, one of the newer browsers is needed (IE 9, Chrome, Firefox, Edge ...)

## Kori�tenje aplikacije

### Ulazak u aplikaciju

Po ulasku u aplikaciju pojavljuje se forma za logiranje korisnika. Korisnik se prijavljue svojim Emailom, Facebook ili Google ra�unom. U slu�aju da korisnik nije registriran on to mo�e napraviti
klikom na 'Register' �ime se otvara nova forma u kojoj korisnik mo�e stvoriti svoj novi korisni�ki ra�un.
Ako se korisnik prvi puta logirao na aplikaciju on je du�an stvoriti svoj novi korisni�ki profil upisuju�i svoje korisni�ko ime i stavljanjem slike profila (neobavezno).
Nakon toga prelazi na po�etnu stranicu aplikacije. U sredini stranice se nalaze slike koje objavili korisnici aplikacije (uklju�uju�i i trenutno logiranog korisnika). Prikaz slika koje �e
se prikazati mogu�e je kontrolirati gumbima na lijevoj strani naslovne stranice (ispod slike profina trenutno logiranog korisnika). S desne strane nalazi se lista ostalih korisnika koji su
registrirani te lista korinika koji se prate (Following users).

### Vlastiti profil

Korinik mo�e ure�ivati svoj profil mjenjaju�i svoju sliku profila te dodavanjem i brisanjem albuma. Unutar album korisnik mo�e pregledavati slike u tom albumu, dodavati druge slike ili ih brisati.

### Pregledavanje slika drugih korisnika

Korisnik vidi slike drugih korisnika na naslovnoj stranici aplikacije. Iste mo�e vidjeti i ulaskom na profil korisnika. Tamo mo�e pregledavati njegove albume i slike u njima. Slike mo�e "like-ati"
ili "dislike-ati" te komentirati. Slike se mogu ozna�iti sa "Favorite".

## Kori�tene tehnologije

- [Asp.Net Core](https://docs.microsoft.com/en-us/aspnet/core/) - Web framework
- [Bootstrap](https://getbootstrap.com/) - Dizajn

## Autor

- Ivan Maltar - [MaltarIvan](https://github.com/MaltarIvan)