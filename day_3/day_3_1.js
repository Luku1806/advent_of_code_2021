const diagnosticInputs = require("./day_3_input.json");

function toTokens(array) {
  return array.map((code) => code.split(""));
}

function inverse(array) {
  return array.map((value) => (value === "0" ? "1" : "0"));
}

function transpose(array) {
  return array[0].map((_, colIndex) => array.map((row) => row[colIndex]));
}

function toMostCommonToken(onTie) {
  return (array) =>
    array.reduce((mostCommon, current, index, source) => {
      const mostCommonCount = source.filter((v) => v === mostCommon).length;
      const currentCount = source.filter((v) => v === current).length;

      return mostCommon === current
        ? mostCommon
        : mostCommonCount === currentCount
        ? onTie ?? mostCommon
        : mostCommonCount >= currentCount
        ? mostCommon
        : current;
    }, null);
}

function powerConsumption(input) {
  const mostCommonTokens = transpose(toTokens(input)).map(
    toMostCommonToken("1")
  );
  const leastCommonTokens = inverse(mostCommonTokens);

  const gammaRate = parseInt(mostCommonTokens.join(""), 2);
  const epsilonRate = parseInt(leastCommonTokens.join(""), 2);

  return gammaRate * epsilonRate;
}

console.log(powerConsumption(diagnosticInputs));

module.exports = {
  toTokens,
  inverse,
  transpose,
  toMostCommonToken,
};
