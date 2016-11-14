#include <iostream>
#include <vector>
#include <cmath>
using namespace std;

class interval{
public:
	double lEnd, rEnd;
	interval(double x, double y){
		lEnd = x;
		rEnd = y;
	}
};

//=============================================================================
//================================ decode =====================================
//=============================================================================

double decodeBitStringGene(vector<bool> subSet, interval Range){

	int b10Value = 0;
	for (int i = 0; i < subSet.size(); i++){

		b10Value = b10Value + int(subSet[i]) * pow(2, (subSet.size() - i - 1));
	}
	double inRangeValue = (b10Value / (pow(2, subSet.size()) - 1)) * (Range.rEnd - Range.lEnd) + Range.lEnd;
	// chromosome: xi(ai,bi) = [b10Valuei/2^Ni - 1]*(bi-ai)+ai | N=nr zecimale pe care se face reprez

	return inRangeValue;
}

//cand genele au aceeasi dimensiune / sunt reprez pe acelasi nr de biti
vector<double> decodeBitStringCH(vector<bool> Chromosome, int geneLength, interval Range){
	vector<double> result;
	vector<bool> subSet;
	int counter = 0;

	for (vector<bool>::iterator i = Chromosome.begin(); i != Chromosome.end(); i++){

		subSet.push_back(*i); // cate un bit din reprezentare
		counter++;

		if (counter == geneLength){

			result.push_back(decodeBitStringGene(subSet, Range));
			subSet.clear();
			counter = 0;
		}
	}
	return result;
}

//gene de dimensiuni diferite: (pentru six-hump-camel-back)
vector<double> decodeBitStringCH(vector<bool> Chromosome, vector<int> geneLength, vector<interval> Ranges){
	vector<double> result;

	vector<bool> subSet; //aici o sa fie o singura gena mereu
	int bitCounter = 0, geneCounter = 0;
	for (vector<bool>::iterator i = Chromosome.begin(); i != Chromosome.end(); i++){

		subSet.push_back(*i); //bit cu bit
		bitCounter++;

		if (bitCounter == geneLength[geneCounter]){ //o gena
			double decodedGene = decodeBitStringGene(subSet, Ranges[geneCounter]);
			result.push_back(decodedGene);

			subSet.clear();
			bitCounter = 0;
			geneCounter++;
		}
	}
	return result;
}

/*============================================================================*/

double Sphere(vector<double> x)
{
	double sum = 0;
	for (vector<double>::iterator i = x.begin(); i != x.end(); i++)
		sum = sum + pow(*i, 2);

	return sum;
}

double Rastrigin(vector<double> x)
{
	double img = 10 * x.size();
	for (vector<double>::iterator i = x.begin(); i != x.end(); i++)
		img = img + pow(*i, 2) - 10 * cos(2 * 3.14*(*i));
	return img;
}

double Michalewicz(vector<double> x)
{
	double img = 0;
	double count = 1;
	for (vector<double>::iterator i = x.begin(); i != x.end(); i++)
	{
		double aux = count*pow(*i, 2);
		aux = aux / 3.14;
		img = img + sin(*i)*pow(sin(aux), 2 * 10);
		count++;
	}
	return -img;
}

double sixHumpCamelBack(vector<double> x)
{
	double aux = 4 - 2.1*pow(x[0], 2) + (pow(x[0], 4)) / 3;
	aux = aux*pow(x[0], 2);
	aux = aux + x[0] * x[1];
	aux = aux + ((-4) + 4 * pow(x[1], 2))*pow(x[1], 2);
	return aux;
}







double fitSphere(vector <bool> x, int paramNumber,interval range, unsigned int precision = 2)
{	
	unsigned int N = ceil(log2((range.rEnd - range.lEnd)*pow(10, precision))); //nr biti pt reprez nr
	return -1 * Sphere(decodeBitStringCH(x,N,range));
}

double fitRastrigin(vector <bool> x, int paramNumber, interval range, unsigned int precision = 2)
{
	unsigned int N = ceil(log2((range.rEnd - range.lEnd)*pow(10, precision))); //nr biti pt reprez nr
	return -1 * Rastrigin(decodeBitStringCH(x, N, range));
}

double fitMichalewicz(vector <bool> x, int paramNumber, interval range, unsigned int precision = 2)
{ 
	unsigned int N = ceil(log2((range.rEnd - range.lEnd)*pow(10, precision))); //nr biti pt reprez nr
	return -1 * Michalewicz(decodeBitStringCH(x, N, range));
}

double fitSixHumpCamelBack(vector <bool> x, int paramNumber, vector<interval> range, unsigned int precision = 2)
{
	vector<int> N;
	for (unsigned int i = 0; i < paramNumber; i++){
		N.push_back(log((range[i].rEnd - range[i].lEnd)*pow(10, precision)));
	}
	return -1 * sixHumpCamelBack(decodeBitStringCH(x, N, range));
}









double Sphere(vector <bool> x, int paramNumber, interval range, unsigned int precision = 2)
{
	unsigned int N = ceil(log2((range.rEnd - range.lEnd)*pow(10, precision))); //nr biti pt reprez nr
	return Sphere(decodeBitStringCH(x, N, range));
}

double Rastrigin(vector <bool> x, int paramNumber, interval range, unsigned int precision = 2)
{
	unsigned int N = ceil(log2((range.rEnd - range.lEnd)*pow(10, precision))); //nr biti pt reprez nr
	return Rastrigin(decodeBitStringCH(x, N, range));
}

double Michalewicz(vector <bool> x, int paramNumber, interval range, unsigned int precision = 2)
{
	unsigned int N = ceil(log2((range.rEnd - range.lEnd)*pow(10, precision))); //nr biti pt reprez nr
	return Michalewicz(decodeBitStringCH(x, N, range));
}

double SixHumpCamelBack(vector <bool> x, int paramNumber, vector<interval> range, unsigned int precision = 2)
{
	vector<int> N;
	for (unsigned int i = 0; i < paramNumber; i++){
		N.push_back(log((range[i].rEnd - range[i].lEnd)*pow(10, precision)));
	}
	return sixHumpCamelBack(decodeBitStringCH(x, N, range));
}
