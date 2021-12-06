const numbers = require("./numbers.json");
const boards = require("./boards.json");

function transpose(array) {
  return array[0].map((_, colIndex) => array.map((row) => row[colIndex]));
}

function hasWinningRow(board, numbers) {
  return board.some((row) => row.every((cell) => numbers.includes(cell)));
}

function isWinner(board, numbers) {
  return (
    hasWinningRow(board, numbers) || hasWinningRow(transpose(board), numbers)
  );
}

function play(boards, numbers, drawnNumbers = []) {
  const winner = boards.find((board) => isWinner(board, drawnNumbers));

  if (winner) {
    return { winner, drawnNumbers };
  }

  return play(boards, numbers, numbers.slice(0, drawnNumbers.length + 1));
}

function score(board, drawnNumbers) {
  return (
    board
      .flatMap((row) => row.filter((number) => !drawnNumbers.includes(number)))
      .reduce((acc, number) => acc + number, 0) *
    drawnNumbers[drawnNumbers.length - 1]
  );
}

module.exports = {
  isWinner,
  score,
};

const { winner, drawnNumbers } = play(boards, numbers);
const endScore = score(winner, drawnNumbers);

console.log(endScore);
