#include <iostream>
#include <vector>
#include <cmath>
using namespace std;

double Sphere(vector<double> randoms)
{
	double sum = 0;
	for (vector<double>::iterator i = randoms.begin(); i != randoms.end(); i++)
		sum = sum + pow(*i, 2);

	return sum;
}

double Rastrigin(vector<double> randoms)
{
	double img = 10 * randoms.size();
	for (vector<double>::iterator i = randoms.begin(); i != randoms.end(); i++)
		img = img + pow(*i, 2) - 10 * cos(2 * 3.14*(*i));
	return img;
}

double Michalewicz(vector<double> randoms)
{
	double img = 0;
	double count = 1;
	for (vector<double>::iterator i = randoms.begin(); i != randoms.end(); i++)
	{
		double aux = count*pow(*i, 2);
		aux = aux / 3.14;
		img = img + sin(*i)*pow(sin(aux), 2 * 10);
		count++;
	}
	return -img;
}

double sixHumpCamelBack(vector<double> randoms)
{
	double aux = 4 - 2.1*pow(randoms[0], 2) + (pow(randoms[0], 4)) / 3;
	aux = aux*pow(randoms[0], 2);
	aux = aux + randoms[0] * randoms[1];
	aux = aux + ((-4) + 4 * pow(randoms[1], 2))*pow(randoms[1], 2);
	return aux;
}

double tPrim(vector<double> x){
	return pow(x[0], 3) - 60 * pow(x[0], 2) + 900 * x[0] + 100;
}

