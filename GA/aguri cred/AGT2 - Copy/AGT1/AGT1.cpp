#include "stdafx.h"
#include <iostream>
#include <vector>
#include <ctime>
#include "testME.h" 
#include <cfloat>
#include "ageul.h"
using namespace std;

double functie(vector<bool> Gene){
	int b10Value = 0;
	for (int i = 0; i < Gene.size(); i++){

		b10Value = b10Value + int(Gene[i]) * pow(2, (Gene.size() - i - 1));
	}
	return b10Value;
}

int main(){
	srand(time(0));
	
	/*
	vector<vector<bool>> pop=initPop(10,10);
	printPop(pop);
	cout << endl;

	vector <double> fit = evalPop(pop, functie);
	printFit(fit);
	cout << endl;

	vector <vector<bool>> select = applySelect(pop, fit);
	printPop(select);

	cout << "\nAPPLIED CROSSOVER.\n";
	applyCrossover(select,0.7);

	cout << "\nAPPLIED MUTATE.\n";
	applyMutate(select, 0.3);

	cout << endl;
	printPop(select);
	*/

	GA(10, 5, functie, 0.7, 0.3);

}