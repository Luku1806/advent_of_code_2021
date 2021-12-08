const fs = require("fs");

function toSignals(input) {
  return input
    .split("\n")
    .map((line) => line.split("|"))
    .map((line) => ({
      signal: line[0].trim().split(/\s/),
      digits: line[1].trim().split(/\s/),
    }));
}

function digitForSegments(segments) {
  const mapping = [-1, 2, -1, -1, 4, -1, -1, 3, 7, -1];
  return mapping.findIndex((count) => count === segments);
}

function isSearchedDigit(segments) {
  return digitForSegments(segments) !== -1;
}

function countSegments(signals) {
  return signals
    .flatMap(({ digits }) => digits)
    .reduce((sum, digit) => (isSearchedDigit(digit.length) ? sum + 1 : sum), 0);
}

function count() {
  const signalsFile = fs.readFileSync("./signals.txt", "utf-8");
  const signals = toSignals(signalsFile);
  return countSegments(signals);
}

// console.log(count());

module.exports = {
  toSignals,
};
