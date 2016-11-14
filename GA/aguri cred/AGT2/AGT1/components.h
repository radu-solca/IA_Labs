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

vector <double> evalPop(vector<vector<bool>> pop, double(*fitness)(vector<bool>, int paramNumber, interval range, unsigned int precision), int paramNumber, interval range, unsigned int precision){

	vector<double> fit;
	int offset=0;

	fit.resize(pop.size(), -1);
	for (int i = 0; i < pop.size(); i++){
		fit[i] = fitness(pop[i], paramNumber, range, precision);
		if (fit[i]<0 && abs(fit[i])>offset){
			offset = abs(fit[i]);
		}
	}

	for (int i = 0; i < fit.size(); i++)
	{
		fit[i] = fit[i] + offset;
	}

	return fit;
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

void applySelect(vector<vector<bool>> &pop, vector<double> fit){

	vector <double> popProb=getPopProb(fit);
	vector <double> summedPopProb = getSummedPopProb(popProb);

	pop = compete(pop, summedPopProb);
}

//=============================================================================
//=============================== crossover ===================================
//=============================================================================

void crossover(vector <bool> &ch1, vector <bool> &ch2)
{
	unsigned int choice = round(((double)rand() / RAND_MAX)*(ch1.size() - 2))+1;

	vector<bool> aux1=ch1, aux2=ch2;
	aux1.erase(aux1.begin(), aux1.begin() + choice+1);
	aux1.insert(aux1.begin(), ch2.begin(), ch2.begin() + choice + 1);

	aux2.erase(aux2.begin(), aux2.begin() + choice + 1);
	aux2.insert(aux2.begin(), ch1.begin(), ch1.begin() + choice + 1);

	ch1 = aux1;
	ch2 = aux2;

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

	random = round((double)rand() / RAND_MAX); //ori adaug ch ori scot unu

	if (candidates.size() % 2 != 0) 
	{
		if (random == 0)
		{
			candidates.erase(candidates.end() - 1);
		}
		else
		{
			unsigned int choice = round(((double)rand() / RAND_MAX)*(pop.size()-2));
			candidates.push_back(choice);
		}
	}

	for (int i = 0; i < candidates.size(); i = i + 2){

		/*
		cout << "\n===ITERATIA: " << i << endl;;
		cout << "\ncandidat 1: ";
		printCH(pop[candidates[i]]);
		cout << endl;
		cout << "\ncandidat 2: ";
		printCH(pop[candidates[i + 1]]);
		cout << endl;

		crossover(pop[candidates[i]], pop[candidates[i+1]]);
		cout << "------------";

		cout << "\ndupacros 1: ";
		printCH(pop[candidates[i]]);
		cout << endl;
		cout << "\ndupacros 2: ";
		printCH(pop[candidates[i + 1]]);
		cout << endl;
		*/

		crossover(pop[candidates[i]], pop[candidates[i + 1]]);
	}

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

