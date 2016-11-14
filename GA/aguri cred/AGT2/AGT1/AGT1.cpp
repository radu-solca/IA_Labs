#include "stdafx.h"
#include <iostream>
#include <vector>
#include <ctime>
#include "testME.h" 
#include <cfloat>
#include "ageul.h"
using namespace std;

int main(){
	srand(time(0));
	
	//GA(int PopNo, double(*fitness)(vector<bool>, int paramNumber, interval range, unsigned int precision), double probCross, double probMut, int paramNumber, interval range, unsigned int precision)
	
	GA(7, fitRastrigin, 0, 0, 30, interval(-5.12,5.12), 2);

}