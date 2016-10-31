def sumaPonderata(state) :
	return 1*state[0] + 1*state[1] + 9*state[2]

def stickAndCarrot(state) :
	score = 0
	for disk, stack in reversed(list(enumerate(state))):

		if stack == 3:
		#if this disk is at the goal stack
		#apply it's weight as a reward
			score += disk + 1
		else:
			smallerDisk = disk - 1
			while smallerDisk >= 0 :
			#for each smaller disk:

				smallerDisksStack = state[smallerDisk]

				if smallerDisksStack == stack or smallerDisksStack == 3 :
				#if the smaller disk is blocking the larger one's way to the goal
				#apply the disk's weight as a penalty
					score -= disk + 1

				smallerDisk -= 1
				
	return score;


optimalStates = [[1,1,1],[3,1,1],[3,2,1],[2,2,1],[2,2,3],[1,2,3],[1,3,3],[3,3,3]]

print (list(map(stickAndCarrot,optimalStates)))