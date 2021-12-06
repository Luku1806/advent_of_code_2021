const numbers = require("./numbers.json");
const boards = require("./boards.json");

const { isWinner, score } = require("./day_4_1");

function findSolution(board, numbers, drawnNumbers = []) {
  if (isWinner(board, drawnNumbers)) {
    return { board, drawnNumbers, drawnNumbersCount: drawnNumbers.length };
  }

  return findSolution(
    board,
    numbers,
    numbers.slice(0, drawnNumbers.length + 1)
  );
}

function byDrawnNumbersCount(solution1, solution2) {
  return solution2.drawnNumbersCount - solution1.drawnNumbersCount;
}

function solutions(boards, numbers) {
  return boards
    .reduce(
      (solvedBoards, board) => [...solvedBoards, findSolution(board, numbers)],
      []
    )
    .sort(byDrawnNumbersCount);
}

const solutionsResult = solutions(boards, numbers);
const { board, drawnNumbers } = solutionsResult[0];
const endScore = score(board, drawnNumbers);

console.log(endScore);
