const { countIncreases } = require("./day_1_1");
const numbers = require("./day1_1_input.json");

function windowSum(array, index) {
  return array.slice(index, index + 3).reduce((n1, n2) => n1 + n2);
}

function countWindowIncreases(numbers) {
  return countIncreases(
    numbers.map((_, index, source) => windowSum(source, index))
  );
}

console.log(countWindowIncreases(numbers));
