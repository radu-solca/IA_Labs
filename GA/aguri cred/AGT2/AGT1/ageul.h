#include "stdafx.h"
#include <iostream>
#include <cstdlib>
#include <ctime>
#include <vector>
#include "components.h"
#include <iomanip>

int maxFit(vector <double> fit)
{
	int max = 0;
	for (int i = 0; i < fit.size(); i++){
		if (fit[i]>fit[max])
			max = i;
	}
	return max;
}

void GA(int PopNo, double(*fitness)(vector<bool>, int paramNumber, interval range, unsigned int precision), double probCross, double probMut, int paramNumber, interval range, unsigned int precision){

	unsigned int N = ceil(log2((range.rEnd - range.lEnd)*pow(10, precision))); //nr biti pt reprez nr
	int CHLength = N*paramNumber;

	vector<vector<bool>> pop = initPop(CHLength, PopNo);

	//cout << "\nORIGINAL POP\n";
	//cout << endl;
	//printPop(pop);
	//cout << endl; 

	vector <double> fit = evalPop(pop, fitness, paramNumber, range, precision);
	int count = 0;

	//cout << "\nafter fit POP\n";
	//cout << endl;
	//printPop(pop);
	//cout << endl;

	while (count<=10){
		count++;
		cout << "Generatia nr: " << count<<endl;
		//cout << "\tFct fitness: "<<fit[maxFit(fit)]<<endl;

		//cout << "\tCromozomul/Individul: ";
		//printCH(pop[maxFit(fit)]);
		//cout << endl;

		cout << "\t===Imag: ";
		cout << Sphere(pop[maxFit(fit)], paramNumber, range, precision);
		cout << endl;

		applySelect(pop,fit);

		//cout << "\napplied SELECT POP\n";
		//cout << endl;
		//printPop(pop);
		//cout << endl;

		//cout << endl;
		//printPop(pop);
		//cout << endl;

		applyCrossover(pop, probCross);

		//cout << "\napplied crossover POP\n";
		//cout << endl;
		//printPop(pop);
		//cout << endl;

		applyMutate(pop, probMut);

		//cout << "\napplied mutate POP\n";
		//cout << endl;
		//printPop(pop);
		//cout << endl;

		fit = evalPop(pop, fitness, paramNumber, range, precision);

		//cout << "\nre evaluated POP\n";
		//cout << endl;
		//printPop(pop);
		//cout << endl;
	}	
}



