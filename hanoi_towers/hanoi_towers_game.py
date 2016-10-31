class ArgumentException(Exception):
    pass

class InvalidMoveException(Exception):
    pass

class DeadEndException(Exception):
    pass

class EndStateException(Exception):
    pass

import copy
import sys
from random import randint
class HanoiTowersGame:

    stateStack = []
    exploredStates = []

    def currentState(self):
        return self.stateStack[len(self.stateStack)-1]


    def __init__(self, noOfDisks, noOfStacks, allowLoops = False):

        if(noOfStacks < 3):
            raise ArgumentException("At least 3 stacks are required")

        self.noOfDisks = noOfDisks
        self.noOfStacks = noOfStacks

        initialState = []
        for i in range(0,noOfDisks):
            initialState.append(0)

        self.stateStack.append(initialState)
        self.exploredStates.append(initialState)
        self.allowLoops = allowLoops

    

    def undo(self):
        if len(self.stateStack) > 1 :
            self.stateStack.pop()
        return self

    def transition(self, disk, stack): # take disk'th disk and move it to stack'th stack
        
        if(not(disk < self.noOfDisks and disk >= 0)):
            raise ArgumentException("Disk out of range")

        if(not(stack < self.noOfStacks and stack >= 0)):
            raise ArgumentException("Stack out of range")


        #validation: 
        for i in range(0, disk):
            if self.currentState()[i] == stack or self.currentState()[i] == self.currentState()[disk]:
                raise InvalidMoveException("This move leads to an invalid state")
            # if self.currentState()[i] == self.currentState()[disk]:
            #     raise InvalidMoveException("This move leads to an invalid state")
            #     pass

        # if self.currentState()[disk] == self.noOfDisks-1 :
        # # if the disk we atempt to move is on the last stack, 
        # # first check that it's not already in it's final position 
        # # (i.e:there aren't any disks larger than itself left on the other stacks)
        #     i = disk + 1
        #     while i < len(self.currentState()):
        #         if not self.currentState()[i] == self.noOfDisks-1 :
        #             raise InvalidMoveException("Attempting to move a piece that is already in it's correct position")
        #         i += 1


        newState = copy.copy(self.currentState())
        newState[disk] = stack;

        if not self.allowLoops :
            for i in self.exploredStates:
                if i == newState:
                    raise InvalidMoveException("This move leads to a state that has already been visited")

        self.stateStack.append(newState)
        self.exploredStates.append(newState)

        return self


    def checkEndState(self):
        for i in self.currentState() :
            if i != (self.noOfStacks - 1) :
                return False;
        return True;

class HTRandomSolver:
    def __init__(self, game, maxIterations, maxAttemptsPerIteration):
        self._game = copy.copy(game)
        self.maxIterations = maxIterations
        self.maxAttemptsPerIteration = maxAttemptsPerIteration

    def _makeRandomMove(self):

        moveFound = False

        for i in range(0, self.maxAttemptsPerIteration):


            randomDisk = randint(0,self._game.noOfDisks-1)
            randomStack = randint(0,self._game.noOfStacks-1)

            try:
                self._game.transition(randomDisk, randomStack)
            except InvalidMoveException as exception:
                pass
            else:
                moveFound = True
                break

        if not moveFound:
            raise DeadEndException('A dead end has been encountered')

    def solve(self):
        while len(self._game.exploredStates) <= self.maxIterations and not self._game.checkEndState():
            try:
                self._makeRandomMove()
            except DeadEndException as exception:
                self._game.undo()

        print(len(self._game.stateStack))
        print(self._game.checkEndState())
        print(len(self._game.exploredStates))

class HTHillClimberSolver:
    def __init__(self, game, maxIterations, maxAttemptsPerIteration):
        self._game = copy.copy(game)
        self.maxIterations = maxIterations
        self.maxAttemptsPerIteration = maxAttemptsPerIteration

    def _makeRandomMove(self):

        moveFound = False

        for i in range(0, self.maxAttemptsPerIteration):


            randomDisk = randint(0,self._game.noOfDisks-1)
            randomStack = randint(0,self._game.noOfStacks-1)

            try:
                self._game.transition(randomDisk, randomStack)
            except InvalidMoveException as exception:
                pass
            else:
                moveFound = True
                break

        if not moveFound:
            raise DeadEndException('A dead end has been encountered')

    #'stick and carrot' fitness function
    #apply a penalty for each disk blocking another disk's path
    #and apply a reward for each disk that makes it's way to the goal stack
    def _fitness(self) :
        state = self._game.currentState()
        score = 0
        for disk, stack in reversed(list(enumerate(state))):

            if stack == self._game.noOfStacks-1:
            #if this disk is at the goal stack
            #apply it's weight as a reward
                score += disk + 1
            else:
                smallerDisk = disk - 1
                while smallerDisk >= 0 :
                #for each smaller disk:

                    smallerDisksStack = state[smallerDisk]

                    if smallerDisksStack == stack or smallerDisksStack == self._game.noOfStacks-1 :
                    #if the smaller disk is blocking the larger one's way to the goal
                    #apply the disk's weight as a penalty
                        score -= disk + 1

                    smallerDisk -= 1
        return score;

    def solve(self):
        #initialise the fitness score to something very (very) small
        maxFitness = -sys.maxsize

        loops = 0
        while loops <= self.maxIterations and not self._game.checkEndState():
            try:
                self._makeRandomMove()
            except DeadEndException as exception:
                self._game.undo()
                maxFitness = self._fitness()
            else:
                currentFitness = self._fitness()
                print(currentFitness, maxFitness)
                if currentFitness < maxFitness :
                    self._game.undo()
                else:
                    maxFitness = currentFitness

            loops += 1
            # print(len(self._game.exploredStates), maxFitness)


        print(len(self._game.stateStack))
        print(self._game.checkEndState())
        print(len(self._game.exploredStates))

class HTBacktrackingSolver:
    def __init__(self, game):
        self._game = copy.copy(game)

    def _explore(self):

        if self._game.checkEndState():
            raise EndStateException('The end state was reached')

        for disk, stack in list(enumerate(self._game.currentState())):
            for targetStack in range(0, self._game.noOfStacks):
                try:
                    self._game.transition(disk, targetStack)
                except InvalidMoveException as exception:
                    pass
                else:
                    try:
                        self._explore()
                    except EndStateException as exception:
                        raise EndStateException('The end state was reached')
                    self._game.undo()


    def solve(self):
        
        try:
            self._explore()
        except EndStateException as exception:
            print (exception)

        print(len(self._game.stateStack))
        print(self._game.checkEndState())
        print(len(self._game.exploredStates))

class HTAStarSolver:

    solution = {'length': sys.maxsize, 'stateStack': []}

    def __init__(self, game):
        self._game = copy.copy(game)
        self._game.allowLoops = True

    def _explore(self, depth = 1):

        if self._game.checkEndState():
            if len(self._game.stateStack) < solution['length']:
                solution['length'] = len(self._game.stateStack)
                solution['stateStack'] = self._game.stateStack

        for disk, stack in list(enumerate(self._game.currentState())):
            for targetStack in range(0, self._game.noOfStacks):
                try:
                    self._game.transition(disk, targetStack)
                except InvalidMoveException as exception:
                    pass
                else:
                    self._explore(depth + 1)
                    self._game.undo()


    def solve(self):
        
        try:
            self._explore()
        except EndStateException as exception:
            print (exception)

        print(len(self._game.stateStack))
        print(self._game.checkEndState())
        print(len(self._game.exploredStates))
    
# game = HanoiTowersGame(6,3)
# randomSolver = HTRandomSolver(game, 10000, 100)
# randomSolver.solve()

game = HanoiTowersGame(6,3)
HCSolver = HTHillClimberSolver(game, 10000, 100)
HCSolver.solve()

# game = HanoiTowersGame(6,3)
# bktSolver = HTBacktrackingSolver(game)
# bktSolver.solve()

# game = HanoiTowersGame(1,3)
# aStarSolver = HTAStarSolver(game)
# aStarSolver.solve()



#What the fuck is going on i don't fucking even nu ar trebui sa aiba vreo legatura iteratiile
# for i in range(0,10):
#     game = HanoiTowersGame(6,3)
#     randomSolver = HTRandomSolver(game, 10000, 100)
#     randomSolver.solve()
#     print()
#     print()

