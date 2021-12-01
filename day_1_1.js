const numbers = require("./day1_1_input.json");

function countIncreases(numbers) {
  const { amount } = numbers.reduce(
    (result, currentNumber) => {
      const { previous, amount } = result;

      return {
        previous: currentNumber,
        amount: currentNumber > previous ? amount + 1 : amount,
      };
    },
    { previous: numbers[0], amount: 0 }
  );

  return amount;
}

console.log(countIncreases(numbers));

module.exports = { countIncreases };
