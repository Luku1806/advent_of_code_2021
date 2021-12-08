const fs = require("fs");
const { toSignals } = require("./day_8_1");
const { sum } = require("../utils");

function sortCharacters(string) {
  return string.split("").sort().join("");
}

function containSameCharacters(string1, string2) {
  return sortCharacters(string1) === sortCharacters(string2);
}

function byLength(length) {
  return (command) => command.length === length;
}

function byContainsAllCharacters(value, length) {
  return (command) =>
    value.split("").every((character) => command.includes(character)) &&
    command.length === length;
}

function without(...elements) {
  return (current) => elements.every((element) => element !== current);
}

function decodeSignal(signal) {
  const one = signal.find(byLength(2));
  const four = signal.find(byLength(4));
  const seven = signal.find(byLength(3));
  const eight = signal.find(byLength(7));

  const fourWithoutOne = four
    .split("")
    .filter((x) => !one.includes(x))
    .join("");

  const three = signal.find(byContainsAllCharacters(one, 5));

  const five = signal
    .filter(without(three))
    .find(byContainsAllCharacters(fourWithoutOne, 5));

  const two = signal.filter(without(three, five)).find(byLength(5));

  const nine = signal.find(byContainsAllCharacters(four, 6));
  const six = signal.filter(without(nine)).find(byContainsAllCharacters(6));
  const zero = signal.filter(without(nine, six)).find(byLength(6));

  return [zero, one, two, three, four, five, six, seven, eight, nine];
}

function decodeDigits({ signalMap, digits }) {
  return digits.reduce(
    (result, digit) =>
      result +
      signalMap.findIndex((command) => containSameCharacters(command, digit)),
    ""
  );
}

function decodeSignals(signals) {
  return signals
    .map(({ signal, digits }) => ({ signalMap: decodeSignal(signal), digits }))
    .reduce((values, current) => [...values, decodeDigits(current)], [])
    .map((number) => Number.parseInt(number, 10));
}

function count() {
  const signalsFile = fs.readFileSync("./signals.txt", "utf-8");
  const signals = toSignals(signalsFile);
  const displayValues = decodeSignals(signals);

  return sum(displayValues);
}

console.log(count());
