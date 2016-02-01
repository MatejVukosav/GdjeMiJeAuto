Uvod

Aplikacija “Gdje mi je auto?!” razvijena je u sklopu projekta na predmetu “ Programske paradigme I jezici” akademske godine 2014. / 2015. Aplikacija na jednostavan način olakšava korisniku aktivnosti kao što su pamćenje lokacije automobila te navigiranje do njega te plaćanje parkinga putem sms poruke ili unosom podataka iz automata. Aplikacija periodički zapisuje lokacije korisnika, pomoću kojih pokušava odrediti gdje je parkirao. Korisnik na karti može vidjeti moguće lokacije na kojima je parkirao generirane posebnim algoritmom te odabrati jednu od  ponudenih lokacija.Aplikacija također nudi prikaz najkraćeg puta do automobila. Aplikacija također olakšava plaćanje parkinga na način da pamti registarske oznake I automatski određuje u kojoj zoni se nalazi na temelju trenutne korisnikove lokacije. Dodatne funkcionalnosti su podsjetnik na istek parkinga koji korisnika upozorava na vrijeme preostalo do isteka parkirne karte te karta Zagreba sa prikazanim parkirnim zonama.

Izgled i korištenje aplikacije

Početni zaslon
Ovo je početni zaslon naše aplikacije, na njemu korisnik odabire jednu od sljedećih funkcionalnosti aplikacije:

<a href="http://tinypic.com?ref=dqrvbq" target="_blank" height="15px" width="5px"><img src="http://i61.tinypic.com/dqrvbq.jpg" border="0" ></a>

•	Gdje mi je auto – Google karta sa opcijama za pronaženje auta
•	Plaćanje – Plačanje parkinga

•	Postavke – Razne postavke aplikacije koje korisnik može 
                   uređivati po vlastitoj želji,




Gdje mi je auto?

Ovaj dio aplikacije služi da korisnika dovede do njegovog automobila. Uz osnovne funkcionalnositi Google karte korisnik ovdje ima opcije promjene tipa izgleda karte (normalna, satelitska i hibridna), zatim može birati želi li imati prikaz svih parkirnih zona u Zagrebu, i imogućnost izbora dostupnih lokacij na kojima je možda parkirao. Odabirom jedne od lokacija aplikacija će mu nacrtati rutu najlakšeg puta do automobila i vrijeme kada jezadnji put bio na toj lokaciji.

<a href="http://tinypic.com?ref=xpt35w" target="_blank"><img src="http://i60.tinypic.com/xpt35w.png" border="0" alt="Image and video hosting by TinyPic"></a>





Plaćanje

Plaćanje parkinga se može izvesti na dva načina: plačanje iz aplikacije putem sms poruke i preko automata na parkiralištu. Ako je korisnik već platio parking preko automata ili na neki drugi način, može unijeti vrijeme plačanja te zonu u kojoj je platio parking. To će mu omogučiti da prima obavijesti i upozorenja o isteku parkinga. 	

Aplikacija korisniku olakšava plaćanje na način da pamti korisnikovu registracijsku oznaku I automatski određuje zonu na temelju korisnikove lokacije. Korisnik može I ručno odabrati zonu ako se ne slaže sa preporučenom zonom koja je određena automatski. Ako korisnik plati parking iz aplikacije alarm će automatski biti podešen ukoliko u postavkama nije odabrano drugačije. Također ovdje korisnik može vidjeti povijest svih svojih plačanja na dnu ekrana. Pritiskom na bilo koji zapis korisniku se otvara veliki prikaz svih dotadašnjih plaćanja.

<a href="http://tinypic.com?ref=21ovw61" target="_blank"><img src="http://i58.tinypic.com/21ovw61.jpg" border="0" alt="Image and video hosting by TinyPic"></a>

<a href="http://tinypic.com?ref=154zgvc" target="_blank"><img src="http://i59.tinypic.com/154zgvc.jpg" border="0" alt="Image and video hosting by TinyPic"></a>




Postavke

  Korisnik aplikacije ima mnogo mogučnosti podešavanja   aplikacije. Postavke su podjeljene u tri kategorije. 

U kategoriji “Mapa” korisnik može podesiti gustoću pamćenja lokacija u sporom i brzom načinu rada, graničnu brzinu između ta dva načina rada te može kompletno isključiti mogučnost pamćenja lokacija.

<a href="http://tinypic.com?ref=14alf02" target="_blank"><img src="http://i57.tinypic.com/14alf02.jpg" border="0" alt="Image and video hosting by TinyPic"></a>

 U kategoriji plaćanje može se podesiti slika registracije,  odabrati podrazumjevana registracija , te korisnik može učitati poruke iz inboxa da bi u aplikaciji imao zapise o plačanjima parkinga napravljenim prije instalacije aplikacije. Odabirom dijela "Odaberi registraciju" otvara se prozor u kojem se korisniku prikazuje ispis svih registracija s kojima je platio parking u aplikaciji do tada. Kratkim pritiskom na odabranu registracijsku oznaku, oznaka se postavlja kao defaultna i prilikom svakog ulaska u aplikaciji stoji kao ponuđena za plaćanje. Duljim pritiskom na odabranu registraciju korisniku se nudi mogućnost brisanja ponuđene registracijske oznake. Ako je ta oznaka bila označena kao defaultna u tom trenutku, aplikacija briše defaulntu registracijsku oznaku te korisnik mora ponovo odabrati želi li koristiti tu mogućnost s nekom drugom registracijskom oznakom.
 
 <a href="http://tinypic.com?ref=mue7sz" target="_blank"><img src="http://i57.tinypic.com/mue7sz.jpg" border="0" alt="Image and video hosting by TinyPic"></a>


Implementacija

Algoritam traženja automobila
Aplikacija nakon što se prvi put pokrene stvori pozadniski proces koji cijelo vrijeme bilježi u datoteku korisnikovu lokaciju, vrijeme i brzinu (odaljenost prošlog mjerenja podjeljeno sa vremenom koje je prošlo). Aplikacija ima dva načina zapisivanja lokacije: brzi i spori. U sporom načinu lokacija se određuje sa Googlovim mrežnim lociranjem svakih nekoliko minuta (korisnik može podesiti u postavkama), na taj način baterija se ne opterečuje previše. Kada izračunata brzina između dva zapisa bude veća od granične brzine (koju također korisnik odabire) aplikacija prelazi u brzi način, tada se za određivanje lokacije koristi GPS ako je omogučen i lokacija se zapisuje nekoliko puta u minuti (korisnik postavlja koliko). Kada korisnik pritisne tipku “Gdje mi je auto?!” aplikacija učita zapise lokacija, podjeli zapise po brzini na brze i spore, i onda gleda zapise koji su na granici nizova brzih i sporih mjerenja. Ako je korisnik određeni broj minuta večinom bio brži od granične brzine, a nakon toga slijedi niz sporih mjerenja, aplikacija zaključuje da je moguće da je u tom trenutku korisnik parkirao. Sve takve lokacije aplikacija korisniku pokaže na karti, a korisnik može odabrati jednu od njih do koje će mu aplikacija nacrtati put.

Algoritam određivanja trenutne zone
Aplikacija određuje automatski u kojoj zoni se korisnik nalazi da bi mu olakšao plačanje parkiranja. Parkirne zone su aplikaciji pohranjene kao poligoni (isti koji se pokazuju na mapi). Određivanje zone u kojoj se korisnik nalazi svodi se na rješavanja problema pripadnosti točke u poligonu (http://en.wikipedia.org/wiki/Point_in_polygon). Općenito se taj problem rješava na način da se povuće polupravac iz točke, I zatim se broje presjeci tog polupravca stranicama poligona. Ako je broj presjeka paran točka je izvan poligona, ako je neparan točka je u poligonu. Ovako je to implementirano u aplikaciji:
	
	bool c = false;

	foreach (LatLng point in poly_points)

	{

		//lat_i, lon_i te lat_j I lon_j se postave na koordinate dva susjedna vrha
 
		if ( ((lat_i  >  lat != (lat_j > lat)) &&

	 	     (lon < (lon_j - lon_i) * (lat - lat_i) / (lat_j - lat_i) + lon_i) ) )

				c = !c;
		
	}

	return c;

Autori:
Matej Vukosav
Filip Sakač
Antonio Brezjan






