#include "stdafx.h"
#include <iostream>
#include <cstdlib>
#include <ctime>
#include <vector>
#include "components.h"
#include <iomanip>

void GA(int CHLength, int PopNo, double(*fitness)(vector<bool>),double probCross,double probMut){

	vector<vector<bool>> pop = initPop(CHLength, PopNo);
	vector <double> fit = evalPop(pop, fitness);

	//printPop(pop);
	//printFit(fit);
	while (1){
		cout << "sunt la select...\n";
		pop = applySelect(pop, fit);
		cout << "sunt la crossover...\n";
		applyCrossover(pop, probCross);
		cout << "sunt la mutate...\n";
		applyMutate(pop, probMut);
		cout << "sunt la fitness...\n";
		fit = evalPop(pop, fitness);
		cout << "fitness over...\n";
		
		//printPop(pop);
		//printFit(fit);

	}
	
}



