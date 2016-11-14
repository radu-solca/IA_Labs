#include "stdafx.h"
#include <iostream>
#include <cstdlib>
#include <ctime>
#include <vector>
#include <iomanip>
#include <cmath>
using namespace std;

//=============================================================================
//================================= init ======================================
//=============================================================================
void printCH(vector<bool> ch)
{
	for (int i = 0; i < ch.size(); i++){
		cout << ch[i] << " ";
	}
}

void printPop(vector<vector<bool>> pop)
{
	for (int i = 0; i < pop.size(); i++)
	{
		for (int j = 0; j < pop[i].size(); j++)
			cout << pop[i][j] << " ";
	
		cout << endl;
	}
}

vector<bool> getBitStringCH(int CHLength){

	vector<bool> result;
	unsigned int aux;

	for (int i = 0; i < CHLength; i++)
	{
		aux = round((double)rand() / RAND_MAX);
		result.push_back(aux);
	}
	return result;
}

vector<vector<bool>> initPop(int CHLength, int PopNo){
	vector<vector<bool>> pop;

	for (int i = 0; i < PopNo; i++)
	{
		pop.push_back(getBitStringCH(CHLength));
	}

	return pop;


}

//=============================================================================
//================================= eval ======================================
//=============================================================================

void printFit(vector<double> fit)
{
	for (int i = 0; i < fit.size(); i++){
		cout << fit[i] << " ";
		cout << endl;
	}
}

vector <double> evalPop(vector<vector<bool>> pop, double (*fitness)(vector<bool>)){

	vector<double> popImg;

	popImg.resize(pop.size(), -1);
	for (int i = 0; i < pop.size(); i++)
		popImg[i] = fitness(pop[i]);

	return popImg;
} //val fit

//=============================================================================
//=============================== select ======================================
//=============================================================================

void printPopProb(vector<double> popProb)
{
	for (int i = 0; i < popProb.size(); i++){
		cout << popProb[i] << " ";
		cout << endl;
	}
}

void printCompete(vector<vector<bool>> pop)
{
	for (int i = 0; i < pop.size(); i++)
	{
		for (int j = 0; j < pop[i].size(); j++)
			cout << pop[i][j] << " ";

		cout << endl;
	}
}

vector<double> getPopProb(vector <double> fit){

	int fitSum = 0;
	for (int i = 0; i < fit.size(); i++)
		fitSum = fitSum + fit[i];

	vector <double> popProb;
	popProb.resize(fit.size(), -1);

	for (int i = 0; i < popProb.size(); i++)
		popProb[i] = (double)fit[i] / fitSum;

	return popProb;
}

vector<double> getSummedPopProb(vector <double> popProb){
	
	vector <double> summedPopProb;
	summedPopProb.resize(popProb.size(),-1);
	double prevSum = 0;

	for (int i = 0; i < popProb.size(); i++)
	{	
		summedPopProb[i] = popProb[i] + prevSum;
		prevSum = summedPopProb[i];
	}
	return summedPopProb;
}

vector<vector<bool>> compete(vector<vector<bool>> pop, vector <double> summedPopProb){

	vector<vector<bool>> winners;
	winners.resize(pop.size());

	for (int i = 0; i < winners.size(); i++)
	{
		double random = (double)rand() / RAND_MAX;

		for (int j = 0; j < summedPopProb.size(); j++) //pop
		{
			if (random < summedPopProb[j])
			{
				winners[i] = pop[j];
				break;
			}
		}
	}
	return winners;
}

vector<vector<bool>> applySelect(vector<vector<bool>> pop, vector<double> fit){

	vector <double> popProb=getPopProb(fit);
	vector <double> summedPopProb = getSummedPopProb(popProb);

	return compete(pop, summedPopProb);
}

//=============================================================================
//=============================== crossover ===================================
//=============================================================================

void crossover(vector <bool> &ch1, vector <bool> &ch2)
{
	unsigned int choice = round(((double)rand() / RAND_MAX)*(ch1.size() - 2))+1;
	cout << "\n*** split point: " << choice << " ***\n";
	cout << "BEFORE:first ch...\n";
	printCH(ch1);
	cout << endl << "BEFORE:second ch...\n";
	printCH(ch2);

	vector<bool> aux1=ch1, aux2=ch2;
	aux1.erase(aux1.begin(), aux1.begin() + choice+1);
	aux1.insert(aux1.begin(), ch2.begin(), ch2.begin() + choice + 1);

	cout << "\n\tformed 1st \"child\"...\n";

	aux2.erase(aux2.begin(), aux2.begin() + choice + 1);
	aux2.insert(aux2.begin(), ch1.begin(), ch1.begin() + choice + 1);

	cout << "\n\tformed 2nd \"child\"...\n";

	ch1 = aux1;
	ch2 = aux2;

	cout << "AFTER:first ch...\n";
	printCH(ch1);
	cout <<endl<< "\nAFTER:second ch...\n";
	printCH(ch2);

	cout << "\nsaved \"children\" to original variables...\n";
	cout << "\n========================================\n";

}

void applyCrossover(vector<vector<bool>> &pop, double prob){ //0.7 standard - 70% pop - participa la crossover

	vector<int> candidates;
	double random;

	for (int i = 0; i < pop.size(); i++)
	{
		random = (double)rand() / RAND_MAX;
		if (random < prob)
			candidates.push_back(i);
	}

	cout << "finished adding candidates...\n";

	random = round((double)rand() / RAND_MAX); //ori adaug ch ori scot unu

	if (candidates.size() % 2 != 0) 
	{
		cout << "\nP O P   I M P A R A !\n";
		if (random == 0)
		{
			cout << "\nE R A S E !\n";
			candidates.erase(candidates.end() - 1);
		}
		else
		{
			cout << "\nA D D !\n";
			unsigned int choice = round(((double)rand() / RAND_MAX)*(pop.size()-1));
			cout << "\t\t!!!"<<choice << endl;
			printCH(pop[choice]); cout << endl;
			candidates.push_back(choice);
			cout << "~~~~~~~~~~~~~~~~ AJUNGE AICI??????: ";
		}
	}

	cout << "solved even number of CH...\n";
	cout << "dim cand: " << candidates.size() << endl;

	for (int i = 0; i < candidates.size(); i = i + 2){
		cout << "SUNT IN APPLY CROSSOVER!!! - "<<i<<endl;
		printCH(pop[candidates[i]]);
		cout << endl;
		printCH(pop[candidates[i+1]]);
		cout << endl;
		crossover(pop[candidates[i]], pop[candidates[i+1]]);
	}

	cout << "finished executing basic crossover...\n";
}



//=============================================================================
//================================ mutate =====================================
//=============================================================================

void mutate(vector <bool> &ch, int position)
{
	ch[position] = !ch[position];
}

void applyMutate(vector<vector<bool>>&pop, double prob){
	prob = (double)prob / pop[1].size();

	for (int i = 0; i < pop.size(); i++) //pan la nr de ch
	{
		for (int j = 0; j < pop[i].size(); j++) //parcurgere CH
		{
			double random = (double)rand() / RAND_MAX;
			if (random < prob)
			{
				mutate(pop[i], j);
			}
		}
	}

	

}