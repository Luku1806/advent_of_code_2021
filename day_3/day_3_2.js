const diagnosticInputs = require("./day_3_input.json");

const {
  transpose,
  toTokens,
  toMostCommonToken,
  inverse,
} = require("./day_3_1");

function toOxygen(input, index = 0) {
  const mostCommonTokens = transpose(toTokens(input)).map(
    toMostCommonToken("1")
  );

  const filtered = input.filter(
    (current) => current.charAt(index) === mostCommonTokens[index]
  );

  if (filtered.length <= 1) {
    return parseInt(filtered[0], 2);
  }

  return toOxygen(filtered, index + 1);
}

function toCo2(input, index = 0) {
  const mostCommonTokens = inverse(
    transpose(toTokens(input)).map(toMostCommonToken("1"))
  );

  const filtered = input.filter(
    (current) => current.charAt(index) === mostCommonTokens[index]
  );

  if (filtered.length <= 1) {
    return parseInt(filtered[0], 2);
  }

  return toCo2(filtered, index + 1);
}

function lifeSupportRating(input) {
  const oxygenRating = toOxygen(input);
  const co2Rating = toCo2(input);

  return oxygenRating * co2Rating;
}

console.log(lifeSupportRating(diagnosticInputs));

module.exports = {
  toOxygen,
  toCo2,
  lifeSupportRating,
};
