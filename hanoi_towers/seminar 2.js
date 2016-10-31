Turnurile Hanoi
n turnuri, m piese
[Exemplu de (3,3) pe tabla]


STRATEGII:
1. RANDOM
2. BKT
3. HILL CLIMBING
4. A*

STATISTICA:
1. timp rulare
2. parametri
3. medie

//==============================================
Pas 1: Reprezentarea starilor
(i1,i2,...,im) , piese in ordine jos-sus

ex: (1,1,1) -> tije
	 1 2 3  -> discu

	(3,1,1)
	(3,2,1)
	(2,2,1)
	(2,2,3)
	(1,2,3)
	(1,3,3)
	(3,3,3)

//==============================================
Pas 2: Identificarea starilor speciale/reprezentative
(ex: piesa cea mai mare pe ultima tija)

SI: stare initiala		return (1,1,...,1)
SF: stare finala (SC)		suma=n*m
						for i=1;m
							if SC[i] != n
								return false
						return true

//==============================================
Pas 3: Validare tranzitie

	tranzitie (SC,p,c) //SC - stare curenta ; SN - stare noua
		SC[p]=c;
		return SC
	validare(SN,SC,x) //piesa x
		for i=1;x-1
			if SC[i] == SN[x]
				return false
			if SN[i]==SN[x]
				return false
		return true

//==============================================
Pas 4: 
* Strategia RANDOM
==================
R(SC)
	while !stare_finala(SC)
		p = random(1,m)
		c = random(1,m)
		SN = tranzitie (SC,p,c) //piesa p, coloana/tija c
		if validare(SN)
			SC=SN

OPTIMIZARI:
-> memorare stari (eliminare bucle)
"A intraaat in buuucla infiniiiitaa" - Ichim Cosmin
-> piesele mari ajung pe tija finala => nu se mai muta

* Strategia HC
==============
(suma)
(suma ponderata?)
-> tranzitare in scor mai mare:

if validare(SN)					if(validare(SN))
	SC=SN            --->			if(scor(SN) >= scor(SC))
										SC=SN;		

* Strategia BKT
===============
	while !stare_finala(SC)
		for i=1;m
			for j=1;n
				SN = tranzitie(SC,i,j)
					if(valid(SN, SC, i))
						BKT(SN)

* Strategia (A*)
===============
	SF -> SI

----------------------------------------
|		    | RANDOM   HC    BKT    A* |
----------------------------------------
| o sg sol 	| (DA)     NU?   DA     DA |
| toate     |  NU      NU   (DA)    ?  |
| optim     |  NU      NU?   ?    (DA) |
